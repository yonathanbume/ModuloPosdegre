using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IStudentAbsenceJustificationService
    {
        Task<IEnumerable<StudentAbsenceJustification>> GetAll(Guid? termId = null, Guid? studentId = null);
        Task<IEnumerable<StudentAbsenceJustification>> GetAll(Guid? termId = null, string userId = null);
        Task<IEnumerable<StudentAbsenceJustification>> GetAll(Guid? termId = null);
        Task<StudentAbsenceJustification> Get(Guid id);
        Task Insert(StudentAbsenceJustification studentAbsenceJustification);
        Task Update(StudentAbsenceJustification studentAbsenceJustification);
        Task DeleteById(Guid id);
        Task Delete(StudentAbsenceJustification studentAbsenceJustification);
        Task<bool> Any(Guid classStudentId, int? status = null);
        Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string searchValue = null, byte? status = null);
        Task<object> GetAdminDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string searchValue = null, Guid? termId = null, byte? status = null);
    }
}
