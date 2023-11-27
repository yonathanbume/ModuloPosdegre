using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Implementations
{
    public class QuestionnaireRepository : Repository<Questionnaire>, IQuestionnaireRepository
    {
        public QuestionnaireRepository(AkdemicContext context) : base(context) { }

        public async Task<Questionnaire> GetByScholarshipId(Guid scholarshipId)
            => await _context.Questionnaires.Where(x => x.ScholarshipId == scholarshipId).FirstOrDefaultAsync();
    }
}
