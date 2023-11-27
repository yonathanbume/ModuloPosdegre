using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IApplicationTermSurveyQuestionRepository : IRepository<ApplicationTermSurveyQuestion>
    {
        Task<List<ApplicationTermSurveyQuestion>> GetQuestionsBySurveyId(Guid applicationTermSurveyId);
        Task DeleteAnswers(Guid questionId);
        Task<List<ApplicationTermSurveyAnswer>> GetAnswersByQuestionId(Guid questionId);
    }
}
