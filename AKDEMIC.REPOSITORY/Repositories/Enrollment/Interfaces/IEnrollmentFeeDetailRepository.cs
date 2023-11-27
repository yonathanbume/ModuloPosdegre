using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IEnrollmentFeeDetailRepository : IRepository<EnrollmentFeeDetail>
    {
        Task<List<EnrollmentFeeDetail>> GetAllByEnrollmentFeeAndTerm(Guid enrollmentFeeId, Guid termId);
        Task<List<EnrollmentFeeDetail>> GetAllByEnrollmentFeeTermId(Guid enrollmentFeeTermId);
        Task DeleteAllByEnrollmentFeeId(Guid enrollmentFeeId);
        Task<Tuple<bool, string>> GeneratePayments(Guid enrollmentFeeDetailId);
        Task<Tuple<bool, string>> GenerateFeePayments(bool allPayments = false);
        Task<object> GetDataDatatable(Guid enrollmentFeeId);
    }
}
