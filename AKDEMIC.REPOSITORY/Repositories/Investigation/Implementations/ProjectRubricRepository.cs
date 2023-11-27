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
    public class ProjectRubricRepository : Repository<ProjectRubric>, IProjectRubricRepository
    {
        public ProjectRubricRepository(AkdemicContext context) :base(context) { }

        public async Task<IEnumerable<Select2Structs.Result>> GetProjectRubricsSelect2ClientSide(byte? type = null)
        {
            var query = _context.InvestigationProjectRubrics.AsQueryable();

            if (type.HasValue)
                query = query.Where(x => x.Type == type);

            var result = await query
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<ProjectRubric> GetByType(byte type)
            => await _context.InvestigationProjectRubrics.Where(x=>x.Type == type).FirstOrDefaultAsync();

        public async Task<DataTablesStructs.ReturnedData<ProjectRubric>> GetProjectRubricDatatable(DataTablesStructs.SentParameters sentParameters,string search = null)
        {
            Expression<Func<ProjectRubric, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Type);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Status);
                    break;
                default:
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

            var query = _context.InvestigationProjectRubrics.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(search.Trim().ToLower()));

            query = query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            Expression<Func<ProjectRubric, ProjectRubric>> selectPredicate = (x) => new ProjectRubric
            {
                Name = x.Name,
                Status = x.Status,
                Type = x.Type,
                Id = x.Id
            };

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        public async Task<IEnumerable<ProjectRubric>> GetProjectRubricsByType(byte type, bool? status = null)
        {
            var query = _context.InvestigationProjectRubrics.Where(x => x.Type == type).AsQueryable();

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            var result = await query.ToArrayAsync();

            return result;
        }

        public async Task<bool> AnyByName(string name,Guid? ignoredId = null)
            => await _context.InvestigationProjectRubrics.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()) && x.Id != ignoredId);
    }
}
