using AKDEMIC.ENTITIES.Models.Tutoring;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface ITutoringAnnouncementCareerService
    {
        Task InsertRange(IEnumerable<TutoringAnnouncementCareer> tutoringAnnouncementCareers);
        Task DeleteByTutoringAnnouncementId(Guid tutoringAnnouncementId);
    }
}
