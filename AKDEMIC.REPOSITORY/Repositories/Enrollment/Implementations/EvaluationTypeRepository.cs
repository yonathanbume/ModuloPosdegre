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
    public class EvaluationTypeRepository : Repository<EvaluationType>, IEvaluationTypeRepository
    {
        public EvaluationTypeRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyEvaluation(Guid id, Guid? termId = null)
        {
            var query = _context.EvaluationTypes.Where(x => x.Id == id && x.Evaluations.Any()).AsNoTracking();

            if (termId.HasValue)
                query = query.Where(x => x.Evaluations.Any(y => y.CourseTerm.TermId == termId));

            return await query.AnyAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<EvaluationType, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.EvaluationTypes
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Enabled
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetEvaluationTypeJson()
        {
            var result = await _context.EvaluationTypes
                .Where(x=>x.Enabled)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).OrderByDescending(x => x.text).ToListAsync();

            return result;
        }
    }
}
