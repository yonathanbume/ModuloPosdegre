using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.SERVICE.Services.InterestGroup.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Implementations
{
    public class ConferenceService : IConferenceService
    {
        private readonly IConferenceRepository _conferenceRepository;

        public ConferenceService(IConferenceRepository conferenceRepository)
        {
            _conferenceRepository = conferenceRepository;
        }

        public async Task Delete(Conference conference)
            => await _conferenceRepository.Delete(conference);

        public async Task<Conference> Get(Guid id)
            => await _conferenceRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<Conference>> GetConferencesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId = null, string searchValue = null,string userAdminid = null)
            => await _conferenceRepository.GetConferencesDatatable(sentParameters, interestGroupId, searchValue,userAdminid);

        public async Task<DataTablesStructs.ReturnedData<ConferenceUser>> GetUserConferenceDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId = null, string searchValue = null, string userId = null)
            => await _conferenceRepository.GetUserConferenceDatatable(sentParameters, interestGroupId, searchValue, userId);

        public async Task<object> GetVideoConferenceReportChart(Guid? careerId = null, ClaimsPrincipal user = null)
            => await _conferenceRepository.GetVideoConferenceReportChart(careerId, user);

        public async Task Insert(Conference conference)
            => await _conferenceRepository.Insert(conference);
    }
}
