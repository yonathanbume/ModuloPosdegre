using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.EnrollmentReservation;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class EnrollmentReservationService : IEnrollmentReservationService
    {
        private readonly IEnrollmentReservationRepository _enrollmentReservationRepository;

        public EnrollmentReservationService(IEnrollmentReservationRepository enrollmentReservationRepository)
        {
            _enrollmentReservationRepository = enrollmentReservationRepository;
        }

        public async Task<EnrollmentReservation> Get(Guid id) =>
            await _enrollmentReservationRepository.Get(id);

        public async Task<List<EnrollmentReservation>> GetEnrollmentReservations()
        {
            return await _enrollmentReservationRepository.GetEnrollmentReservations();
        }

        public async Task Delete(EnrollmentReservation enrollmentReservation) =>
            await _enrollmentReservationRepository.Delete(enrollmentReservation);

        public async Task Insert(EnrollmentReservation enrollmentReservation) =>
            await _enrollmentReservationRepository.Insert(enrollmentReservation);

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrollmentReservationsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null)
            => await _enrollmentReservationRepository.GetEnrollmentReservationsDatatable(sentParameters, searchValue, termId, facultyId, careerId, user);

        public Task<IEnumerable<EnrollmentReservation>> GetEnrollmentReservationsByStudent(Guid studentId, Guid? termId = null)
            => _enrollmentReservationRepository.GetEnrollmentReservationsByStudent(studentId, termId);

        public Task<bool> ValidateStudentReservationExist(Guid termId, Guid studentId)
            => _enrollmentReservationRepository.ValidateStudentReservationExist(termId, studentId);

        public async Task<IEnumerable<ReservationExcelTemplate>> GetEnrollmentReservationsExcel(Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null)
            => await _enrollmentReservationRepository.GetEnrollmentReservationsExcel(termId, facultyId, careerId, user);

        public async Task Update(EnrollmentReservation entity)
            => await _enrollmentReservationRepository.Update(entity);
    }
}
