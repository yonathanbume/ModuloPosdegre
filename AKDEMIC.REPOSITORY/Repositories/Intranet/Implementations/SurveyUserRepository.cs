using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class SurveyUserRepository : Repository<SurveyUser>, ISurveyUserRepository
    {
        public SurveyUserRepository(AkdemicContext context) : base(context) { }

        #region Private
        private async Task<DataTablesStructs.ReturnedData<object>> GetGenericSurveyUserDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, Expression<Func<SurveyUser, object>> selectPredicate = null, string searchValue = null)
        {
            Expression<Func<SurveyUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.SurveyId);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            var query = _context.SurveyUsers
                .Where(x => x.SurveyId == surveyId)
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            //Filtrar por aquellas que si fueron respondidas
            var answerByUsers = _context.AnswerByUsers.AsQueryable();

            query = query.Where(x => answerByUsers.Any(y => y.SurveyUserId == x.Id));

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                                    || x.User.Dni.Contains(searchValue)
                                    || x.User.UserName.ToUpper().Contains(searchValue.ToUpper()));
            }

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }
        #endregion

        public async Task<IEnumerable<SurveyUser>> GetAllFirstLevelByUser(string userId)
        {
            var query = _context.SurveyUsers
                    .Include(x => x.Survey)
                    .Include(x => x.Section)
                    .Include(x => x.User)
                    .Include(x => x.AnswerByUsers)
                    .Where(x => x.UserId == userId);

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSurveyUserDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, string searchValue = null)
        {
            return await GetGenericSurveyUserDatatable(sentParameters, surveyId, ExpressionHelpers.SurveyUserAnswer(), searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetBySurveyIdAnswersDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, string searchValue = null)
        {
            Expression<Func<SurveyUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.SurveyId);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            var query = _context.SurveyUsers
                .Where(x => x.SurveyId == surveyId)
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.User.FullName.ToUpper().Contains(searchValue.ToUpper()));
            }

            Expression<Func<SurveyUser, object>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                x.Id,
                User = x.User.FullName,
                x.User.Email,
                Survey = x.Survey.Name,
                FinishDate = x.Survey.FinishDate.ToString("dd-MM-yyyy")
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<SurveyUser> GetIncludeFirstLevel(Guid id)
        {
            var query = _context.SurveyUsers
                .Include(x => x.Survey)
                .Include(x => x.Section)
                .Include(x => x.User)
                .Include(x => x.AnswerByUsers)
                .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SurveyUser>> GetInterestGroupUserSurveisByUserId(string userId, Guid? academicProgramId = null)
        {
            //var result = _context.SurveyUsers
            //    .Include(x => x.Survey)
            //    .Where(x => x.UserId == userId && x.Survey.PublicationDate.Date <= DateTime.Now.Date && x.Survey.FinishDate.Date >= DateTime.Now.Date 
            //    && !(_context.AnswerByUsers.Include(y => y.SurveyUser.User).Include(y => y.SurveyUser.Survey)
            //    .Any(y => y.SurveyUser.UserId == x.UserId && y.SurveyUser.SurveyId == x.SurveyId)));

            var result = await _context.SurveyUsers
                .Include(x => x.Survey.Career)
                .Where(x => x.UserId == userId &&
                    x.Survey.PublicationDate.Date <= DateTime.Now &&
                    x.Survey.FinishDate.Date >= DateTime.Now.Date &&
                    !x.AnswerByUsers.Any(y => y.SurveyUser.UserId == x.UserId &&
                                              y.SurveyUser.SurveyId == x.SurveyId)
                ).OrderByDescending(x => x.CreatedAt).ToListAsync();

            if (academicProgramId.HasValue)
            {
                result = result
                    .Where(x =>
                        x.Survey.Career.AcademicPrograms
                            .Any(z => z.Id == academicProgramId.Value)
                    ).ToList();
            }

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSurveysByUserDatatable(DataTablesStructs.SentParameters sentParameters, string userId, DateTime? PublicationDate, DateTime? FinishDate,int type = 0, int system = 0)
        {
            Expression<Func<SurveyUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Survey.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Survey.PublicationDate);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Survey.FinishDate);
                    break;
            }


            var query = _context.SurveyUsers
                    .Where(x => x.UserId == userId
                    && x.Survey.Type == type
                    && x.Survey.System == system)
                    .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                    .AsNoTracking();

            if (PublicationDate.HasValue)
                query = query.Where(x => x.Survey.PublicationDate <= PublicationDate.Value);

            if (FinishDate.HasValue)
                query = query.Where(x => x.Survey.FinishDate >= FinishDate.Value);

            Expression<Func<SurveyUser, object>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                x.Id,
                title = x.Survey.Name,
                publishDate = x.Survey.PublicationDate.ToLocalDateFormat(),
                finishDate = x.Survey.FinishDate.ToLocalDateFormat(),
                isExpired = x.Survey.FinishDate < DateTime.UtcNow,
                isAnswered = x.DateTime != null,
                answerDate = x.DateTime == null ? "-" : x.DateTime.Value.ToLocalDateFormat()
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<IEnumerable<SurveyUser>> GetSurveyUsersBySurveyId(Guid surveyId)
        {
            return await _context.SurveyUsers
                          .Include(x => x.User)
                          .Where(x => x.SurveyId == surveyId).ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUsersInGeneralSurveyDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, string searchValue = null,bool? answered = null)
        {
            //Expression<Func<SurveyUser, dynamic>> orderByPredicate = null;

            //switch (sentParameters.OrderColumn)
            //{
            //    case "0":
            //        orderByPredicate = ((x) => x.Id);
            //        break;
            //    case "1":
            //        orderByPredicate = ((x) => x.SurveyId);
            //        break;
            //    case "2":
            //        orderByPredicate = ((x) => x.UserId);
            //        break;
            //    default:
            //        orderByPredicate = ((x) => x.Id);
            //        break;
            //}

            //var query = _context.SurveyUsers
            //    .Where(x => x.SurveyId == surveyId)
            //    .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
            //    .AsQueryable();

            //Expression<Func<SurveyUser, dynamic>> selectPredicate = null;
            //selectPredicate = (x) => new
            //{
            //    User = x.User.FullName,
            //    Answered = x.AnswerByUsers.Count == 0 ? "Pendiente" : "Completada"
            //};

            //return await query.ToDataTables2(sentParameters, selectPredicate);

            Expression<Func<SurveyUser, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
            }

            var query = _context.SurveyUsers
                .Where(x => x.SurveyId == surveyId)
                .AsNoTracking();

            if (answered.HasValue && answered.Value)
            {
                query = query.Where(x => x.AnswerByUsers.Count > 0);
            }else if (answered.HasValue && !answered.Value)
            {
                query = query.Where(x => x.AnswerByUsers.Count == 0);
            }

            if (!String.IsNullOrEmpty(searchValue))
            {
                string search = searchValue.ToUpper().Trim();
                query = query.Where(x => x.User.Name.ToUpper().Contains(search)
                                || x.User.PaternalSurname.ToUpper().Contains(search)
                                || x.User.MaternalSurname.ToUpper().Contains(search)
                                || x.User.FullName.ToUpper().Contains(search));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    User = x.User.FullName,
                    Answered = x.AnswerByUsers.Count == 0 ? "Pendiente" : "Completada"
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<bool> IsSurveySendedToUsers(Guid id)
        {
            return await _context.SurveyUsers
                            .AnyAsync(x => x.SurveyId == id);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetUsersBySurveysDataTable(DataTablesStructs.SentParameters parameters, Guid surveyId)
        {
            return await GetUsersBySurveysDataTable(parameters, surveyId, null);
        }
        private async Task<DataTablesStructs.ReturnedData<object>> GetUsersBySurveysDataTable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, Expression<Func<object, object>> selectPredicate = null)
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

            var query = _context.SurveyUsers
                .Include(x => x.User)
                .Where(x => x.SurveyId == surveyId)
                .Select(x => new { fullname = x.User.FullName })
                .AsNoTracking();

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSurveyByCompanyJobExchangeDatatable(DataTablesStructs.SentParameters sentParameters, string userId,int surveyType = 0, string searchValue = null)
        {
            var system = ConstantHelpers.Solution.JobExchange;

            Expression<Func<SurveyUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Survey.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Survey.Type);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Survey.PublicationDate);
                    break;
                default:
                    orderByPredicate = ((x) => x.Survey.Id);
                    break;
            }

            var employeeSurveyQuery = _context.EmployeeSurveys
                    .Where(x => x.SurveyUser.UserId == userId
                    && x.SurveyUser.Survey.System == system).AsQueryable();


            var query = _context.SurveyUsers
                    .Where(x => x.UserId == userId && x.Survey.System == system)
                    .AsNoTracking();

            if(surveyType != 0)
            {
                query = query.Where(x => x.Survey.Type == surveyType);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Survey.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    Name = employeeSurveyQuery.Where(y => y.SurveyUserId == x.Id).Select(y => y.User.FullName).FirstOrDefault() ?? "Sistema Bolsa", 
                    Survey = x.Survey.Name,
                    Type = ConstantHelpers.TYPE_SURVEY.VALUES.ContainsKey(x.Survey.Type)
                    ? ConstantHelpers.TYPE_SURVEY.VALUES[x.Survey.Type]
                    : "Desconocido",
                    SurveyDate = x.Survey.PublicationDate.ToLocalDateFormat()
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
 
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersBySurveyId(Guid surveyId)
            => await _context.SurveyUsers.Where(x => x.SurveyId == surveyId && x.DateTime.HasValue).Select(x=>x.User).ToArrayAsync();

        public async Task<IEnumerable<SurveyUser>> GetLastPendingSurveyByUser(int take, string userId,int system)
        {
            //Encuestas pendientes del usuario
            var query = _context.SurveyUsers
                .Include(x => x.Survey)
                .Where(x => x.UserId == userId && x.DateTime == null && x.Survey.System == system)
                .AsQueryable();

            return await query.OrderByDescending(x => x.Survey.FinishDate).Take(take).ToListAsync();
        }

        public async Task<SurveyUser> GetFirstUserSurvey(bool isRequired, string userId)
        {
            var today = DateTime.UtcNow;

            var surveyUsers = await _context.SurveyUsers
                .Where(x => x.UserId == userId && x.Survey.IsRequired == isRequired
                && x.DateTime == null
                && (today >= x.Survey.PublicationDate && today <= x.Survey.FinishDate))
                .OrderBy(x => x.Survey.PublicationDate)
                .ToListAsync();

            return surveyUsers.FirstOrDefault();
        }

        public async Task<SurveyUserTemplate> GetSurveyUserTemplate(Guid id)
        {
            var result = await _context.SurveyUsers
                .Where(x => x.Id == id)
                .Select(x => new SurveyUserTemplate
                {
                    SurveyName = x.Survey.Name,
                    UserName = x.User.UserName,
                    PublicationDate = x.Survey.PublicationDate.ToLocalDateFormat(),
                    SurveyUserId = x.Id,
                    Solved = x.DateTime != null,
                    Expired = x.Survey.FinishDate < DateTime.UtcNow,
                    SurveyItems = x.Survey.SurveyItems
                        .Select(y => new SurveyItemTemplate 
                        { 
                            Title = y.Title,
                            IsLikert = y.IsLikert,
                            Questions = y.Questions
                                .Select(z => new QuestionsTemplate 
                                {
                                    Id = z.Id,
                                    Description = z.Description,
                                    Type = z.Type,
                                    Response = z.AnswerByUsers.Where(a => a.QuestionId == z.Id && a.SurveyUserId == id).Select(a => a.Description).FirstOrDefault(),
                                    Answers = z.Answers
                                        .Select(a => new AnswersTemplate
                                        {
                                            Id = a.Id,
                                            Description = a.Description,
                                            Selected = a.AnswerByUsers.Where(au => au.AnswerId == a.Id && au.SurveyUserId == id)
                                                .FirstOrDefault() == null ? false : true
                                        }).ToList()
                                }).ToList()
                        }).ToList()
                }).FirstOrDefaultAsync();

            return result;                     
        }

        public async Task<bool> AtLeastOneIsStudent(Guid surveyId)
        {
            return await _context.SurveyUsers.AnyAsync(x => x.SurveyId == surveyId && x.User.UserRoles.Any(y => y.Role.Name == ConstantHelpers.ROLES.STUDENTS));
        }
    }
}
