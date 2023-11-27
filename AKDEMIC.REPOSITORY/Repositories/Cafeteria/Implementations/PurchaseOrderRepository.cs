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
    public class PurchaseOrderRepository : Repository<PurchaseOrder>, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPurchaseOrderDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            var query = _context.PurchaseOrders.Include(x=>x.Provider.User).AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Provider.User.FullName.ToLower().Contains(searchValue.Trim().ToLower()));
            }

            Expression<Func<PurchaseOrder, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                id = x.Id,
                Code = x.Code,
                FullName =  ((x.Provider.User.PaternalSurname != null && x.Provider.User.MaternalSurname != null ) ? x.Provider.User.FullName : x.Provider.User.Name  ),
                x.State,
                CreatedAt = x.CreatedAt.ToLocalDateFormat(),
                StartDate = x.StartDate.ToLocalDateFormat(),
                EndDate = x.EndDate.ToLocalDateFormat()

            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<object> GetPurchaseOrderSelect()
        {
            var result = await _context.PurchaseOrders.Where(x => x.PurchaseOrderDetails.Any(s => s.State == false)).Select(x=> new {
                x.Id,
                text = x.Code

            }).ToListAsync();
            return result;
        }

        public async Task<PurchaseOrder> GetWithDetails(Guid purchaseOrderId)
        {
            return await _context.PurchaseOrders.Include(x => x.PurchaseOrderDetails).FirstOrDefaultAsync(x => x.Id == purchaseOrderId);
        }

        public async Task<bool> NumberExist(int number)
        {
            return await _context.PurchaseOrders.AnyAsync(x => x.Number == number);
        }
    }
}
