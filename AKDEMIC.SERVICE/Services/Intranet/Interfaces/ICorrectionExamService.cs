using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface ICorrectionExamService
    {
        Task Insert(CorrectionExam entity);
        Task Update(CorrectionExam entity);
        Task Delete(CorrectionExam entity);
        Task<CorrectionExam> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? careerId, Guid? curriculumId, int? academicYear, string search, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, string teacherId, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentToCorrectionExam(DataTablesStructs.SentParameters parameters, Guid correctionExamId, string search);
        Task<bool> AnyBySection(Guid sectionId);
    }
}
