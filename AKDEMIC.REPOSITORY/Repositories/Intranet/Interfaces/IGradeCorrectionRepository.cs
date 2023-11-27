using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IGradeCorrectionRepository : IRepository<GradeCorrection>
    {
        Task<IEnumerable<GradeCorrection>> GetAll(string teacherId = null, Guid? termId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string teacherId = null, Guid? termId = null, string searchValue = null, int? state = null);
        Task<GradeCorrection> GetByTeacherStudent(string teacherId, Guid studentId);
        Task<bool> AnyGradeCorrectionByFilters(Guid gradeId, int status);
        Task<DataTablesStructs.ReturnedData<object>> GetGradeCorrectionsRequestedByStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? studentId, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetAllByRoleDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, string searchValue = null, int? state = null, ClaimsPrincipal user = null);
    }
}
