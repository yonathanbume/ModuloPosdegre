using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Interfaces
{
    public interface IPurchaseOrderService
    {
        Task Add(PurchaseOrder purchaseOrder );
        Task Update(PurchaseOrder purchaseOrder);
        Task<DataTablesStructs.ReturnedData<object>> GetPurchaseOrderDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<object> GetPurchaseOrderSelect();
        Task<PurchaseOrder> Get(Guid purchaseOrderId);
        Task<PurchaseOrder> GetWithDetails(Guid purchaseOrderId);
        Task<bool> NumberExist(int number);
    }
}
