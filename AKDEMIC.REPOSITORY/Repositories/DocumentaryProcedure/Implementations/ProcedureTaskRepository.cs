using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class ProcedureTaskRepository : Repository<ProcedureTask>, IProcedureTaskRepository
    {
        public ProcedureTaskRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid procedureId, string search)
        {
            var query = _context.ProcedureTasks
                .OrderBy(x => x.CreatedAt)
                .Where(x => x.ProcedureId == procedureId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Description.ToLower().Trim().Contains(search.Trim().ToLower()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
               .Skip(parameters.PagingFirstRecord)
               .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    createdAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    x.Description,
                    x.ActivityType,
                    x.Type,
                    x.RecordHistoryType
                })
               .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<ProcedureTask>> GetProcedureTasks(Guid procedureId)
            => await _context.ProcedureTasks.Where(x => x.ProcedureId == procedureId).ToListAsync();
    }
}
