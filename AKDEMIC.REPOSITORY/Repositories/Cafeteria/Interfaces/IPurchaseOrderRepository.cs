using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces
{
    public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetPurchaseOrderDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<object> GetPurchaseOrderSelect();
        Task<PurchaseOrder> GetWithDetails(Guid purchaseOrderId);
        Task<bool> NumberExist(int number);
        
    }
}
