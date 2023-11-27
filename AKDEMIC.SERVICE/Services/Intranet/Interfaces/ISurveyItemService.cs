using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface ISurveyItemService
    {
        Task Insert(SurveyItem surveyItem);
        Task<SurveyItem> Add(SurveyItem surveyItem);
        Task<List<SurveyItem>> GetSurveyItemsToImport(Guid surveyId);
        Task<List<SurveyItemReportTemplate>> GetSurveyItemTemplate(Guid surveyId, bool? graduated = null);
        Task DeleteById(Guid id);
        Task<SurveyItem> Get(Guid id);
        Task<IEnumerable<SurveyItem>> GetBySurvey(Guid surveyId);
        Task Update(SurveyItem item);
        Task SaveChanges();
        Task<bool> HasQuestions(Guid id);
        Task<IEnumerable<SurveyItem>> GetAllBySurvey(Guid surveyId);
        Task<IEnumerable<SurveyItem>> GetAllBySurveryAndQuestionType(Guid surveyId, int questionType);
    }
}
