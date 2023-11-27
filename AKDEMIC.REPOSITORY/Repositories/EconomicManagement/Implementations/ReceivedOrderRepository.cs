using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class ReceivedOrderRepository : Repository<ReceivedOrder>, IReceivedOrderRepository
    {
        public ReceivedOrderRepository(AkdemicContext context) : base(context) { }

        public async Task<ReceivedOrder> GetByOrderIdAndUserRequirement(Guid orderId, Guid userRequirementId)
            => await _context.ReceivedOrders.Where(x => x.OrderId == orderId && x.UserRequirementId == userRequirementId).FirstOrDefaultAsync();
        public async Task<bool> AnyByOrderIdAndUserRequirement(Guid orderId, Guid userRequirementId)
            => await _context.ReceivedOrders.AnyAsync(x => x.OrderId == orderId && x.UserRequirementId == userRequirementId);
        public async Task<bool> AnyByOrderId(Guid orderId)
            => await _context.ReceivedOrders.AnyAsync(x => x.OrderId == orderId);
    }
}
