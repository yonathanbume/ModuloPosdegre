using System;
using System.Linq;
using System.Threading.Tasks;

using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Implementations
{
    public class ProjectScheduleRepository : Repository<ProjectSchedule>, IProjectScheduleRepository
    {
        public ProjectScheduleRepository(AkdemicContext context) : base(context) { }

        public async Task<int> Count(Guid projectId)
        {
            return await _context.InvestigationProjectAdvances.CountAsync(x => x.ProjectId == projectId);
        }

        public async Task<object> GetProjectSchedule(Guid id)
        {
            var query = _context.InvestigationProjectAdvances.Select(x => new
            {
                id = x.Id,
            });
            return await query.FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectSchedulesDatatable(DataTablesStructs.SentParameters sentParameters, Guid projectId)
        {
            IQueryable<ProjectSchedule> query = _context.ProjectSchedules.AsNoTracking();

            query = query.Where(x => x.ProjectId == projectId);

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescending(x => x.DateTime)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    file = x.File,
                    date = x.DateTime.ToDateTimeFormat()
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
