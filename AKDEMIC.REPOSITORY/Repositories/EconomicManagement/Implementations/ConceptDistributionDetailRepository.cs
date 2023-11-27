using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class ConceptDistributionDetailRepository:Repository<ConceptDistributionDetail> , IConceptDistributionDetailRepository
    {
        public ConceptDistributionDetailRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<ConceptDistributionDetail>> GetAllNonUnitByConceptDistribution(Guid conceptDistributionId)
        {
            var query = _context.ConceptDistributionDetails
                .Where(x => x.ConceptDistributionId == conceptDistributionId && !x.IsUnit);

            return await query.ToListAsync();
        }
    }
}
