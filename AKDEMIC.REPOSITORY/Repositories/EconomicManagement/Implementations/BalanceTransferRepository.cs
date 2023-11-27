using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class BalanceTransferRepository : Repository<BalanceTransfer>, IBalanceTransferRepository
    {
        public BalanceTransferRepository(AkdemicContext context): base(context) { }

        public IQueryable<BalanceTransfer> TransfersQry(DateTime date)
            => _context.BalanceTransfers.Where(x => x.CreatedAt.Value.Date == date).AsQueryable();
        public async Task<DataTablesStructs.ReturnedData<object>> GetBalanceTransferDatatable(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<BalanceTransfer, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.FromDependency.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.ToDependency.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Amount;
                    break;
                case "4":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }


            var query = _context.BalanceTransfers
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.FromDependency.Name.Contains(search) || x.ToDependency.Name.Contains(search));
            }

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    id = x.Id,
                    amount = x.Amount,
                    from = x.FromDependency.Name,
                    fromId = x.FromDependencyId,
                    to = x.ToDependency.Name,
                    toId = x.ToDependencyId,
                    date = x.CreatedAt.ToLocalDateFormat(),
                    month = $"{x.Date:MM/yyyy}",
                    document = x.ReferenceDocument,
                    order = x.Order,
                    concept = x.Concept,
                    isCut = x.IsCutTransfer,
                    cutType = x.CutType
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<IEnumerable<BalanceTransfer>> GetAllBalanceTransfers()
        {
            var result = await _context.BalanceTransfers
                .Include(x => x.FromDependency)
                .Include(x => x.ToDependency)
                .ToListAsync();

            return result;
        }
    }
}
