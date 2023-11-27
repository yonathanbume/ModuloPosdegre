using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class SurveyRepository : Repository<Survey>, ISurveyRepository
    {
        public SurveyRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Func<Survey, string[]> GetSurveysDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Name + "",
                x.Code + ""
            };
        }

        private async Task<DataTablesStructs.ReturnedData<object>> GetSurveysDatatable(DataTablesStructs.SentParameters sentParameters,int type, int system, Guid? careerId = null, Expression<Func<Survey,object>> selectPredicate = null, Expression<Func<Survey, dynamic>> orderByPredicate = null, Func<Survey, string[]> searchValuePredicate = null, string startSearchDate = null, string endSearchDate = null, string searchValue = null, ClaimsPrincipal user = null)
        {
            var query = _context.Survey
                .WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            query = query.Where(x => x.Type == type && x.System == system);

            if (user != null) 
            {
                if (user.IsInRole(ConstantHelpers.ROLES.JOBEXCHANGE_COORDINATOR))
                { 
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    query = query.Where(x => x.Career.CoordinatorCareers.Any(y => y.UserId == userId));
                }
            } 


            if (careerId != null && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(startSearchDate))
            {
                var startDate = ConvertHelpers.DatepickerToDatetime(startSearchDate);
                query = query.Where(x => x.PublicationDate.AddHours(-5).Date >= startDate.Date);
            }

            if (!string.IsNullOrEmpty(endSearchDate))
            {
                var endDate = ConvertHelpers.DatepickerToDatetime(endSearchDate);
                query = query.Where(x => x.PublicationDate.AddHours(-5).Date <= endDate.Date);
            }

            return await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .ToDataTables2(sentParameters, selectPredicate);
        }
        private async Task<Select2Structs.ResponseParameters> GetSurveyByIdSelect2(Select2Structs.RequestParameters requestParameters, Guid surveyId, Expression<Func<Survey, Select2Structs.Result>> selectPredicate = null, Func<Survey, string[]> searchValuePredicate = null)
        {
            var query = _context.Survey
                .Where(x => x.Id == surveyId).AsQueryable();

            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;
            var results = await query
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(selectPredicate)
                .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }
        private async Task<DataTablesStructs.ReturnedData<object>> GetGenericSurveyDatatable(DataTablesStructs.SentParameters sentParameters, int type, int system, Expression<Func<Survey, object>> selectPredicate = null, string searchValue = null)
        {
            Expression<Func<Survey, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.State);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.PublicationDate);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.FinishDate);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            string[] searchValuePredicate(Survey x) => new[]
            {
                x.Name + "",
                x.Code + "",
                x.Description + ""
            };

            var query = _context.Survey
                .Where(x => x.Type == type && x.System == system)
                .WhereSearchValue(searchValuePredicate, searchValue)
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }
        #endregion

        #region PUBLIC


        public async Task<bool> AnySurveyByName(int type, int system, string name , Guid? id = null)
        {
            var query = _context.Survey
                .Where(x => x.Type == type && x.System == system)
                .AsQueryable();

            if (id!= null)
            {
                var result = await query.AnyAsync(x => x.Name.Trim().ToUpper() == name.Trim().ToUpper() && id != x.Id);
                return result;
            }
            else
            {
                var result = await query.AnyAsync(x => x.Name.Trim().ToUpper() == name.Trim().ToUpper());
                return result;
            }
        }

        public async Task<bool> AnySurvey(int type, string name)
        {
            var query = _context.Survey.Where(x => x.Type == type && x.Name == name);
            return await query.AnyAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetGeneralSurveysDatatable(DataTablesStructs.SentParameters sentParameters, int system, Guid? careerId = null, string startSearchDate = null, string endSearchDate = null, string searchValue = null, ClaimsPrincipal user = null)
        {
            var type = ConstantHelpers.TYPE_SURVEY.GENERAL;
            Expression<Func<Survey, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code); break;
                case "1":
                    orderByPredicate = ((x) => x.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.Career.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.SurveyUsers.Where(y => y.DateTime != null).Count()); break;
                case "4":
                    orderByPredicate = ((x) => x.State); break;
                case "5":
                    orderByPredicate = ((x) => x.PublicationDate); break;
                case "6":
                    orderByPredicate = ((x) => x.FinishDate); break;
            }

            var query = _context.Survey.AsNoTracking();

            query = query.Where(x => x.Type == type && x.System == system);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.JOBEXCHANGE_COORDINATOR))
                {
                    query = query.Where(x => x.Career.CoordinatorCareers.Any(y => y.UserId == userId));
                }
                else
                {
                    if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                        user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) ||
                        user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                        user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                    {
                        query = query.Where(x =>
                        x.Career.AcademicCoordinatorId == userId ||
                        x.Career.CareerDirectorId == userId ||
                        x.Career.AcademicDepartmentDirectorId == userId ||
                        x.Career.AcademicSecretaryId == userId);
                    }

                    if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                    {
                        if (!string.IsNullOrEmpty(userId))
                        {
                            query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                        }
                    }
                }
            }


            if (careerId != null && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(startSearchDate))
            {
                var startDate = ConvertHelpers.DatepickerToDatetime(startSearchDate);
                query = query.Where(x => x.PublicationDate.AddHours(-5).Date >= startDate.Date);
            }

            if (!string.IsNullOrEmpty(endSearchDate))
            {
                var endDate = ConvertHelpers.DatepickerToDatetime(endSearchDate);
                query = query.Where(x => x.PublicationDate.AddHours(-5).Date <= endDate.Date);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Code.ToUpper().Contains(searchValue.ToUpper()) ||
                                    x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }


            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    Title = x.Name,
                    Career = x.CareerId == null ? "Sin Asignar" : x.Career.Name,
                    Answers = x.SurveyUsers.Where(y => y.DateTime != null).Count(),
                    StatusId = x.State,
                    Status = ConstantHelpers.SURVEY_STATES.VALUES.ContainsKey(x.State) == false
                        ? "Desconocido"
                        : ConstantHelpers.SURVEY_STATES.VALUES[x.State],
                    PublishDate = x.PublicationDate.ToLocalTime().ToString("dd/MM/yyyy"),
                    FinishDate = x.FinishDate.ToLocalTime().ToString("dd/MM/yyyy")
                })
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetIntranetGeneralSurveysDatatable(DataTablesStructs.SentParameters sentParameters, int system, Guid? careerId = null, string startSearchDate = null, string endSearchDate = null, string searchValue = null, ClaimsPrincipal user = null)
        {
            var type = ConstantHelpers.TYPE_SURVEY.GENERAL;
            Expression<Func<Survey, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code); break;
                case "1":
                    orderByPredicate = ((x) => x.State); break;
                case "2":
                    orderByPredicate = ((x) => x.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.PublicationDate); break;
                case "4":
                    orderByPredicate = ((x) => x.FinishDate); break;
            }

            var query = _context.Survey.AsNoTracking();

            query = query.Where(x => x.Type == type && x.System == system);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.JOBEXCHANGE_COORDINATOR))
                {
                    query = query.Where(x => x.Career.CoordinatorCareers.Any(y => y.UserId == userId));
                }
                else
                {
                    if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                        user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) ||
                        user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                        user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                    {
                        query = query.Where(x =>
                        x.Career.AcademicCoordinatorId == userId ||
                        x.Career.CareerDirectorId == userId ||
                        x.Career.AcademicDepartmentDirectorId == userId ||
                        x.Career.AcademicSecretaryId == userId);
                    }

                    if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                    {
                        if (!string.IsNullOrEmpty(userId))
                        {
                            query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                        }
                    }
                }
            }


            if (careerId != null && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(startSearchDate))
            {
                var startDate = ConvertHelpers.DatepickerToDatetime(startSearchDate);
                query = query.Where(x => x.PublicationDate.AddHours(-5).Date >= startDate.Date);
            }

            if (!string.IsNullOrEmpty(endSearchDate))
            {
                var endDate = ConvertHelpers.DatepickerToDatetime(endSearchDate);
                query = query.Where(x => x.PublicationDate.AddHours(-5).Date <= endDate.Date);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Code.ToUpper().Contains(searchValue.ToUpper()) ||
                                    x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }


            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    Title = x.Name,
                    Career = x.CareerId == null ? "Sin Asignar" : x.Career.Name,
                    Answers = x.SurveyUsers.Where(y => y.DateTime != null).Count(),
                    StatusId = x.State,
                    Status = ConstantHelpers.SURVEY_STATES.VALUES.ContainsKey(x.State) == false
                        ? "Desconocido"
                        : ConstantHelpers.SURVEY_STATES.VALUES[x.State],
                    PublishDate = x.PublicationDate.ToLocalTime().ToString("dd/MM/yyyy"),
                    FinishDate = x.FinishDate.ToLocalTime().ToString("dd/MM/yyyy")
                })
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeSurveysDatatable(DataTablesStructs.SentParameters sentParameters,int system, Guid? careerId = null, string startSearchDate = null, string endSearchDate = null, string searchValue = null, ClaimsPrincipal user = null)
        {
            var type = ConstantHelpers.TYPE_SURVEY.BOSS_REPORT;
            Expression<Func<Survey, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code); break;
                case "1":
                    orderByPredicate = ((x) => x.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.Career.Name); break;
            }

            var query = _context.Survey.AsNoTracking();

            query = query.Where(x => x.Type == type && x.System == system);

            if (user != null)
            {
                if (user.IsInRole(ConstantHelpers.ROLES.JOBEXCHANGE_COORDINATOR))
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    query = query.Where(x => x.Career.CoordinatorCareers.Any(y => y.UserId == userId));
                }
            }


            if (careerId != null && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(startSearchDate))
            {
                var startDate = ConvertHelpers.DatepickerToDatetime(startSearchDate);
                query = query.Where(x => x.PublicationDate.AddHours(-5).Date >= startDate.Date);
            }

            if (!string.IsNullOrEmpty(endSearchDate))
            {
                var endDate = ConvertHelpers.DatepickerToDatetime(endSearchDate);
                query = query.Where(x => x.PublicationDate.AddHours(-5).Date <= endDate.Date);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Code.ToUpper().Contains(searchValue.ToUpper()) ||
                                    x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    Title = x.Name,
                    x.Code,
                    Career = x.Career == null ? "" : x.Career.Name,
                    HasQuestions = x.SurveyItems.Where(y => y.Questions.Any(z => z.SurveyItemId == y.Id)).Count() == 0 ? false : true,
                    StatusId = x.State,
                    Status = ConstantHelpers.SURVEY_STATES.VALUES.ContainsKey(x.State) == false
                        ? "Desconocido"
                        : ConstantHelpers.SURVEY_STATES.VALUES[x.State]
                })
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

        public async Task<SurveyDetailTemplate> GetSurveyDetail(Guid surveyId)
        {
            var query = _context.Survey
                .Where(x => x.Id == surveyId)
                .Select(x => new SurveyDetailTemplate
                {
                    Id = x.Id,
                    Name = x.Name,
                    CareerId = x.CareerId,
                    Description = x.Description,
                    Code = x.Code,
                    PublicationDate = x.PublicationDate.ToString("dd/MM/yyyy"),
                    FinishDate = x.FinishDate.ToString("dd/MM/yyyy"),
                    IsAnonymous = x.IsAnonymous,
                    IsRequired = x.IsRequired
                });

            return await query.FirstOrDefaultAsync();
        }

        public override async Task DeleteById(Guid id)
        {
            var survey = await _context.Survey
                    .Include(x => x.SurveyItems)
                        .ThenInclude(x => x.Questions)
                            .ThenInclude(x => x.Answers)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

            foreach (var surveyItem in survey.SurveyItems)
            {
                foreach (var question in surveyItem.Questions)
                {
                    //Por cada pregunta eliminar sus respuestas
                    _context.Answer.RemoveRange(question.Answers);
                }
                //por cada seccion eliminar sus preguntas
                _context.Question.RemoveRange(surveyItem.Questions);
            }
            _context.SurveyItems.RemoveRange(survey.SurveyItems);
            await _context.SaveChangesAsync();
            await base.DeleteById(id);
        }

        public async Task<bool> ValidateSurvey(Guid id)
        {
            var query = _context.Question.Where(x => x.SurveyItem.SurveyId == id);

            return await query.AnyAsync();
        }        

        public async Task<Guid> InsertAndReturnId(Survey survey)
        {
            await _context.Survey.AddAsync(survey);
            return survey.Id;
        }

        public async Task<IEnumerable<Survey>> GetSurveisInInterestGroupByUserId(string id)
        {
            return await _context.Survey
               .Where(x => x.Type == ConstantHelpers.TYPE_SURVEY.GENERAL &&
                        x.Id == _context.InterestGroupSurveys.FirstOrDefault(y => y.SurveyId == x.Id && y.InterestGroup.UserAdminId == id).SurveyId)
                .Include(x => x.Career)
                .ThenInclude(x => x.InterestGroups).ToArrayAsync();
        }

        //public async Task<DataTablesStructs.ReturnedData<object>> GetTeachingSatisfactionSurveyDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        //{
        //    var type = ConstantHelpers.TYPE_SURVEY.TEACHER_SATISFACTION;
        //    var system = ConstantHelpers.Solution.TeachingManagement;
        //    return await GetGenericSurveyDatatable(sentParameters, type, system, ExpressionHelpers.GenericSurvey(),searchValue);
        //}

        public async Task<DataTablesStructs.ReturnedData<object>> GetGeneralTeachingSurveyDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            var type = ConstantHelpers.TYPE_SURVEY.GENERAL;
            var system = ConstantHelpers.Solution.TeachingManagement;
            return await GetGenericSurveyDatatable(sentParameters, type, system, ExpressionHelpers.GenericSurvey(), searchValue);
        }

        public async Task<Survey> GetWithIncludes(Guid id)
        {
            var query = _context.Survey
                    .Include(x => x.Career)
                    .Include(x => x.SurveyUsers)
                    .Include(x => x.SurveyItems)
                    .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportGeneralTeachingSurveyDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<Survey, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Code);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            string[] searchValuePredicate(Survey x) => new[]
            {
                x.Name + "",
                x.Code + ""
            };

            var query = _context.Survey
                    .Where(x => x.Type == ConstantHelpers.TYPE_SURVEY.GENERAL
                    && x.System == ConstantHelpers.Solution.TeachingManagement)
                    .WhereSearchValue(searchValuePredicate, searchValue)
                    .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                    .AsNoTracking();

            Expression<Func<Survey, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                id = x.Id,
                code = x.Code,
                name = x.Name,
                publishDate = x.PublicationDate.ToString("dd-MM-yyyy"),
                surveyuserCount = x.SurveyUsers.Count
            };

            return await query.ToDataTables2(sentParameters , selectPredicate);
        }

        public async Task<IEnumerable<SurveyTemplateA>> GetReportData()
        {
            var query = await _context.Survey
                    .Where(x => x.Type == ConstantHelpers.TYPE_SURVEY.GENERAL
                    && x.System == ConstantHelpers.Solution.TeachingManagement)
                    .Select(x => new SurveyTemplateA
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        PublishDate = x.PublicationDate.ToString("dd-MM-yyyy"),
                        SurveyuserCount = x.SurveyUsers.Count
                    }).ToListAsync();

            return query;
        }

        public async Task<Select2Structs.ResponseParameters> GetSurveyByIdSelect2(Select2Structs.RequestParameters requestParameters, Guid eid)
        {
            return await GetSurveyByIdSelect2(requestParameters, eid, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name });
        }

        public async Task<bool> ExistSurveyCode(int type,int system,string code,Guid? surveyId)
        {
            var query = _context.Survey
                .Where(x => x.Type == type && x.System == system)
                .AsQueryable();

            if (surveyId.HasValue)
            {
                var result = await query.AnyAsync(x => x.Code.Trim().ToUpper() == code.Trim().ToUpper() && surveyId != x.Id);
                return result;
            }
            else
            {
                var result = await query.AnyAsync(x => x.Code.Trim().ToUpper() == code.Trim().ToUpper());
                return result;
            }
        }

        public async Task<object> GetAllAsSelect2CliendSide(Guid sectionId)
        {
            var result = await _context.Survey
                        .Where(x => x.SurveyUsers.Any(y => y.SectionId == sectionId))
                        .Select(x => new
                        {
                            id = x.Id,
                            text = x.Name
                        }).ToListAsync();

            return result;
        }

        public async Task<List<SurveyReportTemplate>> GetSurveyReportExcel(Guid id)
        {
            ////Para estudiantes y empresas...
            //Filtramos encuestas usuario por encuestas
            var surveyUsers = _context.SurveyUsers
                .Where(x => x.SurveyId == id)
                .AsQueryable();

            //Encuestas usuario respondidas
            var answered = surveyUsers.Where(x => x.AnswerByUsers.Count() > 0);

            var query = _context.Students.Where(x => surveyUsers.Any(y => y.UserId == x.UserId)).AsQueryable();

            var studentFaculties = query
                .Select(x => x.Career.FacultyId)
                .Distinct();

            var facultyReport = await _context.Faculties
                .Where(x => studentFaculties.Any(y => y == x.Id))
                .Select(x => new SurveyReportTemplate
                {
                    Faculty = x.Name,
                    Sended = query.Where(y => y.Career.FacultyId == x.Id).Count(),
                    Answered = query.Where(y => y.Career.FacultyId == x.Id && answered.Any(z => z.UserId == y.UserId)).Count()
                }).ToListAsync();

            return facultyReport;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportSurveyDatatable(DataTablesStructs.SentParameters sentParameters, int system, int type, string searchValue = null)
        {
            Expression<Func<Survey, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Code);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            var query = _context.Survey.
                Where(x => x.Type == type && x.System == system)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Code.ToUpper().Contains(searchValue.ToUpper())
                                    || x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,
                    isAnonymous = x.IsAnonymous,
                    publishDate = x.PublicationDate.ToString("dd-MM-yyyy"),
                    surveyuserCount = x.SurveyUsers.Count(),
                    surveyAnswered = x.SurveyUsers.Where(y => y.DateTime != null).Count()
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetIntranetReportSurveyDatatable(DataTablesStructs.SentParameters sentParameters, int system, int type, string searchValue = null)
        {
            Expression<Func<Survey, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.PublicationDate;
                    break;
                case "3":
                    orderByPredicate = (x) => x.SurveyUsers.Count();
                    break;
                case "4":
                    orderByPredicate = (x) => x.SurveyUsers.Where(y => y.DateTime != null).Count();
                    break;
            }

            var query = _context.Survey.
                Where(x => x.Type == type && x.System == system)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Code.ToUpper().Contains(searchValue.ToUpper())
                                    || x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,
                    isAnonymous = x.IsAnonymous,
                    publishDate = x.PublicationDate.ToString("dd-MM-yyyy"),
                    surveyuserCount = x.SurveyUsers.Count(),
                    surveyAnswered = x.SurveyUsers.Where(y => y.DateTime != null).Count()
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

        public async Task<IEnumerable<SurveyTemplateA>> GetReportBySystemAndTypeData(int system, int type)
        {
            var query = await _context.Survey
                               .Where(x => x.Type == type
                               && x.System == system)
                               .Select(x => new SurveyTemplateA
                               {
                                   Id = x.Id,
                                   Code = x.Code,
                                   Name = x.Name,
                                   PublishDate = x.PublicationDate.ToString("dd-MM-yyyy"),
                                   SurveyuserCount = x.SurveyUsers.Count
                               }).ToListAsync();

            return query;
        }

        public async Task<object> GetSurveyByCareerIdSelect2ClientSide(Guid careerId,int? surveyType = null, int? system = null, int? year = null)
        {
            var query = _context.Survey.Where(x => x.CareerId == careerId).AsQueryable();

            if (year != null)
                query = query.Where(x => x.CreatedDate.Year == year);

            if (surveyType != null)
                query = query.Where(x => x.Type == surveyType);

            if (system != null)
                query = query.Where(x => x.System == system);

            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .ToListAsync();

            return result;
        }

        public async Task<List<QuestionExcelTemplate>> GetQuestions(Guid surveyId)
        {
            var result = await _context.Question
                .Where(x => x.SurveyItem.SurveyId == surveyId)
                .Select(x => new QuestionExcelTemplate
                {
                    QuestionId = x.Id,
                    Question = x.Description,
                    Type = x.Type
                }).ToListAsync();

            return result;
        }

        public async Task<object> GetPreEnrollmentSurveySelect2ClientSide()
        {
            var today = DateTime.UtcNow.ToDefaultTimeZone().Date;
            var result = await _context.Survey.Where(x => x.State == ConstantHelpers.SURVEY_STATES.SENT && x.FinishDate.Date >= today)
                .Select(x => new
                {
                    x.Id,
                    text = x.Name,
                })
                .ToListAsync();

            return result;
        }

        public async Task<decimal> GetSurveyProgressPercentage(Guid surveyId)
        {
            var totalSurveySend = await _context.SurveyUsers.Where(x => x.SurveyId == surveyId).CountAsync();

            var answeredSurvey = await _context.SurveyUsers.Where(x => x.SurveyId == surveyId && x.DateTime != null).CountAsync();

            if (totalSurveySend <= 0)
            {
                return 0.0M;
            }
            else
            {
                return (answeredSurvey * 100.0M) / (totalSurveySend * 1.0M);
            }
        }

        public async Task<Survey> GetByCode(string code, int system)
        {
            var result = await _context.Survey.Where(x => x.Code.ToUpper() == code.ToUpper() && x.System == system).FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> HasSurveyItems(Guid surveyId)
        {
            var result = await _context.SurveyItems
                .AnyAsync(x => x.SurveyId == surveyId);

            return result;
        }

        #endregion
    }
}
