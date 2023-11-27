using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class ChannelCareerService : IChannelCareerService
    {
        private readonly IChannelCareerRepository _channelCareerRepository;

        public ChannelCareerService(IChannelCareerRepository channelCareerRepository)
        {
            _channelCareerRepository = channelCareerRepository;
        }

        public Task Delete(ChannelCareer channelCareer) => _channelCareerRepository.Delete(channelCareer);

        public async Task<ChannelCareer> Get(Guid channelId, Guid careerId) => await _channelCareerRepository.Get(channelId, careerId);

        public async Task<object> GetAcademicProgramsSelect2ByChannelId(Guid id)
            => await _channelCareerRepository.GetAcademicProgramsSelect2ByChannelId(id);

        public async Task<IEnumerable<ChannelCareer>> GetAllByChannelId(Guid id, Guid? appTermAdmissionTypeId)
        {
            return await _channelCareerRepository.GetAllByChannelId(id, appTermAdmissionTypeId);
        }

        public async Task<IEnumerable<ChannelCareer>> GetAllByChannelId(Guid id, Guid applicationTermId, Guid admissionTypeId)
        {
            return await _channelCareerRepository.GetAllByChannelId(id, applicationTermId, admissionTypeId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetChannelCareersDatatable(DataTablesStructs.SentParameters sentParameters, Guid channelId)
            => await _channelCareerRepository.GetChannelCareersDatatable(sentParameters, channelId);

        public async Task Insert(ChannelCareer channelCareer) => await _channelCareerRepository.Insert(channelCareer);
    }
}
