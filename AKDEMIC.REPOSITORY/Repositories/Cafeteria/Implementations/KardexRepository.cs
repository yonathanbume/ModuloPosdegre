using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Implementations
{
    public class KardexRepository : Repository<CafeteriaKardex>, IKardexRepository
    {
        public KardexRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetKardexDataTable(DataTablesStructs.SentParameters sentParameters, int type, DateTime startDate, DateTime endDate, Guid? ProviderId, string search)
        {
            Expression<Func<CafeteriaKardex, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.ProviderSupply.Supply.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Type;
                    break;
                case "2":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Quantity;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }

            var query = _context.CafeteriaKardexes
                                .Include(x => x.ProviderSupply.Supply)
                            .AsQueryable();
            if (ProviderId.HasValue)
            {
                query = query.Where(x => x.ProviderSupply.ProviderId == ProviderId.Value);
            }

            if (type != 0)
                query = query.Where(x => x.Type == type);

            if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
                query = query.Where(x => x.CreatedAt.Value.Date >= startDate.Date && x.CreatedAt.Value.Date <= endDate.Date);
            if (startDate != DateTime.MinValue && endDate == DateTime.MinValue)
                query = query.Where(x => startDate.Date == x.CreatedAt.Value.Date);
            if (startDate == DateTime.MinValue && endDate != DateTime.MinValue)
                query = query.Where(x => endDate.Date == x.CreatedAt.Value.Date);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.ProviderSupply.Supply.Name.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x=> new {
                    classifierCode = x.ProviderSupply.Supply.ClassifierCode,
                    supplyName = x.ProviderSupply.Supply.Name,
                    unitMeasurement = x.ProviderSupply.Supply.UnitMeasurement.Description,
                    x.FormmatedType,
                    CreatedAt = x.CreatedAt.ToLocalDateFormat(),
                    x.Quantity,
                    x.Type
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)                
                .ToListAsync();

            var recordsTotal = data.Count;

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
