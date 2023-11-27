using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class AdminEnrollmentService : IAdminEnrollmentService
    {
        private readonly IAdminEnrollmentRepository _adminEnrollmentRepository;
        public AdminEnrollmentService(IAdminEnrollmentRepository adminEnrollmentRepository)
        {
            _adminEnrollmentRepository = adminEnrollmentRepository;
        }

        public async Task<AdminEnrollment> FindById(Guid id) =>
            await _adminEnrollmentRepository.FindById(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? termId = null, Guid? careerId = null, Guid? facultyId = null, ClaimsPrincipal user = null)
            => await _adminEnrollmentRepository.GetDataDatatable(sentParameters, searchValue, termId, careerId, facultyId, user);

        public async Task<object> GetDataDatatableClientSide(Guid studentId, Guid termId)
            => await _adminEnrollmentRepository.GetDataDatatableClientSide(studentId, termId);
        public async Task<AdminEnrollment> GetAdminEnrollmentRectification(Guid studentId, Guid termId)
            => await _adminEnrollmentRepository.GetAdminEnrollmentRectification(studentId, termId);
        public async Task Insert(AdminEnrollment adminEnrollment)
            => await _adminEnrollmentRepository.Insert(adminEnrollment);
        public async Task Update(AdminEnrollment adminEnrollment)
            => await _adminEnrollmentRepository.Update(adminEnrollment);

        public async Task<AdminEnrollment> GetAdminPendingEnrollmentRectification(Guid studentId, string userId, Guid termId)
            => await _adminEnrollmentRepository.GetAdminPendingEnrollmentRectification(studentId, userId, termId);

        public async Task<AdminEnrollment> GetLastAdminEnrollmentByStudent(Guid studentId, Guid termId)
            => await _adminEnrollmentRepository.GetLastAdminEnrollmentByStudent(studentId, termId);
    }
}
