using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Indicators;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Indicators.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Indicators.Implementations
{
    public class ResearchPerYearRepository : Repository<ResearchPerYear>, IResearchPerYearRepository
    {
        public ResearchPerYearRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> AnyByYear(int year, Guid? id = null)
        {
            var query = _context.ResearchPerYears
                .Where(x => x.Year == year)
                .AsNoTracking();

            if (id.HasValue && id.Value != Guid.Empty)
                query = query.Where(x => x.Id != id);

            return await query.AnyAsync();
        }

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<ResearchPerYear, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Year;
                    break;
                case "1":
                    orderByPredicate = (x) => x.PublishedResearch;
                    break;
                case "2":
                    orderByPredicate = (x) => x.TotalInvestigations;
                    break;
                default:
                    orderByPredicate = (x) => x.Year;
                    break;
            }

            var query = _context.ResearchPerYears.AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    year = x.Year,
                    published = x.PublishedResearch,
                    total = x.TotalInvestigations
                })
                .ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<object> GetReportPieChart(int year)
        {
            var researchPerYear = await _context.ResearchPerYears.FirstOrDefaultAsync(x => x.Year == year);

            var data = new List<dynamic>
            {
                new { name = "Investigaciones Publicadas"  , y = researchPerYear == null ? 0 : researchPerYear.PublishedResearch},
                new { name = "Investigaciones Sin Publicar"  , y = researchPerYear == null ? 0 : researchPerYear.TotalInvestigations - researchPerYear.PublishedResearch},
            };

            return new { data };
        }
    }
}
