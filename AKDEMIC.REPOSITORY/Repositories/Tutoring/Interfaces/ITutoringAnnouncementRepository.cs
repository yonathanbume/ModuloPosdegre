using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ITutoringAnnouncementRepository : IRepository<TutoringAnnouncement>
    {
        Task<DataTablesStructs.ReturnedData<TutoringAnnouncement>> GetTutoringAnnouncementsDatatable(DataTablesStructs.SentParameters sentParameters, byte system, string searchValue = null, bool? published = null, bool? isCoordinatorAdmin = null, string userId = null);
        Task<IEnumerable<TutoringAnnouncement>> GetAllByRolesAndCareer(string[] roleNames, byte system, List<Guid> careers);
        Task<TutoringAnnouncement> GetWithCareersAndRolesById(Guid tutoringAnnouncementId);
        Task<IEnumerable<TutoringAnnouncement>> GetAllByRoles(string[] roleNames);
    }
}
