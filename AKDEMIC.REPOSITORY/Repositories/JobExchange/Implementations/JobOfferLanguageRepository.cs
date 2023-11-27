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
    public class JobOfferLanguageRepository : Repository<JobOfferLanguage>, IJobOfferLanguageRepository
    {
        public JobOfferLanguageRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> ExistByLanguage(Guid id)
        {
            return await _context.JobOfferLanguages.AnyAsync(x=>x.LanguageId == id);
        }

        public async Task<IEnumerable<JobOfferLanguage>> GetAllByJobOfferId(Guid jobOfferId)
        {
            var query = _context.JobOfferLanguages
                .Where(x => x.JobOfferId == jobOfferId);

            return await query.ToListAsync();
        }
    }
}
