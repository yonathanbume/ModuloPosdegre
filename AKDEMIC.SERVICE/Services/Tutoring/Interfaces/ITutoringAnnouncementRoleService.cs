using AKDEMIC.ENTITIES.Models.Tutoring;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface ITutoringAnnouncementRoleService
    {
        Task InsertRange(IEnumerable<TutoringAnnouncementRole> tutoringAnnouncementRoles);
        Task DeleteByTutoringAnnouncementId(Guid tutoringAnnouncementId);
    }
}
