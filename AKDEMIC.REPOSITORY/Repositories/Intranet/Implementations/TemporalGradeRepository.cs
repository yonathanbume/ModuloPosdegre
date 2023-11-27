using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class TemporalGradeRepository : Repository<TemporalGrade>, ITemporalGradeRepository
    {
        public TemporalGradeRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<TemporalGrade>> GetAllByFilters(Guid sectionId, Guid evaluationId)
        {
            var grades = await _context.TemporalGrades
                .Where(x => x.StudentSection.SectionId == sectionId && x.EvaluationId == evaluationId)
                .ToListAsync();

            return grades;
        }
    }
}
