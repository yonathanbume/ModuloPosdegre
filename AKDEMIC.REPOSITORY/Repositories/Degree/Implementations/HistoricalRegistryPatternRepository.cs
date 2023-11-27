using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Degree.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Degree.Implementations
{
    public class HistoricalRegistryPatternRepository : Repository<HistoricalRegistryPattern> , IHistoricalRegistryPatternRepository
    {
        public HistoricalRegistryPatternRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetHistoricalRegistryPatternsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<HistoricalRegistryPattern, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.OfficeNumber;
                    break;
                default:
                    break;
            }


            var query = _context.HistoricalRegistryPatterns
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.OfficeNumber.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    number = x.OfficeNumber,
                    description = x.Description,
                    file = x.File
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }
    }
}
