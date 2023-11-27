using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IEnrollmentFeeRepository : IRepository<EnrollmentFee>
    {
        Task<List<Select2Structs.Result>> GetAllSelect2Data();
        //Task<List<EnrollmentFee>> GetAllByTerm(Guid termId);
        Task<EnrollmentFee> GetByFilter(Guid enrollmentFeeTermId, int feeCount);
    }
}
