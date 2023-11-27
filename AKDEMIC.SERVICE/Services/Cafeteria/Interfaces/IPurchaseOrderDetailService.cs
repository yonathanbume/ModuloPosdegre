using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Interfaces
{
    public interface IPurchaseOrderDetailService
    {
        Task Add(PurchaseOrderDetail purchaseOrderDetail);
        Task UpdateRange(List<PurchaseOrderDetail> purchaseOrderDetails);
        Task AddRange(List<PurchaseOrderDetail> purchaseOrderDetails);
        Task<DataTablesStructs.ReturnedData<object>> GetPurchaseOrderDetailDatatable(DataTablesStructs.SentParameters sentParameters, Guid? purchaseOrderId, string searchValue = null);
        Task<object> ListOrderDetailById(Guid purchaseOrderId);
        Task<bool> GetQuantityBySupplyFinished(PurchaseOrder purchaseOrder, Guid providerSupplyId, decimal quantity);
        Task Update(PurchaseOrderDetail purchaseOrderDetail);
    }
}
