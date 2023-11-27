using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class ConceptHistoryRepository:Repository<ConceptHistory> , IConceptHistoryRepository
    {
        public ConceptHistoryRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConceptHistoryDatatable(DataTablesStructs.SentParameters sentParameters, Guid conceptId)
        {
            Expression<Func<ConceptHistory, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.OldAmount);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Amount);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.CreatedBy);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
            }

            var query = _context.ConceptHistories
                .Where(x => x.ConceptId == conceptId)
                .AsNoTracking();
          
            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    oldAmount = x.OldAmount,
                    amount = x.Amount,
                    createdBy = x.CreatedBy,
                    createdAt = x.CreatedAt.ToLocalDateTimeFormat()
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

        public async Task<ConceptHistory> GetLastChangeByConceptId(Guid conceptId)
        {
            var query = _context.ConceptHistories
                .Where(x => x.ConceptId == conceptId)
                .OrderByDescending(x => x.CreatedAt);

            return await query.FirstOrDefaultAsync();
        }
    }
}
