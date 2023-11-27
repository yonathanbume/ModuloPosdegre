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
    public class VocationalTestAnswerCareerRepository: Repository<VocationalTestAnswerCareer>, IVocationalTestAnswerCareerRepository
    {
        public VocationalTestAnswerCareerRepository(AkdemicContext context): base(context)
        {

        }

        public async Task<List<Guid>> GetCareersByAnswers(Guid vocationalTestAnswerCareerId)
        {
            return await _context.VocationalTestAnswerCareers.Where(x => x.VocationalTestAnswerId == vocationalTestAnswerCareerId).Select(x=>x.CareerId).ToListAsync();
        }

        public async Task<VocationalTestAnswerCareer> GetVocationalTestAnswerCareerFirstOrDefault(Guid vocationalTestAnswerId)
        {
            return await _context.VocationalTestAnswerCareers.Where(x => x.VocationalTestAnswerId == vocationalTestAnswerId).FirstOrDefaultAsync();
        }

        public async Task<List<VocationalTestAnswerCareer>> GetVocationalTestAnswerCareersFiltered(Guid vocationalTestAnswerId)
        {
            return await _context.VocationalTestAnswerCareers.Where(x => x.VocationalTestAnswerId == vocationalTestAnswerId).ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> VocationalTestAnswerCareerDatatable(DataTablesStructs.SentParameters sentParameters, Guid vocationalTestQuestionId, string searchValue = null)
        {
            var query = _context.VocationalTestAnswerCareers.Include(x=>x.VocationalTestAnswer).Include(x => x.Career).Where(x => x.VocationalTestAnswer.VocationalTestQuestionId == vocationalTestQuestionId).AsQueryable();            

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Career.Name,
                    x.VocationalTestAnswer.Description,                    
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
