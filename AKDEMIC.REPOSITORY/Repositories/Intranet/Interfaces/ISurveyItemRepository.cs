using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface ISurveyItemRepository : IRepository<SurveyItem>
    {
        Task<IEnumerable<SurveyItem>> GetBySurvey(Guid surveyId);
        Task<List<SurveyItem>> GetSurveyItemsToImport(Guid surveyId);
        Task<List<SurveyItemReportTemplate>> GetSurveyItemTemplate(Guid surveyId, bool? graduated = null);
        Task<bool> HasQuestions(Guid id);
        Task SaveChanges();
        Task<IEnumerable<SurveyItem>> GetAllBySurvey(Guid surveyId);
        Task<IEnumerable<SurveyItem>> GetAllBySurveryAndQuestionType(Guid surveyId, int questionType);
    }
}
