using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.SERVICE.Services.Evaluation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Implementations
{
    public class CourseConferenceExhibitorService : ICourseConferenceExhibitorService
    {
        private readonly ICourseConferenceExhibitorRepository _courseConferenceExhibitorRepository;

        public CourseConferenceExhibitorService(ICourseConferenceExhibitorRepository courseConferenceExhibitorRepository)
        {
            _courseConferenceExhibitorRepository = courseConferenceExhibitorRepository;
        }

        public async Task DeleteRange(IEnumerable<CourseConferenceExhibitor> entities)
            => await _courseConferenceExhibitorRepository.DeleteRange(entities);

        public async Task<IEnumerable<CourseConferenceExhibitor>> GetAllByCourseConferenceIdAsync(Guid courseCoferenceId)
            => await _courseConferenceExhibitorRepository.GetAllByCourseConferenceIdAsync(courseCoferenceId);
    }
}
