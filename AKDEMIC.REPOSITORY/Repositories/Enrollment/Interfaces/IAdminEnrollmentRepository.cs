using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IAdminEnrollmentRepository : IRepository<AdminEnrollment>
    {
        Task<AdminEnrollment> FindById(Guid id);
        Task<object> GetDataDatatableClientSide(Guid studentId, Guid termId);
        Task<AdminEnrollment> GetLastAdminEnrollmentByStudent(Guid studentId, Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? termId = null, Guid? careerId = null, Guid? facultyId = null, ClaimsPrincipal user = null);
        Task<AdminEnrollment> GetAdminEnrollmentRectification(Guid studentId, Guid termId);
        Task<AdminEnrollment> GetAdminPendingEnrollmentRectification(Guid studentId, string userId, Guid termId);
    }
}
