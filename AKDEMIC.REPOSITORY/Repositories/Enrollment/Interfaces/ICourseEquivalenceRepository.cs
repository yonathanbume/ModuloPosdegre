using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseEquivalence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ICourseEquivalenceRepository : IRepository<CourseEquivalence>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetEquivalenceDatatable(DataTablesStructs.SentParameters sentParameters, Guid curriculumId, Guid? programId = null);
        Task<List<CourseTemplate>> GetEquivalenceData(Guid curriculumId, Guid? programId);
        Task<List<CourseEquivalence>> GetByCurriculumId(Guid curriculumId);
        Task<Guid> GetNewCurriculumId(Guid id);
    }
}
