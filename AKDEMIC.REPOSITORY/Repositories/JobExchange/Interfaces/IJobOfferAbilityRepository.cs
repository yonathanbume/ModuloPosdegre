using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IJobOfferAbilityRepository:IRepository<JobOfferAbility>
    {
        Task<IEnumerable<JobOfferAbility>> GetAllByJobOfferId(Guid jobOfferId);
        Task<bool> ExistByAbility(Guid AbilityId);
    }
}
