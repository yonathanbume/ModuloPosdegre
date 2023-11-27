using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IEnrollmentFeeTemplateRepository : IRepository<EnrollmentFeeTemplate>
    {
        Task<EnrollmentFeeTemplate> GetByFeeCount(int feeCount);
    }
}
