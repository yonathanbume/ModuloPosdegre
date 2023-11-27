using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IEnrollmentFeeTemplateService
    {
        Task<EnrollmentFeeTemplate> Get(Guid id);
        Task Insert(EnrollmentFeeTemplate fee);
        Task Update(EnrollmentFeeTemplate fee);
        Task DeleteById(Guid id);
        Task<EnrollmentFeeTemplate> GetByFeeCount(int feeCount);
    }
}
