using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IJobOfferAbilityService
    {
        Task DeleteRange(IEnumerable<JobOfferAbility> jobOfferAbilities);
        Task<IEnumerable<JobOfferAbility>> GetAllByJobOfferId(Guid jobOfferId);
        Task<bool> ExistByAbility(Guid AbilityId);
    }
}
