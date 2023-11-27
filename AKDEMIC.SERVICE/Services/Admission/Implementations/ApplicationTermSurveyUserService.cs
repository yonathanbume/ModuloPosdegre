using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class ApplicationTermSurveyUserService : IApplicationTermSurveyUserService
    {
        private readonly IApplicationTermSurveyUserRepository _applicationTermSurveyUserRepository;

        public ApplicationTermSurveyUserService(IApplicationTermSurveyUserRepository applicationTermSurveyUserRepository)
        {
            _applicationTermSurveyUserRepository = applicationTermSurveyUserRepository;
        }

        public async Task Insert(ApplicationTermSurveyUser entity)
            => await _applicationTermSurveyUserRepository.Insert(entity);
    }
}
