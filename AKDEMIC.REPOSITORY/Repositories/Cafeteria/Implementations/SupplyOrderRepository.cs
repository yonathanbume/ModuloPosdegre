using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Templates;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Implementations
{
    public class SupplyOrderRepository : Repository<SupplyOrder>, ISupplyOrderRepository
    {
        public SupplyOrderRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task ChangeState(Guid supplyOrderId)
        {
            var supplyOrder = await _context.SupplyOrders.Where(x => x.Id == supplyOrderId).FirstOrDefaultAsync();
            var supplyOrderDetails = await _context.SupplyOrderDetails.Where(x => x.SupplyOrderId == supplyOrder.Id).ToListAsync();

            var listKardex = new List<CafeteriaKardex>();
            var cafeteriaStockEntity = new CafeteriaStock();
            var kardex = new CafeteriaKardex();

            foreach (var supplyOrderDetailItem in supplyOrderDetails)
            {
                if (await _context.CafeteriaStocks.AnyAsync(x => (x.ProviderSupplyId == supplyOrderDetailItem.ProviderSupplyId) && (x.TurnId == supplyOrderDetailItem.TurnId)))
                {
                    cafeteriaStockEntity = await _context.CafeteriaStocks.Where(x => x.ProviderSupplyId == supplyOrderDetailItem.ProviderSupplyId && x.TurnId == supplyOrderDetailItem.TurnId).FirstOrDefaultAsync();
                    cafeteriaStockEntity.Quantity += supplyOrderDetailItem.Quantity;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    cafeteriaStockEntity = new CafeteriaStock()
                    {
                        ProviderSupplyId = supplyOrderDetailItem.ProviderSupplyId,
                        Quantity = supplyOrderDetailItem.Quantity,
                        TurnId = supplyOrderDetailItem.TurnId
                    };
                    await _context.CafeteriaStocks.AddAsync(cafeteriaStockEntity);
                }
                supplyOrderDetailItem.State = true;
                await _context.SaveChangesAsync();

            }

            foreach (var g in supplyOrderDetails.GroupBy(x => new { x.ProviderSupplyId }))
            {
                kardex = new CafeteriaKardex();
                kardex.Type = 1;
                kardex.Quantity = supplyOrderDetails.Where(x => x.ProviderSupplyId == g.Key.ProviderSupplyId).Sum(x => x.Quantity);
                kardex.ProviderSupplyId = g.Key.ProviderSupplyId;
                listKardex.Add(kardex);
            }

            supplyOrder.State = true;

            await _context.CafeteriaKardexes.AddRangeAsync(listKardex);
            await _context.SaveChangesAsync();

        }

        public async Task<bool> ExceededCapacity(PurchaseOrder purchaseOrder, Guid providerSupplyId, decimal Quantity)
        {
            var a = await _context.SupplyOrderDetails.Where(x => x.SupplyOrder.PurchaseOrderId == purchaseOrder.Id && x.ProviderSupplyId == providerSupplyId).SumAsync(x => x.Quantity) + Quantity;
            var b = purchaseOrder.PurchaseOrderDetails.Where(x => x.ProviderSupplyId == providerSupplyId).Sum(s => s.Quantity);
            return (a > b);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSupplyOrderDatatable(DataTablesStructs.SentParameters sentParameters, Guid? providerId, string searchValue = null)
        {
            var query = _context.SupplyOrders
                .Include(x => x.PurchaseOrder.Provider)
                .AsNoTracking();
            if (providerId.HasValue)
            {
                query = query.Where(x => x.PurchaseOrder.ProviderId == providerId.Value);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x =>
                x.PurchaseOrder.Provider.User.FullName.Contains(searchValue) ||
                x.GeneratedId.ToString().Contains(searchValue) ||
                x.PurchaseOrder.Provider.User.Document.Contains(searchValue));
            }

            Expression<Func<SupplyOrder, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                id = x.Id,
                x.PurchaseOrder.Provider.User.Document,
                PurchaseOrder = x.PurchaseOrder.Code,
                Code = x.Code + " " + x.GeneratedId.ToString(),
                FullName = (x.PurchaseOrder.Provider.User.PaternalSurname == null && x.PurchaseOrder.Provider.User.MaternalSurname == null) ? x.PurchaseOrder.Provider.User.Name : x.PurchaseOrder.Provider.User.FullName,
                state = x.State,
                CreatedAt = x.CreatedAt.ToLocalDateFormat(),
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPurchaseOrderDetailReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? providerId, string searchValue = null)
        {
            var query = _context.PurchaseOrderDetails
                .Include(x => x.ProviderSupply.Supply.UnitMeasurement)
                .Include(x => x.ProviderSupply.Supply.SupplyPackage)
                .AsNoTracking();

            if (providerId.HasValue)
                query = query.Where(x => x.ProviderSupply.ProviderId == providerId.Value);

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.ProviderSupply.Supply.UnitMeasurement.Description.Contains(searchValue)
                || x.ProviderSupply.Supply.Name.Contains(searchValue));
            }

            //var supplyOrderDetails = await _context.SupplyOrderDetails.Include(x => x.SupplyOrder).ToListAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new PurchaseOrderDetailReportDataTemplate
                {
                    supplyOrderGeneratedId = _context.SupplyOrderDetails.FirstOrDefault(s => s.SupplyOrder.PurchaseOrderId == x.PurchaseOrderId && s.ProviderSupplyId == x.ProviderSupplyId) != null ? _context.SupplyOrderDetails.FirstOrDefault(s => s.SupplyOrder.PurchaseOrderId == x.PurchaseOrderId && s.ProviderSupplyId == x.ProviderSupplyId).SupplyOrder.Code : string.Empty,
                    supplyName = x.ProviderSupply.Supply.Name,
                    providerSupplyGeneratedId = x.ProviderSupply.GeneratedId,
                    quantity = x.Quantity,
                    ummc = x.ProviderSupply.Supply.UnitMeasurement.Description,
                    status = (x.State) ? "Cumplido" : "Pendiente",
                    totalprice = (x.UnitPrice * x.Quantity).ToString("0.00"),
                    unitprice = (x.UnitPrice).ToString("0.00"),
                    quantity_solicited = _context.SupplyOrderDetails.FirstOrDefault(s => s.SupplyOrder.PurchaseOrderId == x.PurchaseOrderId && s.ProviderSupplyId == x.ProviderSupplyId) != null ? _context.SupplyOrderDetails.FirstOrDefault(s => s.SupplyOrder.PurchaseOrderId == x.PurchaseOrderId && s.ProviderSupplyId == x.ProviderSupplyId).Quantity : 0//(s => s.Quantity),
                })
            .ToListAsync();

            data.ForEach(item =>
            {
                item.total = (Convert.ToDecimal(item.unitprice) * item.quantity_solicited).ToString("0.00");
            });

            data = data.OrderBy(x => x.providerSupplyGeneratedId)
                       .ThenBy(x => x.supplyOrderGeneratedId).ToList();

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();

            //Expression<Func<PurchaseOrderDetail, dynamic>> selectPredicate = null;
            //selectPredicate = ;

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            //return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSupplyOrderDetailReportDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            var query = _context.PurchaseOrderDetails
            .Include(x => x.ProviderSupply.Provider.User)
                .Include(x => x.ProviderSupply.Supply.UnitMeasurement)
                .Include(x => x.ProviderSupply.Supply.SupplyPackage)
                .Include(x => x.ProviderSupply.Supply.SupplyPackage)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.ProviderSupply.Supply.UnitMeasurement.Description.Contains(searchValue)
                || x.ProviderSupply.Supply.Name.Contains(searchValue));
            }

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new SupplyOrderDetailReportDataTemplate
                {
                    purchaseId = x.PurchaseOrder.Code,
                    provider = x.PurchaseOrder.Provider.User.Dni + " - " + (x.PurchaseOrder.Provider.User.PaternalSurname == null && x.PurchaseOrder.Provider.User.MaternalSurname == null ? x.PurchaseOrder.Provider.User.Name : x.PurchaseOrder.Provider.User.FullName),
                    groupname = x.PurchaseOrder.Code + " - " + x.PurchaseOrder.Provider.User.Dni + " - " + (x.PurchaseOrder.Provider.User.PaternalSurname == null && x.PurchaseOrder.Provider.User.MaternalSurname == null ? x.PurchaseOrder.Provider.User.Name : x.PurchaseOrder.Provider.User.FullName),
                    supplyName = x.ProviderSupply.GeneratedId + " - " + x.ProviderSupply.Supply.Name,
                    ummc = x.ProviderSupply.Supply.UnitMeasurement.Description,
                    totalprice = (x.UnitPrice * x.Quantity).ToString("0.00"),
                    PurchaseOrderId = x.PurchaseOrderId,
                    ProviderSupplyId = x.ProviderSupplyId,
                    supplyOrderGeneratedId = _context.SupplyOrderDetails.Where(s => s.SupplyOrder.PurchaseOrderId == x.PurchaseOrderId && s.ProviderSupplyId == x.ProviderSupplyId).Select(s => s.SupplyOrder.Code).FirstOrDefault(),
                    quantity_solicited = _context.SupplyOrderDetails.Where(s => s.SupplyOrder.PurchaseOrderId == x.PurchaseOrderId && s.ProviderSupplyId == x.ProviderSupplyId).Select(s => s.Quantity).FirstOrDefault(),//(s => s.Quantity),
                    providerSupplyGeneratedId = x.ProviderSupply.GeneratedId,
                    PurchaseOrderDetailId = x.Id,
                })
                .ToListAsync();

            data.ForEach(item =>
            {
                item.total = GetItemTotal(1, item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId);
                item.balance = GetItemTotal(2, item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId);
                item.progress = GetItemTotal(3, item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId);
                item.m1 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.JANUARY);
                item.m2 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.FEBRAURY);
                item.m3 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.MARCH);
                item.m4 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.APRIL);
                item.m5 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.MAY);
                item.m6 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.JUNE);
                item.m7 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.JULY);
                item.m8 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.AUGUST);
                item.m9 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.SEPTEMBER);
                item.m10 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.OCTOBER);
                item.m11 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.NOVEMBER);
                item.m12 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.DECEMBER);
            });
            data = data.OrderBy(x => x.groupname)
                        .ThenBy(x => x.providerSupplyGeneratedId)
                        .ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<PurchaseOrderDetailReportDataTemplate>> GetPurchaseOrderDetailReportData(Guid? providerId)
        {
            var query = await _context.PurchaseOrderDetails
              .Where(x => x.ProviderSupply.ProviderId == providerId.Value)
              .Include(x => x.ProviderSupply.Supply.UnitMeasurement)
              .Include(x => x.ProviderSupply.Supply.SupplyPackage)
              .Select(x => new PurchaseOrderDetailReportDataTemplate
              {
                  supplyOrderGeneratedId = _context.SupplyOrderDetails.Where(s => s.SupplyOrder.PurchaseOrderId == x.PurchaseOrderId && s.ProviderSupplyId == x.ProviderSupplyId).Select(s => s.SupplyOrder.Code).FirstOrDefault(),
                  supplyName = x.ProviderSupply.Supply.Name,
                  providerSupplyGeneratedId = x.ProviderSupply.GeneratedId,
                  quantity = x.Quantity,
                  ummc = x.ProviderSupply.Supply.UnitMeasurement.Description,
                  status = (x.State) ? "Cumplido" : "Pendiente",
                  totalprice = (x.UnitPrice * x.Quantity).ToString("0.00"),
                  unitprice = (x.UnitPrice).ToString("0.00"),
                  quantity_solicited = _context.SupplyOrderDetails.Where(s => s.SupplyOrder.PurchaseOrderId == x.PurchaseOrderId && s.ProviderSupplyId == x.ProviderSupplyId).Select(s => s.Quantity).FirstOrDefault(),//(s => s.Quantity),
              })
              .ToListAsync();

            query.ForEach(item =>
            {
                item.total = (Convert.ToDecimal(item.unitprice) * item.quantity_solicited).ToString("0.00");
            });

            var data = query
                .OrderBy(x => x.providerSupplyGeneratedId)
                .ThenBy(x => x.supplyOrderGeneratedId)
                .ToList();

            return query;
        }

        public async Task<List<SupplyOrderDetailReportDataTemplate>> GetSupplyOrderDetailReportData()
        {
            var query = _context.PurchaseOrderDetails
                .Include(x => x.ProviderSupply.Provider.User)
                .Include(x => x.ProviderSupply.Supply.UnitMeasurement)
                .Include(x => x.ProviderSupply.Supply.SupplyPackage)
                .Include(x => x.ProviderSupply.Supply.SupplyPackage)
                .AsNoTracking();

            var dataDB = await query
                .Select(x => new
                {
                    purchaseId = x.PurchaseOrder.Code,

                    purchaseCode = x.PurchaseOrder.Code,
                    purchaseUserDni = x.PurchaseOrder.Provider.User.Dni,
                    purchaseUserPaternalSurname = x.PurchaseOrder.Provider.User.PaternalSurname,
                    purchaseUserMaternalSurname = x.PurchaseOrder.Provider.User.MaternalSurname,
                    purchaseUserName = x.PurchaseOrder.Provider.User.Name,
                    purchaseUserFullName = x.PurchaseOrder.Provider.User.FullName,

                    supplyName = x.ProviderSupply.Supply.Name,
                    ummc = x.ProviderSupply.Supply.UnitMeasurement.Description,

                    x.UnitPrice,
                    x.Quantity,

                    supplyOrderGeneratedId = _context.SupplyOrderDetails.Where(s => s.SupplyOrder.PurchaseOrderId == x.PurchaseOrderId && s.ProviderSupplyId == x.ProviderSupplyId).Select(s => s.SupplyOrder.Code).FirstOrDefault(),
                    quantity_solicited = _context.SupplyOrderDetails.Where(s => s.SupplyOrder.PurchaseOrderId == x.PurchaseOrderId && s.ProviderSupplyId == x.ProviderSupplyId).Select(s => s.Quantity).FirstOrDefault(),//(s => s.Quantity),
                    providerSupplyGeneratedId = x.ProviderSupply.GeneratedId,
                    PurchaseOrderDetailId = x.Id,
                    x.PurchaseOrderId,
                    x.ProviderSupplyId,
                })
                .ToListAsync();


            var data = dataDB
                .Select(x => new SupplyOrderDetailReportDataTemplate
                {
                    purchaseId = x.purchaseId,
                    provider = x.purchaseUserDni + " - " + (x.purchaseUserPaternalSurname == null && x.purchaseUserMaternalSurname == null ? x.purchaseUserName : x.purchaseUserFullName),
                    groupname = x.purchaseCode + " - " + x.purchaseUserDni + " - " + (x.purchaseUserPaternalSurname == null && x.purchaseUserMaternalSurname == null ? x.purchaseUserName : x.purchaseUserFullName),
                    supplyName = x.supplyName,
                    ummc = x.ummc,
                    totalprice = (x.UnitPrice * x.Quantity).ToString("0.00"),
                    supplyOrderGeneratedId = x.supplyOrderGeneratedId,
                    quantity_solicited = x.quantity_solicited,
                    providerSupplyGeneratedId = x.providerSupplyGeneratedId,
                    PurchaseOrderDetailId = x.PurchaseOrderDetailId,
                    PurchaseOrderId = x.PurchaseOrderId,
                    ProviderSupplyId = x.ProviderSupplyId,
                })
                .OrderBy(x => x.groupname)
                .ThenBy(x => x.providerSupplyGeneratedId)
                .ToList();

            data.ForEach(item =>
            {
                item.total = GetItemTotal(1, item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId);
                item.balance = GetItemTotal(2, item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId);
                item.progress = GetItemTotal(3, item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId);
                item.m1 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.JANUARY);
                item.m2 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.FEBRAURY);
                item.m3 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.MARCH);
                item.m4 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.APRIL);
                item.m5 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.MAY);
                item.m6 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.JUNE);
                item.m7 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.JULY);
                item.m8 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.AUGUST);
                item.m9 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.SEPTEMBER);
                item.m10 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.OCTOBER);
                item.m11 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.NOVEMBER);
                item.m12 = GetMonthlyPaidPrice(item.PurchaseOrderDetailId, item.PurchaseOrderId, item.ProviderSupplyId, ConstantHelpers.MONTHS.DECEMBER);
            });
            return data;
        }

        public async Task<List<ReferralGuideTemplate>> GetReferralGuideData(Guid supplyOrderId)
        {
            var query = await _context.SupplyOrderDetails
                .Where(x => x.SupplyOrderId == supplyOrderId)
                .Select(x => new ReferralGuideTemplate
                {
                    package = x.ProviderSupply.Supply.SupplyPackage.Name,
                    supplyOrderGeneratedId = x.SupplyOrder.Code,
                    supplyName = x.ProviderSupply.Supply.Name,
                    providerSupplyGeneratedId = x.ProviderSupply.GeneratedId,
                    quantity_solicited = x.Quantity,
                    ummc = x.ProviderSupply.Supply.UnitMeasurement.Description,
                    status = (x.State) ? "Cumplido" : "Pendiente",
                    totalprice = (x.SupplyOrder.PurchaseOrder.PurchaseOrderDetails.Where(y => y.ProviderSupplyId == x.ProviderSupplyId).Select(d => d.UnitPrice).FirstOrDefault() * x.Quantity).ToString("0.00"),
                    unitprice = (x.SupplyOrder.PurchaseOrder.PurchaseOrderDetails.Where(y => y.ProviderSupplyId == x.ProviderSupplyId).Select(d => d.UnitPrice).FirstOrDefault()).ToString("0.00"),
                    //unitprice = (x.UnitPrice).ToString("0.00"),
                    //quantity_solicited = _context.SupplyOrderDetails.Where(s => s.SupplyOrder.PurchaseOrderId == x.PurchaseOrderId && s.ProviderSupplyId == x.ProviderSupplyId).FirstOrDefault().Quantity//(s => s.Quantity),
                })
                .ToListAsync();

            query = query
                .OrderBy(x => x.package)
                .ThenBy(x => x.supplyName)
                .ToList();

            return query;
        }

        //obtener pagado por mes
        private decimal GetMonthlyPaidPrice(Guid PurchaseOrderDetailId, Guid PurchaseOrderId, Guid ProviderSupplyId, int month)
        {
            //detalle de la orden qe tiene ese producto
            var purchaseOrder = _context.PurchaseOrderDetails.Where(x => x.Id == PurchaseOrderDetailId).FirstOrDefault();
            var unitPrice = (purchaseOrder == null ? 0 : purchaseOrder.UnitPrice);

            //cantidad solicitada
            var solicited = _context.SupplyOrderDetails.Where(s => s.SupplyOrder.PurchaseOrderId == PurchaseOrderId && s.ProviderSupplyId == ProviderSupplyId && s.SupplyOrder.CreatedAt.Value.Month == month);
            var qty = (solicited == null ? 0 : solicited.Sum(x => x.Quantity));

            //pagado en esa orden
            var paid = qty * unitPrice;
            return paid;
        }

        private decimal GetItemTotal(int type, Guid PurchaseOrderDetailId, Guid PurchaseOrderId, Guid ProviderSupplyId)
        {
            //detalle de la orden qe tiene ese producto
            var purchaseOrder = _context.PurchaseOrderDetails.Where(x => x.Id == PurchaseOrderDetailId).FirstOrDefault();
            var total = purchaseOrder.Quantity * purchaseOrder.UnitPrice;

            var unitPrice = (purchaseOrder == null ? 0 : purchaseOrder.UnitPrice);
            switch (type)
            {
                case 1:
                    return total;
                case 2:
                    //balance
                    var solicited = _context.SupplyOrderDetails.Where(s => s.SupplyOrder.PurchaseOrderId == PurchaseOrderId && s.ProviderSupplyId == ProviderSupplyId);
                    var qty = (solicited == null ? 0 : solicited.Sum(x => x.Quantity));
                    //pagado en esa orden
                    var paid1 = qty * unitPrice;
                    return total - paid1;
                case 3:
                    var solicited2 = _context.SupplyOrderDetails.Where(s => s.SupplyOrder.PurchaseOrderId == PurchaseOrderId && s.ProviderSupplyId == ProviderSupplyId);
                    var qty2 = (solicited2 == null ? 0 : solicited2.Sum(x => x.Quantity));
                    //pagado en esa orden
                    var paid = qty2 * unitPrice;

                    var progress = paid / total * 100;
                    return progress;
            }
            return 0;
        }
    }
}
