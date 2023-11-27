using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class PerformanceEvaluationRubricRepository : Repository<PerformanceEvaluationRubric>, IPerformanceEvaluationRubricRepository
    {
        public PerformanceEvaluationRubricRepository(AkdemicContext context) : base(context) { }

        public async Task<List<PerformanceEvaluationRubric>> GetPerformanceEvaluationRubricsByEvaluation(Guid evaluationId)
        {
            List<PerformanceEvaluationRubric> result = await _context.PerformanceEvaluationRubrics.Where(x => x.PerformanceEvaluationId == evaluationId).OrderBy(x => x.Min).ToListAsync();
            return result;
        }

        public async Task<int> GetMaxRatingScale(Guid evaluationId)
        {
            var templates = await _context.RelatedPerformanceEvaluationTemplates.Where(x => x.PerformanceEvaluationId == evaluationId)
                .Select(x => new
                {
                    x.PerformanceEvaluationTemplate.Max,
                    questions = x.PerformanceEvaluationTemplate.Questions.Count()
                })
                .ToListAsync();

            var result = templates.Sum(x => x.Max * x.questions);

            return result;
        }

        public async Task<bool> AnyByName(Guid evaluationId , string name, Guid? ignoredId = null)
            => await _context.PerformanceEvaluationRubrics.Where(x => x.PerformanceEvaluationId == evaluationId && x.Description.ToLower().Trim() == name.ToLower().Trim() && x.Id != ignoredId).AnyAsync();
    }
}
