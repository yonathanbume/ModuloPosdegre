using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IChannelContactRepository : IRepository<ChannelContact>
    {
        Task<object> GetSelect2TypeChannels(Guid companyId);
        Task<IEnumerable<ChannelContact>> GetTypeChannelsByCompany(Guid companyId);
    }
}
