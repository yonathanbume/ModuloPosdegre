using AKDEMIC.ENTITIES.Models.Evaluation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Interfaces
{
    public interface ICourseConferenceExhibitorService
    {
        Task<IEnumerable<CourseConferenceExhibitor>> GetAllByCourseConferenceIdAsync(Guid courseCoferenceId);
        Task DeleteRange(IEnumerable<CourseConferenceExhibitor> entities);
    }
}
