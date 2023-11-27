using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class PerformanceEvaluationCriterionRepository : Repository<PerformanceEvaluationCriterion>, IPerformanceEvaluationCriterionRepository
    {
        public PerformanceEvaluationCriterionRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid templateId, string searchValue)
        {
            var query = _context.PerformanceEvaluationCriterions
                .Where(x => x.PerformanceEvaluationTemplateId == templateId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }


        public async Task<bool> AnyByName(string name, Guid templateId, Guid? ignoredId)
            => await _context.PerformanceEvaluationCriterions.AnyAsync(x => x.Name.ToLower().Trim() == name.ToLower().Trim() && x.PerformanceEvaluationTemplateId == templateId && x.Id != ignoredId);

        public async Task<bool> AnyQuestions(Guid id)
            => await _context.PerformanceEvaluationCriterions.Where(x => x.Id == id).AnyAsync(x => x.PerformanceEvaluationQuestions.Any());

        public async Task<Select2Structs.ResponseParameters> GetCriterionsSelect2(Select2Structs.RequestParameters parameters, Guid templateId, string search)
        {
            var query = _context.PerformanceEvaluationCriterions
                .Where(x=>x.PerformanceEvaluationTemplateId == templateId)
                .OrderBy(x=>x.CreatedAt)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToLower().Trim().Contains(search.ToLower().Trim()));

            var currentPage = parameters.CurrentPage != 0 ? parameters.CurrentPage - 1 : 0;
            var results = await query
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(x=> new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name
                })
                .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }

        public async Task<List<PerformanceEvaluationCriterion>> GetCriterions(Guid templateId)
            => await _context.PerformanceEvaluationCriterions.Where(x => x.PerformanceEvaluationTemplateId == templateId).OrderBy(x=>x.CreatedAt).ToListAsync();

        public async Task<List<PerformanceEvaluationCriterion>> GetAll(Guid templateId)
            => await _context.PerformanceEvaluationCriterions.Where(x => x.PerformanceEvaluationTemplateId == templateId).ToListAsync();
    }
}
