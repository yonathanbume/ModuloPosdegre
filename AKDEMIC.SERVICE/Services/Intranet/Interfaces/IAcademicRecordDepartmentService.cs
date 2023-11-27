using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IAcademicRecordDepartmentService
    {
        Task DeleteRange(List<AcademicRecordDepartment> entities);
        Task<List<AcademicRecordDepartment>> GetByUserId(string userId);
        Task InsertRange(List<AcademicRecordDepartment> entities);
        Task<object> GetFacultiesSelect2ClientSide(ClaimsPrincipal user);
        Task<object> GetCareersSelect2ClientSide(ClaimsPrincipal user, Guid? facultyId, bool? hasAll = null);
    }
}
