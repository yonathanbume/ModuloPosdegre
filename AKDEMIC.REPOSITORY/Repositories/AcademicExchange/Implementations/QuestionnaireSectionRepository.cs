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
    public class QuestionnaireSectionRepository : Repository<QuestionnaireSection> , IQuestionnaireSectionRepository
    {
        public QuestionnaireSectionRepository(AkdemicContext context) :base(context) { }

        public async Task<IEnumerable<QuestionnaireSection>> GetDetailsByQuestionnaireId(Guid questionnaireId)
        {
            var result = await _context.QuestionnaireSections.Where(x => x.QuestionnaireId == questionnaireId)
                .Select(x => new QuestionnaireSection
                {
                    Id = x.Id,
                    QuestionnaireId = x.QuestionnaireId,
                    IsStatic = x.IsStatic,
                    Title = x.Title,
                    QuestionnaireQuestions = x.QuestionnaireQuestions
                    .Select(y => new QuestionnaireQuestion
                    {
                        Description = y.Description,
                        DescriptionType = y.DescriptionType,
                        Id = y.Id,
                        IsStatic = y.IsStatic,
                        Type = y.Type,
                        QuestionnaireSectionId = y.QuestionnaireSectionId,
                        QuestionnaireAnswers = y.QuestionnaireAnswers
                        .Select(z=> new QuestionnaireAnswer
                        {
                            Id = z.Id,
                            Description = z.Description
                        })
                        .ToArray()
                    })
                    .ToArray()
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<IEnumerable<QuestionnaireSection>> GetQuestionnaireDetailsByAcademicAgreementId(Guid academicAgreementId)
        {
            var result = await _context.QuestionnaireSections.Where(x => x.Questionnaire.ScholarshipId == academicAgreementId)
                .Select(x => new QuestionnaireSection
                {
                    Id = x.Id,
                    QuestionnaireId = x.QuestionnaireId,
                    Title = x.Title,
                    QuestionnaireQuestions = x.QuestionnaireQuestions
                    .Select(y => new QuestionnaireQuestion
                    {
                        Description = y.Description,
                        DescriptionType = y.DescriptionType,
                        Id = y.Id,
                        Type = y.Type,
                        QuestionnaireSectionId = y.QuestionnaireSectionId,
                        QuestionnaireAnswers = y.QuestionnaireAnswers
                        .Select(z => new QuestionnaireAnswer
                        {
                            Id = z.Id,
                            Description = z.Description
                        })
                        .ToArray()
                    })
                    .ToArray()
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<IEnumerable<QuestionnaireSection>> GetQuestionnaireSectionsByQuestionnaireId(Guid questionnaireId)
            => await _context.QuestionnaireSections.Where(x => x.QuestionnaireId == questionnaireId).ToArrayAsync();

        public async Task<bool> AnyByTitle(Guid questionnaireId, string title)
            => await _context.QuestionnaireSections.Where(x => x.QuestionnaireId == questionnaireId).AnyAsync(x => x.Title.ToLower().Equals(title.ToLower()));
    }
}
