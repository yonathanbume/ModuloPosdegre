using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces
{
    public interface ICourseConferenceExhibitorRepository : IRepository<CourseConferenceExhibitor>
    {
        Task<IEnumerable<CourseConferenceExhibitor>> GetAllByCourseConferenceIdAsync(Guid courseCoferenceId);
    }
}
