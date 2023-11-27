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
    public class InstitutionalWelfareAnswerRepository : Repository<InstitutionalWelfareAnswer>, IInstitutionalWelfareAnswerRepository
    {
        public InstitutionalWelfareAnswerRepository(AkdemicContext akdemicContext) : base(akdemicContext)
        {

        }

        public async Task<IEnumerable<InstitutionalWelfareAnswer>> GetAllBySectionId(Guid sectionId)
           => await _context.InstitutionalWelfareAnswers.Where(x => x.InstitutionalWelfareQuestion.InstitutionalWelfareSectionId == sectionId).ToArrayAsync();

        public async Task<IEnumerable<InstitutionalWelfareAnswer>> GetAllByQuestionId(Guid questionId)
            => await _context.InstitutionalWelfareAnswers.Where(x => x.InstitutionalWelfareQuestionId == questionId).ToArrayAsync();

    }
}
