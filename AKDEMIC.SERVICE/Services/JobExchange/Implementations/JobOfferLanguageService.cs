using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class JobOfferLanguageService: IJobOfferLanguageService
    {
        private readonly IJobOfferLanguageRepository _jobOfferLanguageRepository;
        public JobOfferLanguageService(IJobOfferLanguageRepository jobOfferLanguageRepository)
        {
            _jobOfferLanguageRepository = jobOfferLanguageRepository;
        }

        public async Task DeleteRange(IEnumerable<JobOfferLanguage> jobOfferLanguages)
        {
            await _jobOfferLanguageRepository.DeleteRange(jobOfferLanguages);
        }

        public async Task<bool> ExistByLanguage(Guid id)
        {
            return await _jobOfferLanguageRepository.ExistByLanguage(id);
        }

        public async Task<IEnumerable<JobOfferLanguage>> GetAllByJobOfferId(Guid jobOfferId)
        {
            return await _jobOfferLanguageRepository.GetAllByJobOfferId(jobOfferId);
        }
    }
}
