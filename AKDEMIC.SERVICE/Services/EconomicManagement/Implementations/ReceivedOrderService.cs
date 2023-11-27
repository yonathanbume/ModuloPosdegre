using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class ReceivedOrderService : IReceivedOrderService
    {
        private readonly IReceivedOrderRepository _receivedOrderRepository;

        public ReceivedOrderService(IReceivedOrderRepository receivedOrderRepository)
        {
            _receivedOrderRepository = receivedOrderRepository;
        }

        public async Task<int> Count()
        {
            return await _receivedOrderRepository.Count();
        }

        public async Task<ReceivedOrder> Get(Guid id)
        {
            return await _receivedOrderRepository.Get(id);
        }

        public async Task Delete(ReceivedOrder receivedOrder) =>
            await _receivedOrderRepository.Delete(receivedOrder);

        public async Task Insert(ReceivedOrder receivedOrder) =>
            await _receivedOrderRepository.Insert(receivedOrder);

        public async Task Update(ReceivedOrder receivedOrder) =>
            await _receivedOrderRepository.Update(receivedOrder);

        public async Task Add(ReceivedOrder receivedOrder)
        {
            await _receivedOrderRepository.Add(receivedOrder);
        }
        public async Task InsertRange(List<ReceivedOrder> receivedOrders)
            => await _receivedOrderRepository.InsertRange(receivedOrders);

        public async Task<ReceivedOrder> GetByOrderIdAndUserRequirement(Guid orderId, Guid UserRequirementId)
            => await _receivedOrderRepository.GetByOrderIdAndUserRequirement(orderId, UserRequirementId);
        public async Task<bool> AnyByOrderIdAndUserRequirement(Guid orderId, Guid UserRequirementId)
            => await _receivedOrderRepository.AnyByOrderIdAndUserRequirement(orderId, UserRequirementId);
        public async Task<bool> AnyByOrderId(Guid orderId)
            => await _receivedOrderRepository.AnyByOrderId(orderId);
    }
}
