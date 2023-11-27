using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Implementations
{
    public class PurchaseOrderDetailRepository : Repository<PurchaseOrderDetail>, IPurchaseOrderDetailRepository
    {
        public PurchaseOrderDetailRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPurchaseOrderDetailDatatable(DataTablesStructs.SentParameters sentParameters, Guid? purchaseOrderId, string searchValue = null)
        {
            var query = _context.PurchaseOrderDetails
                .Include(x => x.ProviderSupply.Supply.UnitMeasurement)
                .Include(x => x.ProviderSupply.Supply.SupplyPackage)
                .AsNoTracking();
            if (purchaseOrderId.HasValue)
            {
                query = query.Where(x => x.PurchaseOrderId == purchaseOrderId);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.ProviderSupply.Supply.UnitMeasurement.Description.Contains(searchValue)
                || x.ProviderSupply.Supply.Name.Contains(searchValue));
            }

            //var supplyOrderDetails = _context.SupplyOrderDetails.Where(s => s.SupplyOrder.PurchaseOrderId == x.Id).

            Expression<Func<PurchaseOrderDetail, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                supplyName = x.ProviderSupply.Supply.Name,
                x.Quantity,
                ummc = x.ProviderSupply.Supply.UnitMeasurement.Description,
                status = (x.State) ? "Cumplido" : "Pendiente",
                price = (x.UnitPrice * x.Quantity).ToString("#.##"),
                quantity_solicited = _context.SupplyOrderDetails.Where(s => s.SupplyOrder.PurchaseOrderId == x.PurchaseOrderId && s.ProviderSupply.SupplyId == x.ProviderSupply.SupplyId).Sum(s => s.Quantity),
                supplyPackageName = x.ProviderSupply.Supply.SupplyPackage.Name
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<object> ListOrderDetailById(Guid purchaseOrderId)
        {
            var query = await _context.PurchaseOrderDetails.Include(x => x.ProviderSupply.Supply.UnitMeasurement).Where(x => x.PurchaseOrderId == purchaseOrderId)
                .Select(x => new
                {
                    supplyName = x.ProviderSupply.Supply.Name,
                    x.Quantity,
                    ummc = x.ProviderSupply.Supply.UnitMeasurement.Description,
                    status = x.State,
                    supplyId = x.ProviderSupply.SupplyId,
                    quantity_solicited = _context.SupplyOrderDetails.Where(s => s.SupplyOrder.PurchaseOrderId == x.PurchaseOrderId && s.ProviderSupply.SupplyId == x.ProviderSupply.SupplyId).Sum(s => s.Quantity)
                }).ToListAsync();
            return query;


        }
        public async Task<bool> GetQuantityBySupplyFinished(PurchaseOrder purchaseOrder, Guid providerSupplyId, decimal quantity)
        {
            var purchaseOrderDetailsQuantityBySupply = await _context.PurchaseOrderDetails.Where(x => x.PurchaseOrder == purchaseOrder && x.ProviderSupplyId == providerSupplyId).SumAsync(x => x.Quantity);
            var supplyOrderDetailsBySupplyQuantity = await _context.SupplyOrderDetails.Where(x => x.SupplyOrder.PurchaseOrder == purchaseOrder && x.ProviderSupplyId == providerSupplyId).SumAsync(x => x.Quantity) + quantity;
            return (purchaseOrderDetailsQuantityBySupply == supplyOrderDetailsBySupplyQuantity);
        }
    }
}
