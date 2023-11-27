using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class ApplicationTermSurveyService : IApplicationTermSurveyService
    {
        private readonly IApplicationTermSurveyRepository _applicationTermSurveyRepository;

        public ApplicationTermSurveyService(IApplicationTermSurveyRepository applicationTermSurveyRepository)
        {
            _applicationTermSurveyRepository = applicationTermSurveyRepository;
        }

        public async Task<bool> AnyByApplicationTermId(Guid applicationTermId)
            => await _applicationTermSurveyRepository.AnyByApplicationTermId(applicationTermId);

        public async Task<bool> AnySurveyUser(Guid surveyId)
            => await _applicationTermSurveyRepository.AnySurveyUser(surveyId);

        public async Task<ApplicationTermSurvey> GetByApplicationTermId(Guid applicationTermId)
            => await _applicationTermSurveyRepository.GetByApplicationTermId(applicationTermId);

        public async Task Insert(ApplicationTermSurvey entity)
            => await _applicationTermSurveyRepository.Insert(entity);

        public async Task Update(ApplicationTermSurvey entity)
            => await _applicationTermSurveyRepository.Update(entity);
    }
}
