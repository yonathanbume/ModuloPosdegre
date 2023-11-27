using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IEnrollmentFeeService
    {
        Task<List<Select2Structs.Result>> GetAllSelect2Data();
        Task<EnrollmentFee> Get(Guid id);
        Task Insert(EnrollmentFee fee);
        Task Update(EnrollmentFee fee);
        Task DeleteById(Guid id);
        Task<EnrollmentFee> GetByFilter(Guid enrollmentFeeTermId, int feeCount);
    }
}
