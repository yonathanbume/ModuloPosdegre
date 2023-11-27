using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Matricula;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class TeachingHoursRangeRepository : Repository<TeachingHoursRange>, ITeachingHoursRangeRepository
    {
        public TeachingHoursRangeRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachingHoursRangeDatatable(DataTablesStructs.SentParameters sentParameters, Guid? id = null)
        {
            Expression<Func<TeachingHoursRange, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.StartTime); break;
                case "1":
                    orderByPredicate = ((x) => x.EndTime); break;
                default:
                    orderByPredicate = ((x) => x.StartTime); break;
            }

            var query = _context.TeachingHoursRanges.AsQueryable();

            if (id.HasValue)
                query = query.Where(x => x.Id == id);

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    starttime = x.StartTime,
                    endtime = x.EndTime,
                })
                .ToListAsync();

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
