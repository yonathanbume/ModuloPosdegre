using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class SurveyItemRepository : Repository<SurveyItem>, ISurveyItemRepository
    {
        public SurveyItemRepository(AkdemicContext context) : base(context) { }

        #region PUBLIC

        public async Task<IEnumerable<SurveyItem>> GetBySurvey(Guid surveyId)
        {
            var query = _context.SurveyItems
                .Where(x => x.SurveyId == surveyId)
                .Select(x => new SurveyItem
                {
                    Id = x.Id,
                    Title = x.Title,
                    IsLikert = x.IsLikert,
                    Questions = x.Questions.Select(y => new Question
                    {
                        Id = y.Id,
                        Type = y.Type,
                        Description = y.Description,
                        Answers = y.Answers.Select(z => new Answer
                        {
                            Id = z.Id,
                            Description = z.Description
                        }).ToList()
                    }).ToList()
                });

            return await query.ToListAsync();
        }

        public override async Task DeleteById(Guid id)
        {
            var surveyItem = await _context.SurveyItems
                            .Include(x => x.Questions)
                                .ThenInclude(x => x.Answers)
                            .Where(x => x.Id == id)
                            .FirstOrDefaultAsync();

            foreach (var question in surveyItem.Questions)
            {
                _context.Answer.RemoveRange(question.Answers);
            }
            _context.Question.RemoveRange(surveyItem.Questions);
            await _context.SaveChangesAsync();
            await base.DeleteById(id);
        }

        public async Task<List<SurveyItemReportTemplate>> GetSurveyItemTemplate(Guid surveyId, bool? graduated = null)
        {
            var result = new List<SurveyItemReportTemplate>();

            if (graduated != null)
            {
                result = await _context.SurveyItems
                .Where(x => x.SurveyId == surveyId)
                .Select(x => new SurveyItemReportTemplate
                {
                    Title = x.Title,
                    Reportes = x.Questions
                        .Select(y => new QuestionReportTemplate
                        {
                            Id = y.Id,
                            Name = y.Description,
                            Type = y.Type,
                            Alternatives = y.Answers
                                .Select(z => new AlternativeTemplate
                                {
                                    Description = z.Description,
                                    Count = z.AnswerByUsers
                                        .Where(au => au.Question.Type != ConstantHelpers.SURVEY.TEXT_QUESTION && au.SurveyUser.IsGraduated == graduated)
                                        .GroupBy(au => au.AnswerId)
                                        .Select(au => au.Count()).FirstOrDefault()
                                }).ToList()
                        }).ToList()
                })
                .ToListAsync();
            }
            else
            {
                result = await _context.SurveyItems
                .Where(x => x.SurveyId == surveyId)
                .Select(x => new SurveyItemReportTemplate
                {
                    Title = x.Title,
                    Reportes = x.Questions
                        .Select(y => new QuestionReportTemplate
                        {
                            Id = y.Id,
                            Name = y.Description,
                            Type = y.Type,
                            Alternatives = y.Answers
                                .Select(z => new AlternativeTemplate
                                {
                                    Description = z.Description,
                                    Count = z.AnswerByUsers
                                        .Where(au => au.Question.Type != ConstantHelpers.SURVEY.TEXT_QUESTION)
                                        .GroupBy(au => au.AnswerId)
                                        .Select(au => au.Count()).FirstOrDefault()
                                }).ToList()
                        }).ToList()
                })
                .ToListAsync();
            }

            for (int i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < result[i].Reportes.Count; j++)
                {
                    if (result[i].Reportes[j].Type == ConstantHelpers.SURVEY.LIKERT_QUESTION)
                    {
                        result[i].Reportes[j].Alternatives = new List<AlternativeTemplate>();

                        foreach (var item in ConstantHelpers.SURVEY_LIKERT.RATING_SCALE.LIKERT)
                        {
                            var alternative = new AlternativeTemplate
                            {
                                Description = item.Value,
                                Count = await _context.AnswerByUsers
                                .Where(x => x.QuestionId == result[i].Reportes[j].Id && x.Description == item.Value)
                                .CountAsync()
                            };

                            result[i].Reportes[j].Alternatives.Add(alternative);
                        }
                    }
                }
            }

            return result;
        }

        public async Task<bool> HasQuestions(Guid id)
        {
            var hasQuestions = await _context.Question.AnyAsync(x => x.SurveyItemId == id);
            return hasQuestions;
        }

        public async Task<IEnumerable<SurveyItem>> GetAllBySurvey(Guid surveyId)
        {
            var query = _context.SurveyItems
                .Include(x => x.Survey)
                    .ThenInclude(x => x.SurveyUsers)
                .Include(x => x.Questions)
                    .ThenInclude(x => x.Answers)
                .Include(x => x.Questions)
                    .ThenInclude(x => x.AnswerByUsers)
                        .ThenInclude(x => x.Answer)
                .Where(x => x.SurveyId == surveyId);

            return await query.ToListAsync();
        }


        public async Task<IEnumerable<SurveyItem>> GetAllBySurveryAndQuestionType(Guid surveyId, int questionType)
        {
            var query = _context.SurveyItems
                .Where(x => x.Questions.Any(y => y.Type == questionType))
                .Include(x => x.Survey)
                .ThenInclude(x => x.SurveyUsers)
                .Where(x => x.Survey.SurveyUsers.Any(y => y.DateTime.HasValue))
                .Include(x => x.Questions)
                .Include(x => x.Questions)
                .ThenInclude(x => x.AnswerByUsers)
                .ThenInclude(x => x.Answer)
                .Where(x => x.SurveyId == surveyId);

            return await query.ToListAsync();
        }

        public async Task<List<SurveyItem>> GetSurveyItemsToImport(Guid surveyId)
        {
            var query = _context.SurveyItems
                .Include(x => x.Questions)
                    .ThenInclude(x => x.Answers)
                .Where(x => x.SurveyId == surveyId);

            return await query.ToListAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        #endregion

    }
}
