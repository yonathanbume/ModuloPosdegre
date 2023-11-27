using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Implementations
{
    public class CafeteriaStockRepository : Repository<CafeteriaStock>, ICafeteriaStockRepository
    {
        public CafeteriaStockRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStock(DataTablesStructs.SentParameters sentParameters, Guid? providerId, byte turnType, string searchValue = null)
        {
            var query = _context.CafeteriaStocks                   
                    .Include(x => x.ProviderSupply.Supply.SupplyPackage)
                    .Include(x => x.ProviderSupply)
                    .ThenInclude(x => x.Supply)
                    .AsQueryable();
            if (providerId.HasValue)
            {
                query = query.Where(x => x.ProviderSupply.ProviderId == providerId.Value);
            }
            else
            {
                query = query.Where(x => x.ProviderSupply.ProviderId == new Guid());
            }

            if (turnType >0 )
            {
                query = query.Where(x => x.TurnId == turnType);
            }
            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.ProviderSupply.Supply.Name.Contains(searchValue) || x.Quantity.ToString().Contains(searchValue));
            }

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    supplyName = x.ProviderSupply.Supply.Name,
                    quantity = x.Quantity,
                    supplyPackageName = x.ProviderSupply.Supply.SupplyPackage.Name,
                    Turn = AKDEMIC.CORE.Helpers.ConstantHelpers.TURN_TYPE.VALUES[x.TurnId]
                })
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

        public async Task<CafeteriaStock> GetStockByProviderSupplyIdAndTurn(Guid ProviderSupplyId, byte Turn)
        {
            var result = _context.CafeteriaStocks.Include(x => x.ProviderSupply.Supply).Where(x => x.ProviderSupplyId == ProviderSupplyId && x.TurnId == Turn);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<decimal> GetStockByProviderSupplySum(Guid ProviderSupplyId, byte Turn)
        {
            var result = await _context.CafeteriaStocks.Include(x => x.ProviderSupply.Supply).Where(x => x.ProviderSupplyId == ProviderSupplyId && x.TurnId == Turn).SumAsync(x=>x.Quantity);
            return result;
        }

        public async Task<CafeteriaStock> GetStockBySupplyIdAndProviderId(Guid SupplyId, Guid ProviderId)
        {
            var result = _context.CafeteriaStocks.Include(x => x.ProviderSupply.Supply).Where(x => x.ProviderSupply.SupplyId == SupplyId && x.ProviderSupply.ProviderId == ProviderId);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<object> GetSupplyByProvider(Guid ProviderId, byte TurnType)
        {
            var result = await _context.CafeteriaStocks.Include(x => x.ProviderSupply.Supply).Where(x => x.ProviderSupply.ProviderId == ProviderId && x.TurnId == TurnType)
                .Select(x => new {
                    Id = x.ProviderSupplyId,
                    Text = x.ProviderSupply.Supply.Name
                }).ToListAsync();
            return result;
        }
    }
}
