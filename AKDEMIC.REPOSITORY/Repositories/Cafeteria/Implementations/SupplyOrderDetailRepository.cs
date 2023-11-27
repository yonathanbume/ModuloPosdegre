using AKDEMIC.CORE.Extensions;
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
    public class SupplyOrderDetailRepository : Repository<SupplyOrderDetail>, ISupplyOrderDetailRepository
    {
        public SupplyOrderDetailRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<bool> GetQuantityFinished(Guid purchaseOrderId)
        {
            var purchaseOrderDetailsQuantity = await _context.PurchaseOrderDetails.Where(x => x.PurchaseOrderId == purchaseOrderId).SumAsync(x => x.Quantity);
            var supplyOrderDetailsQuantity = await _context.SupplyOrderDetails.Where(x => x.SupplyOrder.PurchaseOrderId == purchaseOrderId).SumAsync(x => x.Quantity);
            return (purchaseOrderDetailsQuantity == supplyOrderDetailsQuantity);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSupplyOrderDetailDatatable(DataTablesStructs.SentParameters sentParameters, Guid supplyOrderId, byte turnId, string searchValue = null)
        {
            Expression<Func<SupplyOrderDetail, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.ProviderSupply.Supply.Name); break;
                default:
                    orderByPredicate = ((x) => x.ProviderSupply.Supply.SupplyPackage.Name); break;
            }
            var query = _context.SupplyOrderDetails
                .Include(x => x.ProviderSupply.Supply.UnitMeasurement)
                .Include(x => x.ProviderSupply.Supply.SupplyPackage)
                .Where(x => x.SupplyOrderId == supplyOrderId && x.TurnId == turnId).AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.ProviderSupply.Supply.UnitMeasurement.Description.Contains(searchValue)
                || x.ProviderSupply.Supply.Name.Contains(searchValue));
            }

            Expression<Func<SupplyOrderDetail, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                supplyName = x.ProviderSupply.Supply.Name,
                supplyPackageName = x.ProviderSupply.Supply.SupplyPackage.Name,
                x.Quantity,
                ummc = x.ProviderSupply.Supply.UnitMeasurement.Description,
                status = (x.State) ? "Cumplido" : "Pendiente"
            };

            return await query.OrderByCondition(sentParameters.OrderDirection, orderByPredicate).ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<object> GetSupplyOrderDetailDatatableClientSide(Guid purchaseOrderId)
        {
            var query = await _context.PurchaseOrderDetails
                .Include(x => x.ProviderSupply.Supply.UnitMeasurement)
                .Include(x => x.ProviderSupply.Supply.SupplyPackage)
                .Where(x => x.PurchaseOrderId == purchaseOrderId)
                .Select(x => new
                {
                    supplyPackageName = x.ProviderSupply.Supply.SupplyPackage.Name,
                    supplyName = x.ProviderSupply.Supply.Name,
                    x.Quantity,
                    ummc = x.ProviderSupply.Supply.UnitMeasurement.Description,
                    status = x.State,
                    x.ProviderSupplyId,
                    quantity_solicited = _context.SupplyOrderDetails.Where(s => s.SupplyOrder.PurchaseOrderId == x.PurchaseOrderId && s.ProviderSupply.SupplyId == x.ProviderSupply.SupplyId).Sum(s => s.Quantity)
                }).ToListAsync();
            return query;
        }
    }
}
