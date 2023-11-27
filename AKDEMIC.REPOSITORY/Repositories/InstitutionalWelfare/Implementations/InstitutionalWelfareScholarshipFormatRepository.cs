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
    public class InstitutionalWelfareScholarshipFormatRepository : Repository<InstitutionalWelfareScholarshipFormat>, IInstitutionalWelfareScholarshipFormatRepository
    {
        public InstitutionalWelfareScholarshipFormatRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<List<InstitutionalWelfareScholarshipFormat>> GetAllByScholarship(Guid scholarshipId)
            => await _context.InstitutionalWelfareScholarshipFormats.Where(x => x.InstitutionalWelfareScholarshipId == scholarshipId).ToListAsync();
    }
}
