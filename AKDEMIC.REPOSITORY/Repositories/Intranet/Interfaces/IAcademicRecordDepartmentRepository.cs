using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IAcademicRecordDepartmentRepository : IRepository<AcademicRecordDepartment>
    {
        Task<List<AcademicRecordDepartment>> GetByUserId(string userId);
        Task<object> GetFacultiesSelect2ClientSide(ClaimsPrincipal user);
        Task<object> GetCareersSelect2ClientSide(ClaimsPrincipal user, Guid? facultyId, bool? hasAll = null);
    }
}
