using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutoringAnnouncementService : ITutoringAnnouncementService
    {
        private readonly ITutoringAnnouncementRepository _tutoringAnnouncementRepository;

        public TutoringAnnouncementService(ITutoringAnnouncementRepository tutoringAnnouncementRepository)
        {
            _tutoringAnnouncementRepository = tutoringAnnouncementRepository;
        }

        public async Task Insert(TutoringAnnouncement tutoringAnnouncement)
            => await _tutoringAnnouncementRepository.Insert(tutoringAnnouncement);

        public async Task DeleteById(Guid tutoringAnnouncementId)
            => await _tutoringAnnouncementRepository.DeleteById(tutoringAnnouncementId);

        public async Task<TutoringAnnouncement> Get(Guid tutoringAnnouncementId)
            => await _tutoringAnnouncementRepository.Get(tutoringAnnouncementId);

        public async Task<IEnumerable<TutoringAnnouncement>> GetAllByRolesAndCareer(string[] roleNames, byte system , List<Guid> careers)
            => await _tutoringAnnouncementRepository.GetAllByRolesAndCareer(roleNames, system, careers);

        public async Task Update(TutoringAnnouncement tutoringAnnouncement)
            => await _tutoringAnnouncementRepository.Update(tutoringAnnouncement);

        public async Task<TutoringAnnouncement> GetWithCareersAndRolesById(Guid tutoringAnnouncementId)
            => await _tutoringAnnouncementRepository.GetWithCareersAndRolesById(tutoringAnnouncementId);

        public async Task<DataTablesStructs.ReturnedData<TutoringAnnouncement>> GetTutoringAnnouncementsDatatable(DataTablesStructs.SentParameters sentParameters, byte system, string searchValue = null, bool? published = null, bool? isCoordinatorAdmin = null, string userId = null)
            => await _tutoringAnnouncementRepository.GetTutoringAnnouncementsDatatable(sentParameters, system, searchValue, published, isCoordinatorAdmin, userId);
        public async Task<IEnumerable<TutoringAnnouncement>> GetAllByRoles(string[] roleNames)
            => await _tutoringAnnouncementRepository.GetAllByRoles(roleNames);
    }
}
