using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class EnrollmentFeeDetailTemplateService : IEnrollmentFeeDetailTemplateService
    {
        private readonly IEnrollmentFeeDetailTemplateRepository _enrollmentFeeDetailTemplateRepository;
        public EnrollmentFeeDetailTemplateService(IEnrollmentFeeDetailTemplateRepository enrollmentFeeDetailTemplateRepository)
        {
            _enrollmentFeeDetailTemplateRepository = enrollmentFeeDetailTemplateRepository;
        }

        public async Task<List<EnrollmentFeeDetailTemplate>> GetByFeeCount(int feeCount)
            => await _enrollmentFeeDetailTemplateRepository.GetByFeeCount(feeCount);

        public async Task InsertRange(List<EnrollmentFeeDetailTemplate> details)
            => await _enrollmentFeeDetailTemplateRepository.InsertRange(details);

        public async Task UpdateRange(List<EnrollmentFeeDetailTemplate> details)
            => await _enrollmentFeeDetailTemplateRepository.UpdateRange(details);
    }
}
