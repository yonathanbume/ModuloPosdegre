using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations
{
    public class InstitutionalWelfareQuestionRepository : Repository<InstitutionalWelfareQuestion>, IInstitutionalWelfareQuestionRepository
    {
        public InstitutionalWelfareQuestionRepository(AkdemicContext akdemicContext): base(akdemicContext)
        {

        }
        public async Task<IEnumerable<InstitutionalWelfareQuestion>> GetAllBySectionId(Guid sectionId)
          => await _context.InstitutionalWelfareQuestions.Where(x => x.InstitutionalWelfareSectionId == sectionId).ToArrayAsync();

        public async  Task<bool> AnyByDescription(Guid sectionId, string description, Guid? ignoredId)
        {
           return await _context.InstitutionalWelfareQuestions.Where(x => x.InstitutionalWelfareSectionId == sectionId).AnyAsync(x => x.Description.ToLower().Equals(description.ToLower()) && x.Id != ignoredId);
        }

        public async Task<IEnumerable<InstitutionalWelfareQuestion>> GetAllByRecordId(Guid recordId)
        {
           return await _context.InstitutionalWelfareQuestions.Where(x => x.InstitutionalWelfareSection.InstitutionalWelfareRecordId== recordId).ToArrayAsync();
        }

        public async Task<InstitutionalWelfareQuestion> GetWithIncludes(Guid id)
        {
            return await _context.InstitutionalWelfareQuestions.Include(x=>x.InstitutionalWelfareSection.InstitutionalWelfareRecord).Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<RecordQuestionExcelTemplate>> GetQuestionForExcelByRecord(Guid recordId)
        {
            var result = await _context.InstitutionalWelfareQuestions
                    .Where(x => x.InstitutionalWelfareSection.InstitutionalWelfareRecordId == recordId)
                    .Select(x => new RecordQuestionExcelTemplate
                    {
                        QuestionId = x.Id,
                        Question = x.Description,
                        Type = x.Type
                    }).ToListAsync();

            return result;
        }
    }
}