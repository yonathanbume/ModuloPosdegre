using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Tutoring;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface ITutoringAnnouncementService
    {
        Task Insert(TutoringAnnouncement tutoringAnnouncement);
        Task Update(TutoringAnnouncement tutoringAnnouncement);
        Task DeleteById(Guid tutoringAnnouncementId);
        Task<TutoringAnnouncement> Get(Guid tutoringAnnouncementId);
        Task<TutoringAnnouncement> GetWithCareersAndRolesById(Guid tutoringAnnouncementId);
        Task<DataTablesStructs.ReturnedData<TutoringAnnouncement>> GetTutoringAnnouncementsDatatable(DataTablesStructs.SentParameters sentParameters, byte system, string searchValue = null, bool? published = null, bool? isCoordinatorAdmin = null, string userId = null);
        Task<IEnumerable<TutoringAnnouncement>> GetAllByRolesAndCareer(string[] roleNames, byte system , List<Guid> careers);
        Task<IEnumerable<TutoringAnnouncement>> GetAllByRoles(string[] roleNames);
    }
}
