using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class PerformanceEvaluationQuestionRepository : Repository<PerformanceEvaluationQuestion>, IPerformanceEvaluationQuestionRepository
    {
        public PerformanceEvaluationQuestionRepository(AkdemicContext context) : base(context) { }
        public async Task<object> GetPerformanceEvaluationQuestion(Guid id)
        {
            var query = _context.PerformanceEvaluationQuestions.Where(x => x.Id == id).Select(x => new
            {
                id = x.Id,
                description = x.Description,
                templateId = x.PerformanceEvaluationTemplateId,
                criterion = x.PerformanceEvaluationCriterion.Name,
                criterionId = x.PerformanceEvaluationCriterionId
            }).FirstAsync();

            return await query;
        }

        public async Task<bool> AnyPerformanceEvaluationQuestionByDescription(string description, Guid templateId, Guid? id)
        {
            IQueryable<PerformanceEvaluationQuestion> query = _context.PerformanceEvaluationQuestions.Where(x => x.PerformanceEvaluationTemplateId == templateId).AsNoTracking();

            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(x => x.Description == description);
            }
            if (id.HasValue)
            {
                query = query.Where(x => x.Id != id);
            }

            PerformanceEvaluationQuestion result = await query.FirstOrDefaultAsync();
            return result != null;
        }

        public async Task<ReturnedData<object>> GetPerformanceEvaluationQuestionsDatatable(SentParameters sentParameters, Guid templateId, string searchValue = null)
        {
            var query = _context.PerformanceEvaluationQuestions
                .Where(x => x.PerformanceEvaluationTemplateId == templateId)
                .OrderBy(x => x.CreatedAt)
                .AsNoTracking();

            var enabledCriterions = await _context.PerformanceEvaluationTemplates.Where(x => x.Id == templateId).Select(y => y.EnabledCriterions).FirstOrDefaultAsync();

            if (enabledCriterions)
            {
                query = query.OrderBy(x => x.PerformanceEvaluationCriterion.CreatedAt).ThenBy(x => x.PerformanceEvaluationCriterion.Name);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Description.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    description = x.Description,
                    templateId = x.PerformanceEvaluationTemplateId,
                    criterion = x.PerformanceEvaluationCriterion.Name
                }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
