using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Templates;
using AKDEMIC.SERVICE.Services.Cafeteria.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Implementations
{
    public class SupplyOrderService : ISupplyOrderService
    {
        private readonly ISupplyOrderRepository _supplyOrderRepository;
        public SupplyOrderService(ISupplyOrderRepository supplyOrderRepository)
        {
            _supplyOrderRepository = supplyOrderRepository;
        }

        public async Task Add(SupplyOrder supplyOrder)
        {
            await _supplyOrderRepository.Add(supplyOrder);
        }

        public async Task ChangeState(Guid supplyOrderId)
        {
            await _supplyOrderRepository.ChangeState(supplyOrderId);
        }

        public async Task<bool> ExceededCapacity(PurchaseOrder purchaseOrder, Guid supplyId, decimal Quantity)
        {
            return await _supplyOrderRepository.ExceededCapacity(purchaseOrder, supplyId, Quantity);
        }

        public async Task<List<PurchaseOrderDetailReportDataTemplate>> GetPurchaseOrderDetailReportData(Guid? providerId)
        {
            return await _supplyOrderRepository.GetPurchaseOrderDetailReportData(providerId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPurchaseOrderDetailReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? providerId, string searchValue = null)
        {
            return await _supplyOrderRepository.GetPurchaseOrderDetailReportDatatable(sentParameters, providerId, searchValue);
        }

        public async Task<List<ReferralGuideTemplate>> GetReferralGuideData(Guid supplyOrderId)
        {
            return await _supplyOrderRepository.GetReferralGuideData(supplyOrderId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSupplyOrderDatatable(DataTablesStructs.SentParameters sentParameters, Guid? providerId, string searchValue = null)
        {
            return await _supplyOrderRepository.GetSupplyOrderDatatable(sentParameters, providerId, searchValue);
        }

        public async Task<List<SupplyOrderDetailReportDataTemplate>> GetSupplyOrderDetailReportData()
        {
            return await _supplyOrderRepository.GetSupplyOrderDetailReportData();
        }

        public async  Task<DataTablesStructs.ReturnedData<object>> GetSupplyOrderDetailReportDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _supplyOrderRepository.GetSupplyOrderDetailReportDatatable(sentParameters, searchValue);
        }

        public async Task Update(SupplyOrder supplyOrder)
        {
            await _supplyOrderRepository.Update(supplyOrder);
        }
    }
}
