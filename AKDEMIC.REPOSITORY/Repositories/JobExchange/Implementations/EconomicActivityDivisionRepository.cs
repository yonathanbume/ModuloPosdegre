using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class EconomicActivityDivisionRepository : Repository<EconomicActivityDivision>, IEconomicActivityDivisionRepository
    {
        public EconomicActivityDivisionRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByCode(string code, Guid? id = null)
        {
            return await _context.EconomicActivityDivisions.AnyAsync(x => x.Code.ToUpper() == code.ToUpper() && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, Guid? economicActivitySectionId = null, string searchValue = null)
        {
            Expression<Func<EconomicActivityDivision, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.EconomicActivitySection.Name);
                    break;
            }

            var query = _context.EconomicActivityDivisions.AsNoTracking();

            if (economicActivitySectionId != null)
                query = query.Where(x => x.EconomicActivitySectionId == economicActivitySectionId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Code.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    EconomicActivitySection = x.EconomicActivitySection.Name ?? ""
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<object> GetClientSideSelect2(Guid? economicActivitySectionId = null)
        {
            var query = _context.EconomicActivityDivisions.AsNoTracking();

            if (economicActivitySectionId != null)
                query = query.Where(x => x.EconomicActivitySectionId == economicActivitySectionId);

            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.Code}-{x.Name}"
                })
                .ToListAsync();

            return result;
        }
    }
}
