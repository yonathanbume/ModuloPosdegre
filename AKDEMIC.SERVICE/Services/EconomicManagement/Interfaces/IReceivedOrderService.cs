using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IReceivedOrderService
    {
        Task<int> Count();
        Task<ReceivedOrder> Get(Guid id);
        Task Add(ReceivedOrder receivedOrder);
        Task Delete(ReceivedOrder receivedOrder);
        Task Insert(ReceivedOrder receivedOrder);
        Task InsertRange(List<ReceivedOrder> receivedOrders);
        Task Update(ReceivedOrder receivedOrder);
        Task<ReceivedOrder> GetByOrderIdAndUserRequirement(Guid orderId, Guid UserRequirementId);
        Task<bool> AnyByOrderIdAndUserRequirement(Guid orderId, Guid UserRequirementId);
        Task<bool> AnyByOrderId(Guid orderId);
    }
}
