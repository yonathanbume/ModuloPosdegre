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
    public class NonTeachingLoadActivityRepository : Repository<NonTeachingLoadActivity>, INonTeachingLoadActivityRepository
    {
        public NonTeachingLoadActivityRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetNonTeachingLoadActivitiesDatatable(DataTablesStructs.SentParameters parameters, Guid nonTeachingLoadId)
        {
            var query = _context.NonTeachingLoadActivities
                .Where(x => x.NonTeachingLoadId == nonTeachingLoadId)
                .OrderByDescending(x=>x.CreatedAt)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.CreatedAt,
                    x.Completed
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

        public async Task<bool> AnyByNonTeachingLoad(Guid nonTeachingLoadId)
            => await _context.NonTeachingLoadActivities.AnyAsync(x => x.NonTeachingLoadId == nonTeachingLoadId);
    }
}
