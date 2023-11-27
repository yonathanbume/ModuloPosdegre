using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class WorkerManagementPositionRepository : Repository<WorkerManagementPosition> , IWorkerManagementPositionRepository
    {
        public WorkerManagementPositionRepository(AkdemicContext context) :base(context)  { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkerManagementPositionDatatable(DataTablesStructs.SentParameters sentParameters, int? status , string searchValue = null)
        {
            Expression<Func<WorkerManagementPosition, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code); break;
                case "1":
                    orderByPredicate = ((x) => x.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.Dependency.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.Status); break;

            }

            var query = _context.WorkerManagementPositions.Include(x => x.Dependency).AsQueryable();

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Code.ToUpper().Contains(searchValue.ToUpper())
                                    || x.Name.ToUpper().Contains(searchValue.ToUpper())
                                    || x.Dependency.Name.ToUpper().Contains(searchValue.ToUpper()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new 
                  {
                      id = x.Id,
                      code = x.Code,
                      dependency = x.Dependency.Name,
                      name = x.Name,
                      status = x.Status
                  })
                  .ToListAsync();


            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<IEnumerable<WorkerManagementPosition>> GetAll(string search, bool? onlyActive)
        {
            var query = _context.WorkerManagementPositions.AsQueryable();
            if (onlyActive.HasValue && onlyActive.Value)
                query = query.Where(x => x.Status == ConstantHelpers.STATES.ACTIVE);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToLower().Trim().Contains(search.ToLower().Trim()));

            return await query.ToArrayAsync();
        }

        public async Task<bool> AnyByCode(string code, Guid? ignoredId = null)
            => await _context.WorkerManagementPositions.AnyAsync(x => x.Code.ToLower() == code.ToLower() && x.Id != ignoredId);

        public async Task<bool> AnyByName(string name, Guid dependencyId ,Guid? ignoredId = null)
            => await _context.WorkerManagementPositions.AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.DependencyId == dependencyId && x.Id != ignoredId);
    }
}
