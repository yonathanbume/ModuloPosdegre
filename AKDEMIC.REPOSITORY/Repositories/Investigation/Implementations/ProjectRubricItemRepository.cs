using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Implementations
{
    public class ProjectRubricItemRepository : Repository<ProjectRubricItem> , IProjectRubricItemRepository
    {
        public ProjectRubricItemRepository(AkdemicContext context) :base(context) { }

        public async Task<IEnumerable<ProjectRubricItem>> GetByProjectRubricId(Guid projectRubricId)
            => await _context.InvestigationProjectRubricItems.Where(x => x.RubricId == projectRubricId).ToArrayAsync();


        public async Task<DataTablesStructs.ReturnedData<ProjectRubricItem>> GetProjectRubricItemByProjectRubricIdDatatable(DataTablesStructs.SentParameters sentParameters,Guid projectRubricId,string search = null)
        {
            Expression<Func<ProjectRubricItem, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Indicator);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Min);
                    break;
                default:
                    orderByPredicate = ((x) => x.Max);
                    break;
            }

            var query = _context.InvestigationProjectRubricItems.Where(x => x.RubricId == projectRubricId).AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(search.Trim().ToLower()));

            query = query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            Expression<Func<ProjectRubricItem, ProjectRubricItem>> selectPredicate = (x) => new ProjectRubricItem
            {
                Id = x.Id,
                Name = x.Name,
                Indicator = x.Indicator,
                Min = x.Min,
                Max = x.Max
            };

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        public async Task<bool> AnyByNameAndProjectRubricId(Guid projectRubricId, string name, Guid? ignoredId = null)
            => await _context.InvestigationProjectRubricItems.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()) && x.RubricId == projectRubricId && x.Id != ignoredId);

        public async Task<int> GetMaxTotalbyProjectRubricId(Guid projectRubricId,Guid? ignoredId = null)
            => await _context.InvestigationProjectRubricItems.Where(x => x.RubricId == projectRubricId && x.Id != ignoredId).Select(x => (int)x.Max).SumAsync();
    }
}
