using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseEquivalence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICourseEquivalenceService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetEquivalenceDatatable(DataTablesStructs.SentParameters sentParameters, Guid curriculumId, Guid? programId = null);
        Task<List<CourseTemplate>> GetEquivalenceData(Guid curriculumId, Guid? programId);
        Task Delete(CourseEquivalence courseEquivalence);
        Task DeleteById(Guid id);
        Task Insert(CourseEquivalence courseEquivalence);
        Task Update(CourseEquivalence courseEquivalence);
        Task<CourseEquivalence> Get(Guid id);
        Task<List<CourseEquivalence>> GetByCurriculumId(Guid curriculumId);
        Task<Guid> GetNewCurriculumId(Guid guid);
    }
}
