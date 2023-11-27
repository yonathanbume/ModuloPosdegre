using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Implementations
{
    public class QuestionnaireAnswerRepository : Repository<QuestionnaireAnswer> , IQuestionnaireAnswerRepository
    {
        public QuestionnaireAnswerRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<QuestionnaireAnswer>> GetAllBySectionId(Guid sectionId)
            => await _context.QuestionnaireAnswers.Where(x => x.QuestionnaireQuestion.QuestionnaireSectionId == sectionId).ToArrayAsync();

        public async Task<IEnumerable<QuestionnaireAnswer>> GetAllByQuestionId(Guid questionId)
            => await _context.QuestionnaireAnswers.Where(x => x.QuestionnaireQuestionId == questionId).ToArrayAsync();
    }
}
