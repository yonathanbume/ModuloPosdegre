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
    public class ProviderSupplyRepository : Repository<ProviderSupply>, IProviderSupplyRepository
    {
        public ProviderSupplyRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> Exist(ProviderSupply providerSupply)
        {
            return await _context.ProviderSupplies.AnyAsync(x => x.SupplyId == providerSupply.SupplyId && x.ProviderId == providerSupply.ProviderId);
        }

        public async Task<DataTablesStructs.ReturnedData<ProviderSupply>> GetProviderSupplyDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, string search)
        {
            Expression<Func<ProviderSupply, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Supply.Name); break;
                default:
                    orderByPredicate = ((x) => x.Supply.Name); break;
            }

            IQueryable<ProviderSupply> query = _context.ProviderSupplies
                .Where(x => x.ProviderId == id)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Supply.Name.Trim().ToLower().Contains(search.Trim().ToLower()));


            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(s => new ProviderSupply
                  {
                      Id = s.Id,
                      Supply = new Supply
                      {
                          Id = s.SupplyId,
                          Name = s.Supply.Name,
                          SupplyPackage = new SupplyPackage
                          {
                              Name = s.Supply.SupplyPackage.Name
                          }
                      }
                  })
                  .ToListAsync();


            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<ProviderSupply>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
