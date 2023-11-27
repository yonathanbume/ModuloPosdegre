using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class EnrollmentFeeTemplateService : IEnrollmentFeeTemplateService
    {
        private IEnrollmentFeeTemplateRepository _enrollmentFeeTemplateRepository;

        public EnrollmentFeeTemplateService(IEnrollmentFeeTemplateRepository enrollmentFeeTemplateRepository)
        {
            _enrollmentFeeTemplateRepository = enrollmentFeeTemplateRepository;
        }

        public async Task DeleteById(Guid id)
            => await _enrollmentFeeTemplateRepository.DeleteById(id);

        public async Task<EnrollmentFeeTemplate> Get(Guid id)
            => await _enrollmentFeeTemplateRepository.Get(id);

        public async Task<EnrollmentFeeTemplate> GetByFeeCount(int feeCount)
            => await _enrollmentFeeTemplateRepository.GetByFeeCount(feeCount);

        public async Task Insert(EnrollmentFeeTemplate fee)
            => await _enrollmentFeeTemplateRepository.Insert(fee);

        public async Task Update(EnrollmentFeeTemplate fee)
            => await _enrollmentFeeTemplateRepository.Update(fee);
    }
}
