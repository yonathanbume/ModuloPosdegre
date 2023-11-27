using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations
{
    public class InstitutionalWelfareSectionRepository : Repository<InstitutionalWelfareSection>, IInstitutionalWelfareSectionRepository
    {
        public InstitutionalWelfareSectionRepository(AkdemicContext akdemicContext) : base(akdemicContext)
        { 
        }

        public async Task<IEnumerable<InstitutionalWelfareSection>> GetDetailsByRecordId(Guid recordId)
        {
            var result = await _context.InstitutionalWelfareSections
                .Include(x => x.InstitutionalWelfareQuestions)
                .ThenInclude(x => x.InstitutionalWelfareAnswerByStudents)
                .ThenInclude(x => x.InstitutionalWelfareAnswer)
                .Where(x => x.InstitutionalWelfareRecordId == recordId)
                .Select(x => new InstitutionalWelfareSection
                {
                    Id = x.Id,
                    InstitutionalWelfareRecordId = x.InstitutionalWelfareRecordId,
                    Title = x.Title,
                    MaxScore = x.MaxScore,
                    InstitutionalWelfareQuestions = x.InstitutionalWelfareQuestions
                    .Select(y => new InstitutionalWelfareQuestion
                    {
                        Description = y.Description,
                        DescriptionType = y.DescriptionType,
                        Id = y.Id,
                        Type = y.Type,
                        Score = y.Score,
                        InstitutionalWelfareSectionId = y.InstitutionalWelfareSectionId,
                        InstitutionalWelfareAnswers = y.InstitutionalWelfareAnswers
                        .Select(z => new InstitutionalWelfareAnswer
                        {
                            Id = z.Id,
                            Description = z.Description,
                            Score = z.Score
                        })
                        .ToArray()
                    })
                    .ToArray()
                })
                .ToArrayAsync();

            return result;
        }


        public async Task<IEnumerable<InstitutionalWelfareSection>> GetInstitutionalWelfareSectionsByRecordId(Guid institutionalWelfareRecordId, byte? sisfohClasification = 0, Guid? categorizationLevelId = null, Guid? careerId = null)
        {
     
            var query = _context.InstitutionalWelfareSections
                .Include(x => x.InstitutionalWelfareRecord)
                    .ThenInclude(x => x.InstitutionalRecordCategorizationByStudents)
                .Include(x => x.InstitutionalWelfareQuestions)
                    .ThenInclude(x => x.InstitutionalWelfareAnswers)
                .Include(x => x.InstitutionalWelfareQuestions)
                    .ThenInclude(x => x.InstitutionalWelfareAnswerByStudents)
                    .ThenInclude(x => x.InstitutionalWelfareAnswer)
                .Where(x => x.InstitutionalWelfareRecordId == institutionalWelfareRecordId)
                .AsNoTracking();


            if (sisfohClasification != null && sisfohClasification != 0)
            {
                query = query.Where(x => x.InstitutionalWelfareRecord.InstitutionalRecordCategorizationByStudents.Any(y => y.SisfohClasification == sisfohClasification));
            }

            if (categorizationLevelId != null)
                query = query.Where(x => x.InstitutionalWelfareRecord.InstitutionalRecordCategorizationByStudents.Any(y => y.CategorizationLevelId == categorizationLevelId));

            if (careerId != null)
                query = query.Where(x => x.InstitutionalWelfareRecord.InstitutionalRecordCategorizationByStudents.Any(y => y.Student.CareerId == careerId));

            return await query.ToListAsync();
        }

        public async Task<bool> AnyByTitle(Guid recordId, string title,Guid? ignoredId = null)
            => await _context.QuestionnaireSections.Where(x => x.QuestionnaireId == recordId && x.Id != ignoredId).AnyAsync(x => x.Title.ToLower().Equals(title.ToLower()));

        public async Task<InstitutionalWelfareSection> GetWithIncludes(Guid id)
        {
            return await _context.InstitutionalWelfareSections.Include(x=>x.InstitutionalWelfareRecord).Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
