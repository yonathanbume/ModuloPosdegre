using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IStudentAbsenceJustificationRepository : IRepository<StudentAbsenceJustification>
    {
        Task<IEnumerable<StudentAbsenceJustification>> GetAll(Guid? termId = null);
        Task<IEnumerable<StudentAbsenceJustification>> GetAll(Guid? termId = null, Guid? studentId = null);
        Task<IEnumerable<StudentAbsenceJustification>> GetAll(Guid? termId = null, string userId = null);
        Task<bool> Any(Guid classStudentId, int? status = null);
        Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string searchValue = null, byte? status = null);
        Task<object> GetAdminDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string searchValue = null, Guid? termId = null, byte? status = null);
    }
}
