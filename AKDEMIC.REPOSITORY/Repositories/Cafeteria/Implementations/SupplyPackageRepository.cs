using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Implementations
{
    public class SupplyPackageRepository : Repository<SupplyPackage>, ISupplyPackageRepository
    {
        public SupplyPackageRepository(AkdemicContext context): base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSupplyPackages(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            var query = _context.SupplyPackages.AsQueryable();           

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Contains(searchValue));

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();

            var data = await query                
                .Select(x => new {
                    x.Id,
                    x.Name
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

        public async Task<object> GetSupplyPackageSelect()
        {
            var query = await _context.SupplyPackages.Select(x => new {
                x.Id,
                Text = x.Name
            }).ToListAsync();
            return query;
        }
    }
}
