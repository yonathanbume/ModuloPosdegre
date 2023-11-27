using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ResolutiveActs;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.ResolutiveActs.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ResolutiveActs.Implementations
{
    public class ResolutionCategoryRepository : Repository<ResolutionCategory>,IResolutionCategoryRepository
    {
        public ResolutionCategoryRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _context.ResolutionCategories.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()) && x.Id != ignoredId);

        public async Task<DataTablesStructs.ReturnedData<ResolutionCategory>> GetResolutionCategoryDatatable(DataTablesStructs.SentParameters sentParameters,string search = null)
        {
            Expression<Func<ResolutionCategory,dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

            var query = _context.ResolutionCategories.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(search.Trim().ToLower()));

            query = query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            Expression<Func<ResolutionCategory, ResolutionCategory>> selectPredicate = (x) => new ResolutionCategory
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                Name = x.Name
            };

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetResolutionCategorySelect2ClientSide()
        {
            var result = await _context.ResolutionCategories
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name
                })
                .ToArrayAsync();

            return result;
        }
    }
}
