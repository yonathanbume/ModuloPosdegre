using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations
{
    public class InstitutionalWelfareScholarshipRequirementRepository : Repository<InstitutionalWelfareScholarshipRequirement>, IInstitutionalWelfareScholarshipRequirementRepository
    {
        public InstitutionalWelfareScholarshipRequirementRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<List<InstitutionalWelfareScholarshipRequirement>> GetAllByScholarship(Guid scholarshipId)
            => await _context.InstitutionalWelfareScholarshipRequirements.Where(x => x.InstitutionalWelfareScholarshipId == scholarshipId).ToListAsync();
    }
}
