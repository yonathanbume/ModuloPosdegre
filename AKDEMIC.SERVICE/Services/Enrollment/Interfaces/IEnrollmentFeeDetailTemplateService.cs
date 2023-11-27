using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IEnrollmentFeeDetailTemplateService
    {
        Task<List<EnrollmentFeeDetailTemplate>> GetByFeeCount(int feeCount);
        Task InsertRange(List<EnrollmentFeeDetailTemplate> details);
        Task UpdateRange(List<EnrollmentFeeDetailTemplate> details);
    }
}
