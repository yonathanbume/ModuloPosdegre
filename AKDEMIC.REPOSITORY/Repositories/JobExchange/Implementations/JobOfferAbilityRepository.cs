using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class JobOfferAbilityRepository:Repository<JobOfferAbility> , IJobOfferAbilityRepository
    {
        public JobOfferAbilityRepository(AkdemicContext context) : base(context){  }

        public async Task<bool> ExistByAbility(Guid AbilityId)
        {
            return await _context.JobOfferAbilities.AnyAsync(x => x.AbilityId == AbilityId);
        }

        public async Task<IEnumerable<JobOfferAbility>> GetAllByJobOfferId(Guid jobOfferId)
        {
            var query = _context.JobOfferAbilities
                .Where(x => x.JobOfferId == jobOfferId);

            return await query.ToListAsync();
        }
    }
}
