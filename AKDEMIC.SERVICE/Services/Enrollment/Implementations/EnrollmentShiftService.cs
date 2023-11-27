using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class EnrollmentShiftService : IEnrollmentShiftService
    {
        private readonly IEnrollmentShiftRepository _enrollmentShiftRepository;

        public EnrollmentShiftService(IEnrollmentShiftRepository enrollmentShiftRepository)
        {
            _enrollmentShiftRepository = enrollmentShiftRepository;
        }

        public async Task<bool> AnyByTerm(Guid termId) => await _enrollmentShiftRepository.AnyByTerm(termId);

        public async Task<EnrollmentShift> Get(Guid id) => await _enrollmentShiftRepository.Get(id);

        public async Task<EnrollmentShift> GetActiveTermEnrollmentShift() => await _enrollmentShiftRepository.GetActiveTermEnrollmentShift();

        public async Task<EnrollmentShift> GetByTerm(Guid termId) => await _enrollmentShiftRepository.GetByTerm(termId);

        public async Task Insert(EnrollmentShift enrollmentShift) => await _enrollmentShiftRepository.Insert(enrollmentShift);

        public async Task Update(EnrollmentShift enrollmentShift) => await _enrollmentShiftRepository.Update(enrollmentShift);
    }
}
