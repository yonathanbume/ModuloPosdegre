using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class GradeReportRequirementRepository : Repository<GradeReportRequirement>, IGradeReportRequirementRepository
    {
        public GradeReportRequirementRepository(AkdemicContext context): base(context)
        {

        }

        public async Task<List<GradeReportRequirement>> GetRequirementsByGradeReport(Guid gradeReportId)
        {
            var result = await _context.GradeReportRequirements.Include(x=>x.DegreeRequirement)
                .Where(x => x.GradeReportId == gradeReportId).ToListAsync();
            return result;
        }
    }
}
