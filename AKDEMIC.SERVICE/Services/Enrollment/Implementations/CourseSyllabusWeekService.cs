using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CourseSyllabusWeekService : ICourseSyllabusWeekService
    {
        private readonly ICourseSyllabusWeekRepository _courseSyllabusWeekRepository;

        public CourseSyllabusWeekService(ICourseSyllabusWeekRepository courseSyllabusWeekRepository)
        {
            _courseSyllabusWeekRepository = courseSyllabusWeekRepository;
        }

        public async Task<IEnumerable<CourseSyllabusWeek>> GetAllByCourseSyllabusId(Guid courseSyllabusId)
            => await _courseSyllabusWeekRepository.GetAllByCourseSyllabusId(courseSyllabusId);

        public async Task Insert(CourseSyllabusWeek entity)
            => await _courseSyllabusWeekRepository.Insert(entity);

        public async Task InsertRange(IEnumerable<CourseSyllabusWeek> entities)
            => await _courseSyllabusWeekRepository.InsertRange(entities);

        public async Task Update(CourseSyllabusWeek entity)
            => await _courseSyllabusWeekRepository.Update(entity);

        public async Task UpdateRange(IEnumerable<CourseSyllabusWeek> entities)
            => await _courseSyllabusWeekRepository.UpdateRange(entities);
    }
}
