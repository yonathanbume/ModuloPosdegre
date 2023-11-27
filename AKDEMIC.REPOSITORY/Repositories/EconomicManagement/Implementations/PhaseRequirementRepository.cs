using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class PhaseRequirementRepository : Repository<PhaseRequirement>, IPhaseRequirementRepository
    {
        public PhaseRequirementRepository(AkdemicContext context):base(context) { }

        public async Task<object> GetSelectProcess(Guid id)
        {
            var data = await _context.Requirements.Include(x => x.RequirementSuppliers).Include(x => x.PhaseRequirement).Where(x => x.PhaseRequirementId == id)
                .Select(x => new
                {
                    x.PhaseRequirement.Summoned,
                    x.PhaseRequirement.Spoiled,
                    x.PhaseRequirement.Id,
                    x.PhaseRequirement.GrantingGoodPro,
                    x.PhaseRequirement.ContractNumber,
                    x.PhaseRequirement.ServiceOrPurchaseOrder,
                    x.PhaseRequirement.ServiceOrPurchaseOrderNumber,
                    x.PhaseRequirement.Awarded,
                    awardedDate = $"{x.PhaseRequirement.AwardedDate:dd/MM/yyyy}",
                    grantingGoodProDate = $"{x.PhaseRequirement.GrantingGoodProDate:dd/MM/yyyy}",
                    spoiledDate = $"{x.PhaseRequirement.SpoiledDate:dd/MM/yyyy}",
                    summonedDate = $"{x.PhaseRequirement.SummonedDate:dd/MM/yyyy}",
                    x.PhaseRequirement.DescriptionAwarded,
                    x.PhaseRequirement.DescriptionSpoiled,
                    ratioAwarded = x.PhaseRequirement.RatioAwarded.HasValue ? x.PhaseRequirement.RatioAwarded.Value : 1,
                    AmountAwarded = x.PhaseRequirement.AmountAwarded.HasValue ? x.PhaseRequirement.AmountAwarded.Value : 0,
                    startDateExecution = $"{x.PhaseRequirement.StartDateExecution:dd/MM/yyyy}",
                    TermDateExecution = x.PhaseRequirement.TermDateExecution.HasValue ? x.PhaseRequirement.TermDateExecution.Value : 0,
                    endDateExecution = $"{x.PhaseRequirement.EndDateExecution:dd/MM/yyyy}",
                    supplierId = x.SupplierId
                }).FirstOrDefaultAsync();

            return data;
        }
    }
}
