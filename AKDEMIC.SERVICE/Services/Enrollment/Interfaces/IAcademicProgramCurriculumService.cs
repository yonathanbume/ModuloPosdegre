using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IAcademicProgramCurriculumService
    {
        Task<AcademicProgramCurriculum> GetAsync(Guid id);
        Task InsertAsync(AcademicProgramCurriculum academicProgramCurriculum);
        Task UpdateAsync(AcademicProgramCurriculum academicProgramCurriculum);
        Task DeleteAsync(AcademicProgramCurriculum academicProgramCurriculum);
        Task<AcademicProgramCurriculum> GetByFilter(Guid academicProgramId, Guid curriculumId);
        Task<bool> AnyByAcademicProgramAndCurriculum(Guid academicProgramId, Guid curriculumId);
        Task<IEnumerable<Select2Structs.Result>> GetByAcademicProgramIdSelect2ClientSide(Guid careerId, Guid academicProgramId);
        Task LoadAcademicProgramsJob();
    }
}