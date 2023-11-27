using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IApplicationTermSurveyService
    {
        Task Insert(ApplicationTermSurvey entity);
        Task Update(ApplicationTermSurvey entity);
        Task<ApplicationTermSurvey> GetByApplicationTermId(Guid applicationTermId);
        Task<bool> AnyByApplicationTermId(Guid applicationTermId);
        Task<bool> AnySurveyUser(Guid surveyId);
    }
}
