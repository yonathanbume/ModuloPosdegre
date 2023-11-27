using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IApplicationTermSurveyQuestionService
    {
        Task<List<ApplicationTermSurveyQuestion>> GetQuestionsBySurveyId(Guid applicationTermSurveyId);
        Task Insert(ApplicationTermSurveyQuestion entity);
        Task DeleteAnswers(Guid questionId);
        Task Delete(ApplicationTermSurveyQuestion entity);
        Task<ApplicationTermSurveyQuestion> Get(Guid id);
        Task Update(ApplicationTermSurveyQuestion entity);
        Task<List<ApplicationTermSurveyAnswer>> GetAnswersByQuestionId(Guid questionId);
    }
}
