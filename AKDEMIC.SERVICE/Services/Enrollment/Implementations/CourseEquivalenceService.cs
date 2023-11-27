using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseEquivalence;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CourseEquivalenceService : ICourseEquivalenceService
    {
        private readonly ICourseEquivalenceRepository _courseEquivalenceRepository;

        public CourseEquivalenceService(ICourseEquivalenceRepository courseEquivalenceRepository)
        {
            _courseEquivalenceRepository = courseEquivalenceRepository;
        }
         
        public async Task Delete(CourseEquivalence courseEquivalence) => await _courseEquivalenceRepository.Delete(courseEquivalence);

        public async Task DeleteById(Guid id) => await _courseEquivalenceRepository.DeleteById(id);

        public async Task<CourseEquivalence> Get(Guid id) => await _courseEquivalenceRepository.Get(id);

        public async Task<List<CourseEquivalence>> GetByCurriculumId(Guid curriculumId)
        {
            return await _courseEquivalenceRepository.GetByCurriculumId(curriculumId);
        }

        public async Task<List<CourseTemplate>> GetEquivalenceData(Guid curriculumId, Guid? programId)
        {
            return await _courseEquivalenceRepository.GetEquivalenceData(curriculumId, programId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEquivalenceDatatable(DataTablesStructs.SentParameters sentParameters, Guid curriculumId, Guid? programId = null) 
            => await _courseEquivalenceRepository.GetEquivalenceDatatable(sentParameters, curriculumId, programId);

        public async  Task<Guid> GetNewCurriculumId(Guid id)
        {
            return await _courseEquivalenceRepository.GetNewCurriculumId(id);
        }

        public async Task Insert(CourseEquivalence courseEquivalence) => await _courseEquivalenceRepository.Insert(courseEquivalence);

        public async Task Update(CourseEquivalence courseEquivalence) => await _courseEquivalenceRepository.Update(courseEquivalence);
    }
}
