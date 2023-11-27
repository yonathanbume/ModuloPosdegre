using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class EnrollmentFeeService : IEnrollmentFeeService
    {
        private IEnrollmentFeeRepository _enrollmentFeeRepository;

        public EnrollmentFeeService(IEnrollmentFeeRepository enrollmentFeeRepository)
        {
            _enrollmentFeeRepository = enrollmentFeeRepository;
        }

        public async Task DeleteById(Guid id)
            => await _enrollmentFeeRepository.DeleteById(id);

        public async Task<EnrollmentFee> Get(Guid id)
            => await _enrollmentFeeRepository.Get(id);

        public async Task<List<Select2Structs.Result>> GetAllSelect2Data()
            => await _enrollmentFeeRepository.GetAllSelect2Data();

        public async Task<EnrollmentFee> GetByFilter(Guid enrollmentFeeTermId, int feeCount)
            => await _enrollmentFeeRepository.GetByFilter(enrollmentFeeTermId, feeCount);


        public async Task Insert(EnrollmentFee fee)
            => await _enrollmentFeeRepository.Insert(fee);

        public async Task Update(EnrollmentFee fee)
            => await _enrollmentFeeRepository.Update(fee);
    }
}
