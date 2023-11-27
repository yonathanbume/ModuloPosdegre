using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class LivingCostRepository : Repository<LivingCost>, ILivingCostRepository
    {
        public LivingCostRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<LivingCost>> GetLivingCostDataTable(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<LivingCost, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Description); break;
                default:
                    orderByPredicate = ((x) => x.Description); break;
            }

            IQueryable<LivingCost> query = _context.LivingCosts
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Description.Trim().ToLower().Contains(search.Trim().ToLower()));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(s => new LivingCost
                  {
                      Id = s.Id,
                      Description = s.Description,
                      UrlFile = s.UrlFile
                  })
                  .ToListAsync();


            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<LivingCost>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _context.LivingCosts.AnyAsync(x => x.Description.ToLower().Equals(name.ToLower()) && x.Id != ignoredId);
    }
}
