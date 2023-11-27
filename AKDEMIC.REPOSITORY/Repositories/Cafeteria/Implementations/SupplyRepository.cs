using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Implementations
{
    public class SupplyRepository : Repository<Supply>, ISupplyRepository
    {
        public SupplyRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<Supply>> GetSuppliesDataTable(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<Supply, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.UnitMeasurement.Description); break;
                case "2":
                    orderByPredicate = ((x) => x.SupplyPackage.Name); break;
                default:
                    orderByPredicate = ((x) => x.Name); break;
            }
            var query = _context.Supplies
                .Include(x => x.UnitMeasurement)
                .Include(x => x.SupplyPackage)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower().Trim();
                query = query.Where(x => x.Name.ToLower().Contains(search) || x.UnitMeasurement.Description.Contains(search));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<Supply>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<Select2Structs.ResponseParameters> GetSuppliesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetSuppliesSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name + " ( " + x.UnitMeasurement.Description + " )",
            }, (x) => new[] { x.Name }, searchValue);
        }
        private async Task<Select2Structs.ResponseParameters> GetSuppliesSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Supply, Select2Structs.Result>> selectPredicate, Func<Supply, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.Supplies
                .WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        public async Task<object> GetSupplySelect(Guid providerId)
        {
            var result = await _context.ProviderSupplies
                .Where(x => x.ProviderId == providerId)
                .Include(x => x.Supply.UnitMeasurement)
                .Include(x => x.Supply.SupplyPackage)
                .Select(x => new
                {
                    id = x.Id + "/" + x.Supply.SupplyPackage.Name,
                    text = x.Supply.Name + " (" + x.Supply.UnitMeasurement.Description + ")"
                }).ToListAsync();
            return result;
        }
    }
}
