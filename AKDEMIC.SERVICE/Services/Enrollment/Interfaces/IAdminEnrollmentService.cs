using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IAdminEnrollmentService
    {
        Task<AdminEnrollment> FindById(Guid id);
        Task<object> GetDataDatatableClientSide(Guid studentId, Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? termId = null, Guid? careerId = null, Guid? facultyId = null, ClaimsPrincipal user = null);
        Task<AdminEnrollment> GetAdminEnrollmentRectification(Guid studentId, Guid termId);
        Task Insert(AdminEnrollment adminEnrollment);
        Task Update(AdminEnrollment adminEnrollment);
        Task<AdminEnrollment> GetAdminPendingEnrollmentRectification(Guid studentId, string userId, Guid termId);
        Task<AdminEnrollment> GetLastAdminEnrollmentByStudent(Guid studentId, Guid termId);
    }
}
