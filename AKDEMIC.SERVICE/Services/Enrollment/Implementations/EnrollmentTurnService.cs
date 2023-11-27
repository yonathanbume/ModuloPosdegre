using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.EnrollmentTurn;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class EnrollmentTurnService : IEnrollmentTurnService
    {
        private readonly IEnrollmentTurnRepository _enrollmentTurnRepository;

        public EnrollmentTurnService(IEnrollmentTurnRepository enrollmentTurnRepository)
        {
            _enrollmentTurnRepository = enrollmentTurnRepository;
        }

        public Task<Tuple<bool, string>> GenerateTurns(Guid enrollmentShiftId)
            => _enrollmentTurnRepository.GenerateTurns(enrollmentShiftId);

        public async Task<EnrollmentTurn> Get(Guid id) => await _enrollmentTurnRepository.Get(id);
        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, int? type = null, ClaimsPrincipal user = null)
            => await _enrollmentTurnRepository.GetDataDatatable(sentParameters, termId, searchValue, facultyId, careerId, type, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatableWithCredits(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, string searchValue = null, ClaimsPrincipal user = null, bool? isConfirmed = null, bool? isReceived = null)
            => await _enrollmentTurnRepository.GetDataDatatableWithCredits(sentParameters, termId, facultyId, careerId, searchValue, user, isConfirmed, isReceived);

        public Task<object> GetStudentsDetailDatatable(Guid termId, Guid? careerId = null) => _enrollmentTurnRepository.GetStudentsDetailDatatable(termId, careerId);

        public async Task<EnrollmentTurn> GetStudentTurn(Guid studentId, Guid termId) => await _enrollmentTurnRepository.GetStudentTurn(studentId, termId);

        public async Task Update(EnrollmentTurn enrollmentTurn) => await _enrollmentTurnRepository.Update(enrollmentTurn);
        public async Task<EnrollmentTurn> GetWithData(Guid id)
            => await _enrollmentTurnRepository.GetWithData(id);
        public async Task<EnrollmentTurn> GetByStudentIdAndTerm(Guid studentId, Guid termId)
            => await _enrollmentTurnRepository.GetByStudentIdAndTerm(studentId, termId);

        public async Task EnrollmentTurnsFixJob(Guid termid)
        {
            await _enrollmentTurnRepository.EnrollmentTurnsFixJob(termid);
        }

        public async Task<Tuple<bool, string>> GenerateTurns(Guid enrollmentShiftId, Guid careerId) => await _enrollmentTurnRepository.GenerateTurns(enrollmentShiftId, careerId);

        public Task<decimal> GetStudentCreditsWithoutTurn(Guid studentId) => _enrollmentTurnRepository.GetStudentCreditsWithoutTurn(studentId);

        public async Task ValidateReceivedEnrollments() => await _enrollmentTurnRepository.ValidateReceivedEnrollments();

        public async Task<Tuple<bool, string>> GenerateStudentTurn(Guid termId, Guid studentId)
            => await _enrollmentTurnRepository.GenerateStudentTurn(termId, studentId);

        public async Task<List<EnrollmentTurnTemplate>> GetSpecialEnrollmentData(Guid termId, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null)
            => await _enrollmentTurnRepository.GetSpecialEnrollmentData(termId, searchValue, facultyId, careerId, user);

    }
}
