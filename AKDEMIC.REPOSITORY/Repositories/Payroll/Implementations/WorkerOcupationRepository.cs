using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations
{
    public class WorkerOcupationRepository : Repository<WorkerOcupation>, IWorkerOcupationRepository
    {
        public WorkerOcupationRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<WorkerOcupation, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

            var query = _context.WorkerOcupations.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<(IEnumerable<WorkerOcupation> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter)
        {
            var query = _context.WorkerOcupations.AsQueryable();

            if (!string.IsNullOrEmpty(paginationParameter.SearchValue))
                query = query.Where(q => q.Name.Contains(paginationParameter.SearchValue));

            var count = await query.CountAsync();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.Name)
                        : query.OrderBy(q => q.Name);
                    break;
            }

            var result = await query
                .Skip(paginationParameter.CurrentNumber)
                .Take(paginationParameter.RecordsPerPage)
                .Select(q => new WorkerOcupation
                {
                    Id = q.Id,
                    Name = q.Name
                }).ToListAsync();

            return (result, count);
        }
    }
}
