using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IDeferredExamService
    {
        Task Insert(DeferredExam entity);
        Task Update(DeferredExam entity);
        Task<DeferredExam> Get(Guid id);
        Task Delete(DeferredExam entity);
        Task<DataTablesStructs.ReturnedData<object>> GetDeferredExamDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? careerId, Guid? curriculumId, int? academicYear, string search, ClaimsPrincipal user = null);
        Task<bool> AnyBySection(Guid sectionId);
        Task<List<StudentSection>> GetStudentSectionsAvailableToDeferredExam(Guid sectionId);
    }
}
