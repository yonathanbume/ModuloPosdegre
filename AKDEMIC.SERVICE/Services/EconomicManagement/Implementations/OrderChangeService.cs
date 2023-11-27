using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class OrderChangeService : IOrderChangeService
    {
        private readonly IOrderChangeRepository _orderChangeRepository;

        public OrderChangeService(IOrderChangeRepository orderChangeRepository)
        {
            _orderChangeRepository = orderChangeRepository;
        }

        public async Task<int> Count()
        {
            return await _orderChangeRepository.Count();
        }

        public async Task<OrderChangeHistory> Get(Guid id)
        {
            return await _orderChangeRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<OrderChangeHistory>> GetOrderChangesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _orderChangeRepository.GetOrderChangesDatatable(sentParameters, searchValue);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetOrderHistory(DataTablesStructs.SentParameters sentParameters, Guid orderId)
            => await _orderChangeRepository.GetOrderHistory(sentParameters, orderId);
        public async Task Delete(OrderChangeHistory orderChange) =>
            await _orderChangeRepository.Delete(orderChange);

        public async Task Insert(OrderChangeHistory orderChange) =>
            await _orderChangeRepository.Insert(orderChange);

        public async Task Update(OrderChangeHistory orderChange) =>
            await _orderChangeRepository.Update(orderChange);

        public async Task Add(OrderChangeHistory orderChange)
        {
            await _orderChangeRepository.Add(orderChange);
        }
    }
}
