using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
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
    public class AbilityRepository:Repository<Ability> ,IAbilityRepository
    {
        public AbilityRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAbilityDatatable(DataTablesStructs.SentParameters sentParameters, int status, string searchValue = null)
        {
            Expression<Func<Ability, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            var query = _context.Abilities
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            query = query.Where(x => x.Status == status);

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Description.Contains(searchValue));
            }

            Expression<Func<Ability, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                id = x.Id,
                x.Description,
                CreatedAt = x.CreatedAt.Value.ToLocalDateFormat(),
                x.Status,
                x.CreatedBy
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<object> GetAbilitiesSelect2ClientSide()
        {
            var result = await _context.Abilities
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Description
                })
                .OrderBy(x => x.text)
                .ToListAsync();

            return result;
        }

    }
}
