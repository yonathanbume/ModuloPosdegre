using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class ReceivedOrderHistoryService : IReceivedOrderHistoryService
    {
        private readonly IReceivedOrderHistoryRepository _receivedOrderHistoryRepository;

        public ReceivedOrderHistoryService(IReceivedOrderHistoryRepository receivedOrderHistoryRepository)
        {
            _receivedOrderHistoryRepository = receivedOrderHistoryRepository;
        }
        public async Task<ReceivedOrderHistory> Get(Guid id)
        {
            return await _receivedOrderHistoryRepository.Get(id);
        }

        public async Task Delete(ReceivedOrderHistory receivedOrderHistory) =>
            await _receivedOrderHistoryRepository.Delete(receivedOrderHistory);

        public async Task Insert(ReceivedOrderHistory receivedOrderHistory) =>
            await _receivedOrderHistoryRepository.Insert(receivedOrderHistory);

        public async Task Update(ReceivedOrderHistory receivedOrderHistory) =>
            await _receivedOrderHistoryRepository.Update(receivedOrderHistory);

        public async Task Add(ReceivedOrderHistory receivedOrderHistory)
        {
            await _receivedOrderHistoryRepository.Add(receivedOrderHistory);
        }
        public async Task InsertRange(List<ReceivedOrderHistory> receivedOrderHistorys)
            => await _receivedOrderHistoryRepository.InsertRange(receivedOrderHistorys);
        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatableByReceivedOrderId(DataTablesStructs.SentParameters sentParameters, Guid receivedOrderId)
            => await _receivedOrderHistoryRepository.GetDatatableByReceivedOrderId(sentParameters, receivedOrderId);

    }
}
