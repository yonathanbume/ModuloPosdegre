using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class EnrollmentFeeDetailService : IEnrollmentFeeDetailService
    {
        private IEnrollmentFeeDetailRepository _enrollmentFeeDetailRepository;

        public EnrollmentFeeDetailService(IEnrollmentFeeDetailRepository enrollmentFeeDetailRepository)
        {
            _enrollmentFeeDetailRepository = enrollmentFeeDetailRepository;
        }

        public async Task DeleteAllByEnrollmentFeeId(Guid enrollmentFeeId)
            => await _enrollmentFeeDetailRepository.DeleteAllByEnrollmentFeeId(enrollmentFeeId);

        public async Task DeleteRange(List<EnrollmentFeeDetail> details)
            => await _enrollmentFeeDetailRepository.DeleteRange(details);

        public async Task<Tuple<bool, string>> GenerateFeePayments(bool allPayments = false)
             => await _enrollmentFeeDetailRepository.GenerateFeePayments(allPayments);

        public async Task<Tuple<bool, string>> GeneratePayments(Guid enrollmentFeeDetailId)
             => await _enrollmentFeeDetailRepository.GeneratePayments(enrollmentFeeDetailId);

        public async Task<List<EnrollmentFeeDetail>> GetAllByEnrollmentFeeAndTerm(Guid enrollmentFeeId, Guid termId)
            => await _enrollmentFeeDetailRepository.GetAllByEnrollmentFeeAndTerm(enrollmentFeeId, termId);

        public async Task<List<EnrollmentFeeDetail>> GetAllByEnrollmentFeeTermId(Guid enrollmentFeeTermId)
            => await _enrollmentFeeDetailRepository.GetAllByEnrollmentFeeTermId(enrollmentFeeTermId);

        public async Task InsertRange(List<EnrollmentFeeDetail> details)
            => await _enrollmentFeeDetailRepository.InsertRange(details);
    }
}
