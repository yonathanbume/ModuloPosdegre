using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IDeferredExamRepository : IRepository<DeferredExam>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDeferredExamDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? careerId, Guid? curriculumId, int? academicYear, string search, ClaimsPrincipal user = null);
        Task<bool> AnyBySection(Guid sectionId);
        Task<List<StudentSection>> GetStudentSectionsAvailableToDeferredExam(Guid sectionId);
    }
}
