using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class AcademicRecordDepartmentService : IAcademicRecordDepartmentService
    {
        private readonly IAcademicRecordDepartmentRepository _academicRecordDepartmentRepository;

        public AcademicRecordDepartmentService(IAcademicRecordDepartmentRepository academicRecordDepartmentRepository)
        {
            _academicRecordDepartmentRepository = academicRecordDepartmentRepository;
        }

        public async Task DeleteRange(List<AcademicRecordDepartment> entities)
            => await _academicRecordDepartmentRepository.DeleteRange(entities);

        public async Task<List<AcademicRecordDepartment>> GetByUserId(string userId)
            => await _academicRecordDepartmentRepository.GetByUserId(userId);

        public async Task<object> GetCareersSelect2ClientSide(ClaimsPrincipal user, Guid? facultyId, bool? hasAll = null)
            => await _academicRecordDepartmentRepository.GetCareersSelect2ClientSide(user, facultyId, hasAll);

        public async Task<object> GetFacultiesSelect2ClientSide(ClaimsPrincipal user)
            => await _academicRecordDepartmentRepository.GetFacultiesSelect2ClientSide(user);

        public async Task InsertRange(List<AcademicRecordDepartment> entities)
            => await _academicRecordDepartmentRepository.InsertRange(entities);
    }
}
