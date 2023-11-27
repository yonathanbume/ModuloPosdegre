using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IAcademicProgramCurriculumRepository : IRepository<AcademicProgramCurriculum>
    {
        Task<AcademicProgramCurriculum> GetByFilter(Guid academicProgramId, Guid curriculumId);
        Task<bool> AnyByAcademicProgramAndCurriculum(Guid academicProgramId, Guid curriculumId);
        Task<IEnumerable<Select2Structs.Result>> GetByAcademicProgramIdSelect2ClientSide(Guid careerId, Guid academicProgramId);
        Task LoadAcademicProgramsJob();
    }
}