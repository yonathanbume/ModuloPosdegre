using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeacherHiring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Implementations
{
    public class ConvocationRepository : Repository<Convocation>, IConvocationRepository
    {
        public ConvocationRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters)
        {
            Expression<Func<Convocation, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.CreatedAt); break;
                case "1":
                    orderByPredicate = ((x) => x.Name); break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt); break;
            }

            var query = _context.Convocations.AsNoTracking();

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
            => await _context.Convocations.AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.Id != ignoredId);
    }
}
