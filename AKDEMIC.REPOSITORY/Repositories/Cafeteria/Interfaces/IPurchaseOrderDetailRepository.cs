using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces
{
    public interface IPurchaseOrderDetailRepository : IRepository<PurchaseOrderDetail>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetPurchaseOrderDetailDatatable(DataTablesStructs.SentParameters sentParameters, Guid? purchaseOrderId, string searchValue = null);
        Task<object> ListOrderDetailById(Guid purchaseOrderId);
        Task<bool> GetQuantityBySupplyFinished(PurchaseOrder purchaseOrder, Guid providerSupplyId, decimal quantity);
    }
}
