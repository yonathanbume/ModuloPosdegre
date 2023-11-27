using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class JobOfferAbilityService: IJobOfferAbilityService
    {
        private readonly IJobOfferAbilityRepository _jobOfferAbilityRepository;

        public JobOfferAbilityService(IJobOfferAbilityRepository jobOfferAbilityRepository)
        {
            _jobOfferAbilityRepository = jobOfferAbilityRepository;
        }

        public async Task DeleteRange(IEnumerable<JobOfferAbility> jobOfferAbilities)
        {
            await _jobOfferAbilityRepository.DeleteRange(jobOfferAbilities);
        }

        public async Task<bool> ExistByAbility(Guid AbilityId)
        {
            return await _jobOfferAbilityRepository.ExistByAbility(AbilityId);
        }

        public async Task<IEnumerable<JobOfferAbility>> GetAllByJobOfferId(Guid jobOfferId)
        {
            return await _jobOfferAbilityRepository.GetAllByJobOfferId(jobOfferId);
        }
    }
}
