using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Interfaces
{
    public interface ISupplyOrderService
    {
        Task Add(SupplyOrder supplyOrder);
        Task Update(SupplyOrder supplyOrder);
        Task<DataTablesStructs.ReturnedData<object>> GetSupplyOrderDatatable(DataTablesStructs.SentParameters sentParameters, Guid? providerId, string searchValue = null);
        Task<bool> ExceededCapacity(PurchaseOrder purchaseOrder, Guid providerSupplyId, decimal Quantity);
        Task ChangeState(Guid supplyOrderId);
        Task<DataTablesStructs.ReturnedData<object>> GetPurchaseOrderDetailReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? providerId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetSupplyOrderDetailReportDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<List<PurchaseOrderDetailReportDataTemplate>> GetPurchaseOrderDetailReportData(Guid? providerId);
        Task<List<SupplyOrderDetailReportDataTemplate>> GetSupplyOrderDetailReportData();
        Task<List<ReferralGuideTemplate>> GetReferralGuideData(Guid supplyOrderId);
    }
}
