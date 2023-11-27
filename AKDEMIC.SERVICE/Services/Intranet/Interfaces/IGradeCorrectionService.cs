using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IGradeCorrectionService
    {
        Task<IEnumerable<GradeCorrection>> GetAll(string teacherId = null, Guid? termId = null);
        Task<GradeCorrection> Get(Guid id);
        Task Insert(GradeCorrection gradeCorrection);
        Task Update(GradeCorrection gradeCorrection);
        Task<bool> AnyGradeCorrectionByFilters(Guid gradeId, int status);
        Task DeleteById(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetGradeCorrectionsRequestedByStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? studentId, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters,string teacherId = null , Guid? termId = null , string searchValue = null, int? state = null);
        Task<GradeCorrection> GetByTeacherStudent(string techearId, Guid studentId);
        Task<DataTablesStructs.ReturnedData<object>> GetAllByRoleDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, string searchValue = null, int? state = null, ClaimsPrincipal user = null);
    }
}
