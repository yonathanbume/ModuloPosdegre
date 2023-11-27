using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface ICorrectionExamRepository : IRepository<CorrectionExam>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? careerId, Guid? curriculumId, int? academicYear, string search, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, string teacherId, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentToCorrectionExam(DataTablesStructs.SentParameters parameters, Guid correctionExamId, string search);
        Task<bool> AnyBySection(Guid sectionId);
    }
}
