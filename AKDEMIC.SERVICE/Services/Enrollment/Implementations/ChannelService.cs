using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class ChannelService : IChannelService
    {
        private readonly IChannelRepository _channelRepository;

        public ChannelService(IChannelRepository channelRepository)
        {
            _channelRepository = channelRepository;
        }

        public async Task DeleteById(Guid id) => await _channelRepository.DeleteById(id);

        public async Task<Channel> Get(Guid id) => await _channelRepository.Get(id);

        public async Task<IEnumerable<Channel>> GetAll() => await _channelRepository.GetAll();

        public async Task<List<Channel>> GetAllOnlyWithCareers() => await _channelRepository.GetAllOnlyWithCareers();

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
            => await _channelRepository.GetDataDatatable(sentParameters, searchValue);

        public async Task Insert(Channel channel) => await _channelRepository.Insert(channel);

        public async Task Update(Channel channel) => await _channelRepository.Update(channel);
    }
}
