using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class ApplicationTermSurveyQuestionService : IApplicationTermSurveyQuestionService
    {
        private readonly IApplicationTermSurveyQuestionRepository _applicationTermSurveyQuestionRepository;

        public ApplicationTermSurveyQuestionService(IApplicationTermSurveyQuestionRepository applicationTermSurveyQuestionRepository)
        {
            _applicationTermSurveyQuestionRepository = applicationTermSurveyQuestionRepository;
        }

        public async Task Delete(ApplicationTermSurveyQuestion entity)
            => await _applicationTermSurveyQuestionRepository.Delete(entity);

        public async Task DeleteAnswers(Guid questionId)
            => await _applicationTermSurveyQuestionRepository.DeleteAnswers(questionId);

        public async Task<ApplicationTermSurveyQuestion> Get(Guid id)
            => await _applicationTermSurveyQuestionRepository.Get(id);

        public async Task<List<ApplicationTermSurveyAnswer>> GetAnswersByQuestionId(Guid questionId)
            => await _applicationTermSurveyQuestionRepository.GetAnswersByQuestionId(questionId);

        public async Task<List<ApplicationTermSurveyQuestion>> GetQuestionsBySurveyId(Guid applicationTermSurveyId)
            => await _applicationTermSurveyQuestionRepository.GetQuestionsBySurveyId(applicationTermSurveyId);

        public async Task Insert(ApplicationTermSurveyQuestion entity)
            => await _applicationTermSurveyQuestionRepository.Insert(entity);

        public async Task Update(ApplicationTermSurveyQuestion entity)
            => await _applicationTermSurveyQuestionRepository.Update(entity);
    }
}
