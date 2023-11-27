using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class AverageGradeCreditConfigurationRepository : Repository<AverageGradeCreditConfiguration>, IAverageGradeCreditConfigurationRepository
    {
        public AverageGradeCreditConfigurationRepository(AkdemicContext context) : base(context)
        {
        }
    }
}
