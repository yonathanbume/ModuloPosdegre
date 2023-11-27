using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.SERVICE.Services.InterestGroup.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Implementations
{
    public class ConferenceUserService : IConferenceUserService
    {
        private readonly IConferenceUserRepository _conferenceUserRepository;

        public ConferenceUserService(IConferenceUserRepository conferenceUserRepository)
        {
            _conferenceUserRepository = conferenceUserRepository;
        }

        public async Task DeleteRange(IEnumerable<ConferenceUser> entities)
            => await _conferenceUserRepository.DeleteRange(entities);

        public async Task<IEnumerable<ConferenceUser>> GetConferenceUsersByConferenceId(Guid conferenceId)
            => await _conferenceUserRepository.GetConferenceUsersByConferenceId(conferenceId);

        public async Task Insert(ConferenceUser entity)
            => await _conferenceUserRepository.Insert(entity);

        public async Task InsertRange(IEnumerable<ConferenceUser> entities)
            => await _conferenceUserRepository.InsertRange(entities);

        
    }
}
