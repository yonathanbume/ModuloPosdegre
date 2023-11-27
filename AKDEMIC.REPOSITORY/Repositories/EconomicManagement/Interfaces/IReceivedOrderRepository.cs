using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IReceivedOrderRepository : IRepository<ReceivedOrder>
    {
        Task<ReceivedOrder> GetByOrderIdAndUserRequirement(Guid orderId, Guid UserRequirementId);
        Task<bool> AnyByOrderIdAndUserRequirement(Guid orderId, Guid UserRequirementId);
        Task<bool> AnyByOrderId(Guid orderId);
    }
}
