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
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class ConceptGroupDetailRepository : Repository<ConceptGroupDetail>, IConceptGroupDetailRepository
    {
        public ConceptGroupDetailRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<ConceptGroupDetail>> GetAllByGroupId(Guid conceptGroupId)
        {
            var data = await _context.ConceptGroupDetails
                .Where(x => x.ConceptGroupId == conceptGroupId)
                .ToListAsync();
            return data;
        }

        public async Task<List<ConceptGroupDetail>> GetAllWithDataByGroupId(Guid conceptGroupId)
        {
            var data = await _context.ConceptGroupDetails
                .Where(x => x.ConceptGroupId == conceptGroupId)
                .Include(x => x.Concept)
                .ThenInclude(x => x.AccountingPlan)
                .ToListAsync();
            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetGroupDetailsDatatable(DataTablesStructs.SentParameters sentParameters, Guid conceptGroupId)
        {
            Expression<Func<ConceptGroupDetail, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Concept.Description);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Concept.Amount);
                    break;
                default:
                    //orderByPredicate = ((x) => x.CreatedAt);
                    break;
            }

            var query = _context.ConceptGroupDetails
                .Where(x => x.ConceptGroupId == conceptGroupId)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.ConceptId,
                    name = x.Concept.Description,
                    amount = x.Concept.Amount,
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }
    }
}
