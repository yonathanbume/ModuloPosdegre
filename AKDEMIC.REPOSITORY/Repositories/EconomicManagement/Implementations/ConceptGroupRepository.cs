using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class ConceptGroupRepository : Repository<ConceptGroup>, IConceptGroupRepository
    {
        public ConceptGroupRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConceptGroupsDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string search = null)
        {
            Expression<Func<ConceptGroup, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Details.Count);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Details.Sum(y => y.Concept.Amount));
                    break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
            }

            var query = _context.ConceptGroups
                .AsNoTracking();

            if (user != null && !user.IsInRole(ConstantHelpers.ROLES.SUPERADMIN))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                    query = query.Where(x => x.UserId == userId);
            }

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    count = x.Details.Count,
                    total = x.Details.Sum(y => y.Concept.Amount)
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
