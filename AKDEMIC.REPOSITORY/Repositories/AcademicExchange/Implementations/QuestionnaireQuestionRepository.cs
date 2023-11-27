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
    public class QuestionnaireQuestionRepository : Repository<QuestionnaireQuestion>, IQuestionnaireQuestionRepository
    {
        public QuestionnaireQuestionRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<QuestionnaireQuestion>> GetAllBySectionId(Guid sectionId)
            => await _context.QuestionnaireQuestions.Where(x => x.QuestionnaireSectionId == sectionId).ToArrayAsync();

        public async Task<bool> AnyByDescription(Guid sectionId, string description, Guid? ignoredId = null)
            => await _context.QuestionnaireQuestions.Where(x => x.QuestionnaireSectionId == sectionId).AnyAsync(x => x.Description.ToLower().Equals(description.ToLower()) && x.Id != ignoredId);

        public async Task<IEnumerable<QuestionnaireQuestion>> GetAllByQuestionnaireId(Guid questionnaireId)
            => await _context.QuestionnaireQuestions.Where(x => x.QuestionnaireSection.QuestionnaireId == questionnaireId).ToArrayAsync();

        public async Task<QuestionnaireQuestion> GetByDescriptionAndSectionTitle(string description, string sectionTitle, Guid scholarshipId)
            => await _context.QuestionnaireQuestions.Where(x => x.Description.Equals(description) && x.QuestionnaireSection.Title.Equals(sectionTitle) && x.QuestionnaireSection.Questionnaire.ScholarshipId == scholarshipId).FirstOrDefaultAsync();
    }
}
