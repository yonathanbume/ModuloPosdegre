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
    public class SectorRepository: Repository<Sector>, ISectorRepository
    {
        public SectorRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectorsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            var query = _context.Sectors
              .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Description.Contains(searchValue));
            }

            Expression<Func<Sector, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                id = x.Id,
                name = x.Description              

            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<object> GetSectorsSelect2(Guid? sectorId)
        {
            if (sectorId.HasValue)
            {
                var result = _context.Sectors.Select(x => new
                {
                    id = x.Id,
                    text = x.Description
                });
                return await result.FirstOrDefaultAsync();
            }
            else
            {
                var result = _context.Sectors.Select(x => new
                {
                    id = x.Id,
                    text = x.Description
                });
                return await result.ToListAsync();
            }
            
        }

        public async Task<object> GetSectorsSelect2V2()
        {
            var result = await _context.Sectors
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Description
                })
                .ToListAsync();

            return result;
        }

    }
}
