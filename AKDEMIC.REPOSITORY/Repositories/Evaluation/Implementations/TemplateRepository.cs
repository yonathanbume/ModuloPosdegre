using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Implementations
{
    public class TemplateRepository : Repository<Template>, ITemplateRepository
    {
        public TemplateRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyTemplateByName(string name, Guid? id)
        {
            if (id == null)
            {
                return await _context.EvaluationTemplates.Where(x => x.Name == name).AnyAsync();
            }
            else
            {
                return await _context.EvaluationTemplates.Where(x => x.Name == name && x.Id != id).AnyAsync();
            }
        }

        public async Task<object> GetTemplate(Guid id)
        {
            var query = _context.EvaluationTemplates.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                description = x.Description
            });
            return await query.FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<IEnumerable<object>> GetTemplates()
        {
            var query = _context.EvaluationTemplates.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                description = x.Description,
                file = x.File
            });
            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTemplatesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<Template, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Description); break;
                default:
                    orderByPredicate = ((x) => x.Name); break;
            }

            IQueryable<Template> query = _context.EvaluationTemplates.AsNoTracking();

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    description = x.Description,
                    file = x.File
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
