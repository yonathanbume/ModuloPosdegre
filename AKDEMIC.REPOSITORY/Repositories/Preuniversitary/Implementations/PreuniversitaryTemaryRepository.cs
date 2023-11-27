using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Implementations
{
    public class PreuniversitaryTemaryRepository : Repository<PreuniversitaryTemary> , IPreuniversitaryTemaryRepository
    {
        public PreuniversitaryTemaryRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTemariesDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseId , Guid termId, string searchValue = null)
        {
            Expression<Func<PreuniversitaryTemary, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.Topic); break;
                default:
                    orderByPredicate = ((x) => x.Topic); break;
            }

            var query = _context.PreuniversitaryTemaries
                .Where(x => x.PreuniversitaryCourseId == courseId)
                .Where(x => x.PreuniversitaryTermId == termId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Topic.Contains(searchValue));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    topic = x.Topic
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

        public async Task<object> GetTemariesListByCourseIdAndTermId(Guid courseId,Guid termId)
            => await _context.PreuniversitaryTemaries.Where(x => x.PreuniversitaryCourseId == courseId && x.PreuniversitaryTermId == termId)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Topic
                }).AsNoTracking().ToListAsync();
    }
}
