using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class ApplicationTermSurveyUserRepository : Repository<ApplicationTermSurveyUser> , IApplicationTermSurveyUserRepository
    {
        public ApplicationTermSurveyUserRepository(AkdemicContext context) : base(context) { }

    }
}
