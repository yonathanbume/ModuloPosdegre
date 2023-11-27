using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IApplicationTermSurveyRepository : IRepository<ApplicationTermSurvey>
    {
        Task<ApplicationTermSurvey> GetByApplicationTermId(Guid applicationTermId);
        Task<bool> AnyByApplicationTermId(Guid applicationTermId);
        Task<bool> AnySurveyUser(Guid surveyId);
    }
}
