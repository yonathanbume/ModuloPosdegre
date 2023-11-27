using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class VocationalTestQuestionRepository : Repository<VocationalTestQuestion>, IVocationalTestQuestionRepository
    {
        public VocationalTestQuestionRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<List<VocationalTestQuestion>> GetActiveVocationalTestQuestions()
        {
            var vocationalTestQuestions = await _context.VocationalTestQuestions.Include(x => x.vocationalTestAnswers).Where(x => x.VocationalTest.IsActive == true).ToListAsync();
            return vocationalTestQuestions;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> VocationalTestQuestionDatatable(DataTablesStructs.SentParameters sentParameters, Guid vocationalTestId, string searchValue = null)
        {
            var query = _context.VocationalTestQuestions.Where(x=>x.VocationalTestId == vocationalTestId).AsQueryable();
            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Description.Contains(searchValue));
            }

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
