using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutoringAnnouncementCareerService : ITutoringAnnouncementCareerService
    {
        private readonly ITutoringAnnouncementCareerRepository _tutoringAnnouncementCareerRepository;

        public TutoringAnnouncementCareerService(ITutoringAnnouncementCareerRepository tutoringAnnouncementCareerRepository)
        {
            _tutoringAnnouncementCareerRepository = tutoringAnnouncementCareerRepository;
        }

        public async Task DeleteByTutoringAnnouncementId(Guid tutoringAnnouncementId)
            => await _tutoringAnnouncementCareerRepository.DeleteByTutoringAnnouncementId(tutoringAnnouncementId);

        public async Task InsertRange(IEnumerable<TutoringAnnouncementCareer> tutoringAnnouncementCareers)
            => await _tutoringAnnouncementCareerRepository.InsertRange(tutoringAnnouncementCareers);
    }
}
