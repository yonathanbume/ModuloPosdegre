using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Implementations
{
    public class PreuniversitaryExamRepository : Repository<PreuniversitaryExam> , IPreuniversitaryExamRepository
    {
        public PreuniversitaryExamRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByCode(string code, Guid preuniversitaryTermId, Guid? ignoredId = null)
            => await _context.PreuniversitaryExams.AnyAsync(x => x.PreuniversitaryTermId == preuniversitaryTermId && x.Code.ToLower().Trim() == code.ToLower().Trim() && x.Id != ignoredId);

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid? preuniversitaryTermId)
        {
            Expression<Func<PreuniversitaryExam, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.Code); break;
                case "2":
                    orderByPredicate = ((x) => x.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.PreuniversitaryTerm.Name); break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt); break;
            }

            var query = _context.PreuniversitaryExams.AsNoTracking();

            if(preuniversitaryTermId.HasValue)
                query = query.Where(x=>x.PreuniversitaryTermId == preuniversitaryTermId);

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      x.Code,
                      x.Name,
                      x.MinimumScore,
                      x.Weight,
                      DateEvaluation = x.DateEvaluation.ToDateFormat(),
                      x.PreuniversitaryTermId,
                      PreuniversitaryTerm = x.PreuniversitaryTerm.Name
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
