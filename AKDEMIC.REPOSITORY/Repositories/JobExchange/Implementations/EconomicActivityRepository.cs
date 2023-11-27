using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class EconomicActivityRepository : Repository<EconomicActivity>, IEconomicActivityRepository
    {
        public EconomicActivityRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> ExistCode(string code, Guid? Id)
        {
            var query = _context.EconomicActivities.AsQueryable();
            if (Id.HasValue)
            {
                query = query.Where(x => x.Id != Id);
            }
            return await query.AnyAsync(x => x.Code == code);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEcnomicActivityDatatable(DataTablesStructs.SentParameters sentParameters, Guid? economicActivityDivisionId = null, string searchValue = null)
        {
            Expression<Func<EconomicActivity, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.EconomicActivityDivision.Name);
                    break;
            }

            var query = _context.EconomicActivities.AsNoTracking();

            if (economicActivityDivisionId != null)
                query = query.Where(x => x.EconomicActivityDivisionId == economicActivityDivisionId);

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
                    EconomicActivityDivision = x.EconomicActivityDivision.Name ?? ""
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

        public async Task<object> GetClientSideSelect2(Guid? economicActivityDivisionId = null)
        {
            var query = _context.EconomicActivities.AsNoTracking();

            if (economicActivityDivisionId != null)
                query = query.Where(x => x.EconomicActivityDivisionId == economicActivityDivisionId);

            var result = await query.
                Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .ToListAsync();

            return result;
        }
    }


}
