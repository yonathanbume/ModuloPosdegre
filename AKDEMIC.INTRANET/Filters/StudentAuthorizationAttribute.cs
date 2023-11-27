using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Helpers.ConstantHelpers;

namespace AKDEMIC.INTRANET.Filters
{
    public class StudentAuthorizationAttribute : TypeFilterAttribute
    {
        public StudentAuthorizationAttribute() : base(typeof(StudentAuthorizationAttributeImp)) { }

        private class StudentAuthorizationAttributeImp : IAsyncActionFilter
        {
            private readonly AkdemicContext _context;
            protected readonly string _userId;
            protected readonly ClaimsPrincipal _principal;
            public StudentAuthorizationAttributeImp(
                    AkdemicContext context,
                    IHttpContextAccessor httpContextAccessor,
                    UserManager<ApplicationUser> userManager
                )
            {
                _userId = userManager.GetUserId(httpContextAccessor.HttpContext.User);
                _principal = httpContextAccessor.HttpContext.User;
                _context = context;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context,
                                                   ActionExecutionDelegate next)
            {

                var anyPending = false;

                if (_principal.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                {
                    if (context.Controller.GetType() != typeof(AKDEMIC.INTRANET.Controllers.JsonController) &&
                        context.Controller.GetType() != typeof(AKDEMIC.INTRANET.Areas.Student.Controllers.PerformanceEvaluationController) &&
                        context.Controller.GetType() != typeof(AKDEMIC.INTRANET.Controllers.PasswordChangeController) &&
                        context.Controller.GetType() != typeof(AKDEMIC.INTRANET.Controllers.StudentInformationController))
                    {
                        if (!anyPending)
                        {
                            var configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.TeacherManagement.PERFORMANCE_EVALUATION_REQUIRED).FirstOrDefaultAsync();

                            if (configuration is null)
                            {
                                configuration = new ENTITIES.Models.Configuration
                                {
                                    Key = ConstantHelpers.Configuration.TeacherManagement.PERFORMANCE_EVALUATION_REQUIRED,
                                    Value = ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.TeacherManagement.PERFORMANCE_EVALUATION_REQUIRED]
                                };
                            }

                            var required = Convert.ToBoolean(configuration.Value);

                            if (required)
                            {
                                var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE && !x.IsSummer).FirstOrDefaultAsync();

                                if (term != null)
                                {
                                    var evaluations = await _context.PerformanceEvaluations.Where(x => x.TermId == term.Id).ToListAsync();

                                    if (evaluations.Any(x => x.StartDate.Date <= DateTime.UtcNow.ToDefaultTimeZone().Date && x.EndDate.Date >= DateTime.UtcNow.ToDefaultTimeZone().Date))
                                    {
                                        var evaluation = evaluations.Where(x => x.StartDate.Date <= DateTime.UtcNow.ToDefaultTimeZone().Date && x.EndDate.Date >= DateTime.UtcNow.ToDefaultTimeZone().Date).FirstOrDefault();

                                        var student = await _context.Students.Where(x => x.UserId == _userId).FirstOrDefaultAsync();

                                        var teacherRole = await _context.Roles.Where(x => x.Name == ConstantHelpers.ROLES.TEACHERS).Select(x => x.Id).FirstOrDefaultAsync();
                                        var userEvaluation = await _context.PerformanceEvaluationUsers.Where(x => x.FromUserId == student.UserId && x.PerformanceEvaluationId == evaluation.Id && x.FromRole.Name == ConstantHelpers.ROLES.STUDENTS).CountAsync();
                                        var sections = await _context.StudentSections.Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == term.Id && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN).Select(x => x.SectionId).ToListAsync();
                                        var teacherSection = await _context.TeacherSections.Where(x => sections.Contains(x.SectionId)).CountAsync();
                                        if (teacherSection > userEvaluation)
                                        {
                                            context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "PerformanceEvaluation" }, { "action", "Index" }, { "area", "Student" } });
                                            anyPending = true;
                                        }
                                    }
                                }
                            }
                        }
                        //|| context.Controller.GetType() != typeof(AKDEMIC.INTRANET.Controllers.JsonController)
                        if (!anyPending)
                        {
                            var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE)
                                .FirstOrDefaultAsync();

                            if (term != null)
                            {
                                var student = await _context.Students.Include(x => x.StudentInformation).Where(x => x.UserId == _userId)
                                .FirstOrDefaultAsync();

                                var studentInformationTerm = await _context.StudentInformations.Where(x => x.StudentId == student.Id && x.TermId == term.Id)
                                    .FirstOrDefaultAsync();

                                var hasStudentInformation = await _context.StudentInformations.AnyAsync(x => x.StudentId == student.Id && x.TermId != null);


                                var configStudentInformation = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.InstitutionalWelfareManagement.STUDENT_INFORMATION_VISIBILITY).FirstOrDefaultAsync();
                                var configStudentSurvey = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.InstitutionalWelfareManagement.INSTITUTIONAL_WELFARE_SURVEY_VISIBILITY).FirstOrDefaultAsync();
                                var configStudentInformationReiterative = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.InstitutionalWelfareManagement.STUDENT_INFORMATION_REITERATIVE).FirstOrDefaultAsync();
                                var configStudentInformationAllStudent = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.InstitutionalWelfareManagement.STUDENT_INFORMATION_ALLSTUDENT).FirstOrDefaultAsync();

                                var boolConfigStudentInformation = false;
                                var boolConfigStudentSurvey = false;
                                var boolConfigStudentInformationReiterative = false;
                                var boolconfigStudentInformationAllStudent = false;

                                if (configStudentInformation != null)
                                {
                                    boolConfigStudentInformation = bool.Parse(configStudentInformation.Value);
                                }

                                if (configStudentSurvey != null)
                                {
                                    boolConfigStudentSurvey = bool.Parse(configStudentSurvey.Value);
                                }

                                if (configStudentInformationReiterative != null)
                                {
                                    boolConfigStudentInformationReiterative = bool.Parse(configStudentInformationReiterative.Value);
                                }

                                if (configStudentInformationAllStudent != null)
                                {
                                    boolconfigStudentInformationAllStudent = bool.Parse(configStudentInformationAllStudent.Value);
                                }

                                #region Config para la ficha socioeconomica

                                if (boolConfigStudentSurvey)
                                {
                                    //Reglas de la encuesta de Bienestar
                                    //Existe categorizacion en el periodo actual
                                    var recordCategorization = await _context.InstitutionalRecordCategorizationByStudents.Where(x => x.StudentId == student.Id && x.TermId == term.Id).FirstOrDefaultAsync();
                                    if (recordCategorization == null)
                                    {
                                        context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "StudentInformation" }, { "action", "Index" } });
                                        anyPending = true;
                                    }
                                }
                                if (boolConfigStudentInformation)
                                {
                                    //Reglas de la ficha socioeconomica
                                    //Para todos los estudiantes
                                    if (boolconfigStudentInformationAllStudent)
                                    {
                                        //Para todos no importa el Status
                                        //Si la ficha es reiterativa
                                        if (boolConfigStudentInformationReiterative)
                                        {
                                            //Ha llenado la ficha socioeconomica en el periodo actual
                                            if (studentInformationTerm == null)
                                            {
                                                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "StudentInformation" }, { "action", "Index" } });
                                                anyPending = true;
                                            }
                                        }
                                        else
                                        {
                                            //Ya lleno la ficha en cualquier periodo
                                            if (!hasStudentInformation)
                                            {
                                                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "StudentInformation" }, { "action", "Index" } });
                                                anyPending = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Solo para ingresantes
                                        if (student.Status == ConstantHelpers.Student.States.ENTRANT)
                                        {
                                            //Si la ficha es reiterativa
                                            if (boolConfigStudentInformationReiterative)
                                            {
                                                //Ha llenado la ficha socioeconomica en el periodo actual
                                                if (studentInformationTerm == null)
                                                {
                                                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "StudentInformation" }, { "action", "Index" } });
                                                    anyPending = true;
                                                }
                                            }
                                            else
                                            {
                                                //Ya lleno la ficha en cualquier periodo
                                                if (!hasStudentInformation)
                                                {
                                                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "StudentInformation" }, { "action", "Index" } });
                                                    anyPending = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }

                        }
                    }
                }

                if (anyPending)
                {
                    var result = next();
                }
                else
                {
                    var result = await next();
                }
            }
        }
    }
}
