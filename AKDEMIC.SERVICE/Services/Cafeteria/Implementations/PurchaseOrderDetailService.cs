using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.SERVICE.Services.Cafeteria.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Implementations
{
    public class PurchaseOrderDetailService : IPurchaseOrderDetailService
    {
        private readonly IPurchaseOrderDetailRepository _purchaseOrderDetailRepository;
        public PurchaseOrderDetailService(IPurchaseOrderDetailRepository purchaseOrderDetailRepository)
        {
            _purchaseOrderDetailRepository = purchaseOrderDetailRepository;
        }

        public async Task Add(PurchaseOrderDetail purchaseOrderDetail)
        {
            await _purchaseOrderDetailRepository.Add(purchaseOrderDetail);
        }

        public async Task AddRange(List<PurchaseOrderDetail> purchaseOrderDetails)
        {
            await _purchaseOrderDetailRepository.AddRange(purchaseOrderDetails);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPurchaseOrderDetailDatatable(DataTablesStructs.SentParameters sentParameters, Guid? purchaseOrderId, string searchValue = null)
        {
            return await _purchaseOrderDetailRepository.GetPurchaseOrderDetailDatatable(sentParameters, purchaseOrderId, searchValue);
        }

        public async Task<bool> GetQuantityBySupplyFinished(PurchaseOrder purchaseOrder, Guid supplyId, decimal quantity)
        {
            return await _purchaseOrderDetailRepository.GetQuantityBySupplyFinished(purchaseOrder, supplyId, quantity);
        }

        public async Task<object> ListOrderDetailById(Guid purchaseOrderId)
        {
            return await _purchaseOrderDetailRepository.ListOrderDetailById(purchaseOrderId);
        }

        public async Task Update(PurchaseOrderDetail purchaseOrderDetail)
        {
            await _purchaseOrderDetailRepository.Update(purchaseOrderDetail);
        }

        public async Task UpdateRange(List<PurchaseOrderDetail> purchaseOrderDetails)
        {
            await _purchaseOrderDetailRepository.UpdateRange(purchaseOrderDetails);
        }
    }
}
