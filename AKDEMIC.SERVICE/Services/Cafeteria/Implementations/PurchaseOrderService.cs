using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.SERVICE.Services.Cafeteria.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Implementations
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task Add(PurchaseOrder purchaseOrder)
        {
            await _purchaseOrderRepository.Add(purchaseOrder);
        }

        public async Task<PurchaseOrder> Get(Guid purchaseOrderId)
        {
            return await _purchaseOrderRepository.Get(purchaseOrderId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPurchaseOrderDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _purchaseOrderRepository.GetPurchaseOrderDatatable(sentParameters, searchValue);
        }

        public async Task<object> GetPurchaseOrderSelect()
        {
            return await _purchaseOrderRepository.GetPurchaseOrderSelect();
        }

        public async Task<PurchaseOrder> GetWithDetails(Guid purchaseOrderId)
        {
            return await _purchaseOrderRepository.GetWithDetails(purchaseOrderId);
        }

        public async Task<bool> NumberExist(int number)
        {
            return await _purchaseOrderRepository.NumberExist(number);
        }

        public async Task Update(PurchaseOrder purchaseOrder)
        {
            await _purchaseOrderRepository.Update(purchaseOrder);
        }
    }
}
