using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingResearch;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingResearch.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingResearch.Implementations
{
    public class TeachingResearchConvocationRepository : Repository<Convocation>, ITeachingResearchConvocationRepository
    {
        public TeachingResearchConvocationRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConvocationsDatatable(DataTablesStructs.SentParameters parameters, string searchValue)
        {
            Expression<Func<Convocation, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.CreatedAt); break;
                case "1":
                    orderByPredicate = ((x) => x.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.StartDate); break;
                case "3":
                    orderByPredicate = ((x) => x.EndDate); break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt); break;
            }

            var query = _context.TeachingResearchConvocations.AsNoTracking();

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    startDate = x.StartDate.ToDateFormat(),
                    endDate = x.EndDate.ToDateFormat()
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _context.TeachingResearchConvocations.AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.Id != ignoredId);
    }
}
