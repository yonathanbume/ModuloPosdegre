using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IChannelContactService
    {
       Task<object> GetSelect2TypeChannels(Guid companyId);
       Task<IEnumerable<ChannelContact>> GetAll();
       Task<IEnumerable<ChannelContact>> GetTypeChannelsByCompany(Guid companyId);
       Task Delete(ChannelContact channelContact);
       Task DeleteRange(IEnumerable<ChannelContact> channelContacts);
       Task InsertRange(IEnumerable<ChannelContact> channelContacts);
    }
}
