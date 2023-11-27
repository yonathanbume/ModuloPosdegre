using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AnswerByUser;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey;
using ClosedXML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class AnswerByUserRepository : Repository<AnswerByUser>, IAnswerByUserRepository
    {
        public AnswerByUserRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE
        private async Task<DataTablesStructs.ReturnedData<object>> GetGenericAnswerByUserDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, Expression<Func<SurveyUser, object>> selectPredicate = null)
        {
            Expression<Func<SurveyUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.Email);
                    break;
            }

            var query = _context.SurveyUsers
                .Where(x => x.SurveyId == surveyId)
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .AsNoTracking();

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }
        private async Task<DataTablesStructs.ReturnedData<object>> ReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? surveyId, string searchValue, Expression<Func<SurveyUser, object>> selectPredicate = null)
        {
            Expression<Func<SurveyUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.User.Name);
                    break;
            }

            var query = _context.SurveyUsers
               .Include(x=>x.User)
               .Where(x => x.SurveyId == surveyId)
               .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
               .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x => x.User.FullName.Trim().ToLower().Contains(searchValue));
            }

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }
        #endregion

        #region PUBLIC
        public async Task<DataTablesStructs.ReturnedData<object>> AnsweredSurveyUserDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId)
        {
            return await GetGenericAnswerByUserDatatable(sentParameters, surveyId, ExpressionHelpers.AnswerByUserSurvey());
        }

        public async Task<DataTablesStructs.ReturnedData<object>> ReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? surveyId, string search)
        {
            return await ReportDatatable(sentParameters, surveyId, search, (x) => new AnswerByUserReportTemplate
            {
                student = x.User.FullName,
                date = x.DateTime == null ? "-":x.DateTime.Value.ToString("dd/MM/yyyy"),
                id = x.Id,
            });
        }

        public async Task<bool> WasSurveyAnswered(Guid surveyUserId)
        {
            return await _context.AnswerByUsers.AnyAsync(x => x.SurveyUserId == surveyUserId);
        }
        #endregion

        #region REPORT SURVEY DATATABLE
        public async Task<IEnumerable<AnswerByUser>> GetAnswerByUserBySurveyId(Guid eid)
        {
            return await _context.AnswerByUsers.Where(x => x.SurveyUser.SurveyId == eid).ToListAsync();
        }
        #endregion

        #region ANSWERS BY USERS DATA TABLE
        public async Task<DataTablesStructs.ReturnedData<object>> GetAnswerByUserByQuestionIdDataTable(DataTablesStructs.SentParameters parameters, Guid questionId)
        {
            return await GetAnswerByUserByQuestionIdDataTable(parameters, questionId);
        }
        #endregion

        public async Task<IEnumerable<AnswerByUserTemplate>> GetAnswerByUserByQuestionId(Guid qid)
        {
            return await _context.AnswerByUsers
                         .Join(_context.Answer, au => au.AnswerId, a => a.Id, (au, a) => new { au.AnswerId, au.QuestionId, au.SurveyUserId, a.Description })
                         .Where(x => x.QuestionId == qid)
                         .GroupBy(x => new { x.AnswerId, x.Description })
                         .Select(x => new AnswerByUserTemplate
                         {
                             answerid = x.Key.Description,
                             countbyanswerid = x.Count()
                         }).ToArrayAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAnswersFromTextQuesitonDataTable(DataTablesStructs.SentParameters sentParameters, Guid questionId, bool? graduated = null)
        {
            Expression<Func<AnswerByUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Description);
                    break;
                default:
                    orderByPredicate = ((x) => x.Description);
                    break;
            }

            var query = _context.AnswerByUsers
               .Where(x => x.QuestionId == questionId).AsNoTracking();

            if (graduated != null)
                query = query.Where(x => x.SurveyUser.IsGraduated == graduated);

            var recordsFiltered = await query.CountAsync();

            var data = await query
               .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
               .Select(x => new
               {
                   answer = x.Description
               })
               .Skip(sentParameters.PagingFirstRecord)
               .Take(sentParameters.RecordsPerDraw)
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

        public async Task<List<SurveyUserReportTemplate>> GetUserAnswersBySurvey(Guid surveyId)
        {
            var surveyAnswerByUsers = await _context.SurveyUsers
                .AsNoTracking()
                .Where(x => x.SurveyId == surveyId && x.DateTime != null)
                .Select(x => new SurveyUserReportTemplate
                {
                    UserName = x.User.UserName,
                    FullName = x.User.FullName,
                    Role = x.User.Students.Any(y => y.UserId == x.UserId) ? "Alumno" : x.User.Teachers.Any(y => y.UserId == x.UserId) ? "Docente" : "Ni Docente - Ni Alumno",
                    CurrentAcademicYear = x.User.Students.Select(y => y.CurrentAcademicYear).FirstOrDefault(),
                    Dni = x.User.Dni,
                    Career = x.User.Students.Select(y => y.Career.Name).FirstOrDefault(),
                    AnswersQuestions = x.AnswerByUsers                        
                        .Where(y => y.SurveyUserId == x.Id)                        
                        .Select(y => new SurveyAnswerReportTemplate
                        {
                            QuestionId = y.QuestionId,
                            Question = y.Question.Description,
                            Answer = (y.Question.Type == ConstantHelpers.SURVEY.TEXT_QUESTION || y.Question.Type == ConstantHelpers.SURVEY.LIKERT_QUESTION) ? y.Description : y.Answer.Description
                        }).ToList()
                }).ToListAsync();

            return surveyAnswerByUsers;
        }
    }
}
