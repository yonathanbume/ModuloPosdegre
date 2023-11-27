using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class PettyCashBookRepository : Repository<PettyCashBook>, IPettyCashBookRepository
    {
        public PettyCashBookRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPettyCashBookDataTable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null)
        {
            Expression<Func<PettyCashBook, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Date);
                    break;

                case "1":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;

                case "2":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                default:
                    break;
            }

            var query = _context.PettyCashBooks.AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.CASHIER) && !user.IsInRole(ConstantHelpers.ROLES.TREASURY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId)) query = query.Where(x => x.UserId == userId);
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    date = x.Date.ToLocalDateFormat(),
                    code = x.User.UserName,
                    user = x.User.FullName
                }).ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            return result;
        }

    }
}
