using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class StudentAbsenceJustificationService : IStudentAbsenceJustificationService
    {
        private readonly IStudentAbsenceJustificationRepository _studentAbsenceJustificationRepository;

        public StudentAbsenceJustificationService(IStudentAbsenceJustificationRepository studentAbsenceJustificationRepository)
        {
            _studentAbsenceJustificationRepository = studentAbsenceJustificationRepository;
        }

        public async Task<bool> Any(Guid classStudentId, int? status = null)
            => await _studentAbsenceJustificationRepository.Any(classStudentId, status);

        public async Task Delete(StudentAbsenceJustification studentAbsenceJustification)
            => await _studentAbsenceJustificationRepository.Delete(studentAbsenceJustification);

        public async Task DeleteById(Guid id)
            => await _studentAbsenceJustificationRepository.DeleteById(id);

        public async Task<StudentAbsenceJustification> Get(Guid id)
            => await _studentAbsenceJustificationRepository.Get(id);

        public async Task<object> GetAdminDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string searchValue = null, Guid? termId = null, byte? status = null)
            => await _studentAbsenceJustificationRepository.GetAdminDatatable(sentParameters, user, searchValue, termId, status);

        public async Task<IEnumerable<StudentAbsenceJustification>> GetAll(Guid? termId = null, Guid? studentId = null)
            => await _studentAbsenceJustificationRepository.GetAll(termId, studentId);

        public async Task<IEnumerable<StudentAbsenceJustification>> GetAll(Guid? termId = null, string userId = null)
            => await _studentAbsenceJustificationRepository.GetAll(termId, userId);

        public async Task<IEnumerable<StudentAbsenceJustification>> GetAll(Guid? termId = null)
            => await _studentAbsenceJustificationRepository.GetAll(termId);

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string searchValue = null, byte? status = null)
            => await _studentAbsenceJustificationRepository.GetDatatable(sentParameters, user, searchValue, status);

        public async Task Insert(StudentAbsenceJustification studentAbsenceJustification)
            => await _studentAbsenceJustificationRepository.Insert(studentAbsenceJustification);

        public async Task Update(StudentAbsenceJustification studentAbsenceJustification)
            => await _studentAbsenceJustificationRepository.Update(studentAbsenceJustification);
    }
}
