using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class ChannelContactService: IChannelContactService
    {
        private readonly IChannelContactRepository _channelContactRepository;

        public ChannelContactService(IChannelContactRepository channelContactRepository)
        {
            _channelContactRepository = channelContactRepository;
        }

        public async Task Delete(ChannelContact channelContact)
        {
            await _channelContactRepository.Delete(channelContact);
        }

        public async Task DeleteRange(IEnumerable<ChannelContact> channelContacts)
        {
            await _channelContactRepository.DeleteRange(channelContacts);
        }

        public async Task<IEnumerable<ChannelContact>> GetAll()
        {
            return await _channelContactRepository.GetAll();
        }

        public async Task<object> GetSelect2TypeChannels(Guid companyId)
        {
            return await _channelContactRepository.GetSelect2TypeChannels(companyId);
        }       

        public async Task InsertRange(IEnumerable<ChannelContact> channelContacts)
        {
            await _channelContactRepository.InsertRange(channelContacts);
        }

        public async Task<IEnumerable<ChannelContact>> GetTypeChannelsByCompany(Guid companyId)
        {
            return await _channelContactRepository.GetTypeChannelsByCompany(companyId);
        }
    }
}
