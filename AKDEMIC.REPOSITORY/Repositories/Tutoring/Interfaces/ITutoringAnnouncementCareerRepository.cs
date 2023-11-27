using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ITutoringAnnouncementCareerRepository : IRepository<TutoringAnnouncementCareer>
    {
        Task DeleteByTutoringAnnouncementId(Guid tutoringAnnouncementId);
    }
}
