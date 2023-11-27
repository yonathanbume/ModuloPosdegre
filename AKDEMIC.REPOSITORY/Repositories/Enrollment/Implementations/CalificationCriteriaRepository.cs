using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class CalificationCriteriaRepository : Repository<CalificationCriteria>, ICalificationCriteriaRepository
    {
        public CalificationCriteriaRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetCalificationCriteriasObj()
        {
            var result = await _context.CalificactionCriterias
            .Where(x => x.DeletedAt == null)
            .Select(
            x => new
            {
                id = x.Id,
                name = x.Name,
                score = x.Score
            }).ToListAsync();

            return result;
        }

        public async Task<bool> GetAnyCalificationCriterias(Guid id)
        {
            var result = await _context.CalificactionCriterias.AnyAsync(x => x.Id == id);

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCalificationsCriteriasDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<CalificationCriteria, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Score); break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt); break;
            }

            var query = _context.CalificactionCriterias.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToLower().Contains(searchValue.Trim().ToLower()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Score
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
    }
}
