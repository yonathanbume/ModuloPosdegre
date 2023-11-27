using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class SurveyItemService : ISurveyItemService
    {
        private readonly ISurveyItemRepository _surveyItemRepository;

        public SurveyItemService(ISurveyItemRepository surveyItemRepository)
        {
            _surveyItemRepository = surveyItemRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _surveyItemRepository.DeleteById(id);
        }

        public async Task<SurveyItem> Get(Guid id)
        {
            return await _surveyItemRepository.Get(id);
        }

        public async Task<IEnumerable<SurveyItem>> GetBySurvey(Guid surveyId)
        {
            return await _surveyItemRepository.GetBySurvey(surveyId);
        }

        public async Task Insert(SurveyItem surveyItem)
        {
            await _surveyItemRepository.Insert(surveyItem);
        }

        public async Task Update(SurveyItem item)
        {
            await _surveyItemRepository.Update(item);
        }

        public Task<List<SurveyItemReportTemplate>> GetSurveyItemTemplate(Guid surveyId, bool? graduated = null)
            => _surveyItemRepository.GetSurveyItemTemplate(surveyId, graduated);

        public Task<bool> HasQuestions(Guid id)
            => _surveyItemRepository.HasQuestions(id);

        public async Task<IEnumerable<SurveyItem>> GetAllBySurvey(Guid surveyId)
            => await _surveyItemRepository.GetAllBySurvey(surveyId);

        public async Task<IEnumerable<SurveyItem>> GetAllBySurveryAndQuestionType(Guid surveyId, int questionType)
            => await _surveyItemRepository.GetAllBySurveryAndQuestionType(surveyId, questionType);

        public Task<List<SurveyItem>> GetSurveyItemsToImport(Guid surveyId)
            => _surveyItemRepository.GetSurveyItemsToImport(surveyId);

        public Task<SurveyItem> Add(SurveyItem surveyItem)
            => _surveyItemRepository.Add(surveyItem);

        public Task SaveChanges()
            => _surveyItemRepository.SaveChanges();
    }
}
