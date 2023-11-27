using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class VocationalTestAnswerRepository : Repository<VocationalTestAnswer>, IVocationalTestAnswerRepository
    {
        public VocationalTestAnswerRepository(AkdemicContext context): base(context)
        {

        }

        public async Task<DataTablesStructs.ReturnedData<object>> VocationalTestAnswersDatatable(DataTablesStructs.SentParameters sentParameters, Guid VocationalTestQuestionId, string searchValue = null)
        {
            var query = _context.VocationalTestAnswers.Include(x=>x.VocationalTestAnswerCareers).Where(x=>x.VocationalTestQuestionId == VocationalTestQuestionId).AsQueryable();
            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Description                       
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
    }
}
