using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IReceivedOrderHistoryService
    {
        Task<ReceivedOrderHistory> Get(Guid id);
        Task Add(ReceivedOrderHistory receivedOrderHistory);
        Task Delete(ReceivedOrderHistory receivedOrderHistory);
        Task Insert(ReceivedOrderHistory receivedOrderHistory);
        Task InsertRange(List<ReceivedOrderHistory> receivedOrderHistorys);
        Task Update(ReceivedOrderHistory receivedOrderHistory);
        
        Task<DataTablesStructs.ReturnedData<object>> GetDatatableByReceivedOrderId(DataTablesStructs.SentParameters sentParameters, Guid receivedOrderId);
    }
}
