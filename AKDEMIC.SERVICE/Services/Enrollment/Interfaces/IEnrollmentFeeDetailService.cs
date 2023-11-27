using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IEnrollmentFeeDetailService
    {
        Task<List<EnrollmentFeeDetail>>  GetAllByEnrollmentFeeAndTerm(Guid enrollmentFeeId, Guid termId);
        Task<List<EnrollmentFeeDetail>> GetAllByEnrollmentFeeTermId(Guid enrollmentFeeTermId);
        Task DeleteAllByEnrollmentFeeId(Guid enrollmentFeeId);
        Task InsertRange(List<EnrollmentFeeDetail> details);
        Task DeleteRange(List<EnrollmentFeeDetail> details);
        Task<Tuple<bool, string>> GeneratePayments(Guid enrollmentFeeDetailId);
        Task<Tuple<bool, string>> GenerateFeePayments(bool allPayments = false);
    }
}
