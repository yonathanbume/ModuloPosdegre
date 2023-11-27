using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Implementations
{
    public class ProjectAdvanceHistoricRepository : Repository<ProjectAdvanceHistoric>, IProjectAdvanceHistoricRepository
    {
        public ProjectAdvanceHistoricRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAdvanceHistoricDataTable(DataTablesStructs.SentParameters sentParameters, Guid id, string search)
        {
            Expression<Func<ProjectAdvanceHistoric, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Update); break;
                default:
                    orderByPredicate = ((x) => x.Update); break;
            }

            IQueryable<ProjectAdvanceHistoric> query = _context.EvaluationProjectAdvanceHistorics.Where(x => x.ProjectAdvanceId == id).AsNoTracking();
            
            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .OrderBy(x => x.Update).ToListAsync();

            var result = data.Select(x => new
            {
                id = x.Id,
                date = x.Update.ToString("dd/MM/yyyy hh:mm"),
                file = x.File,
                observations = x.Observations
            }).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                result = result.Where(x => x.date.ToUpper().Contains(search)||
                                x.file.Contains(search)||
                                x.observations.Contains(search)).ToList();
            }

            int recordsTotal = result.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = result,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<ProjectAdvanceHistoric> GetWithProjectAdvance(Guid id)
        {
           return await _context.EvaluationProjectAdvanceHistorics.Include(x => x.ProjectAdvance).FirstAsync(x => x.Id == id);
        }
    }
}
