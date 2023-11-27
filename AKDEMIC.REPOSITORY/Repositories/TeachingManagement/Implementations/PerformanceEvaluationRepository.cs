using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluation;
using Microsoft.EntityFrameworkCore;
using static AKDEMIC.CORE.Helpers.ConstantHelpers.DATATABLE.SERVER_SIDE;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class PerformanceEvaluationRepository : Repository<PerformanceEvaluation>, IPerformanceEvaluationRepository
    {
        public PerformanceEvaluationRepository(AkdemicContext context) : base(context) { }
        public async Task<object> GetPerformanceEvaluation(Guid id)
        {
            var query = _context.PerformanceEvaluations.Where(x => x.Id == id).Select(x => new
            {
                id = x.Id,
                name = x.Name,
                code = x.Code,
                termId = x.TermId,
                endDate = x.EndDate.ToLocalDateFormat(),
                startDate = x.StartDate.ToLocalDateFormat(),
                target = x.Target
            }).FirstAsync();

            return await query;
        }

        public async Task<bool> AnyPerformanceEvaluationByTag(string code, string name, Guid? id)
        {
            IQueryable<PerformanceEvaluation> query = _context.PerformanceEvaluations.AsNoTracking();

            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(x => x.Code == code);
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name == name);
            }
            if (id.HasValue)
            {
                query = query.Where(x => x.Id != id);
            }

            PerformanceEvaluation result = await query.FirstOrDefaultAsync();
            return result != null;
        }

        public async Task<bool> AnyPerformanceEvaluationByDateRange(Guid termId, DateTime startDate, DateTime endDate, Guid? ignoredId)
            => await _context.PerformanceEvaluations.AnyAsync(x =>
            x.TermId == termId &&
            ((x.StartDate <= startDate && x.EndDate > startDate) ||
            (x.StartDate < endDate && x.EndDate >= endDate) ||
            (startDate <= x.EndDate && endDate > x.StartDate) ||
            (startDate < x.EndDate && endDate >= x.EndDate)) && x.Id != ignoredId);

        public async Task<ReturnedData<object>> GetPerformanceEvaluationsDatatable(SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<PerformanceEvaluation, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Term.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Code); break;
                case "2":
                    orderByPredicate = ((x) => x.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.StartDate); break;
                case "4":
                    orderByPredicate = ((x) => x.EndDate); break;
                default:
                    orderByPredicate = ((x) => x.Term.Name); break;
            }

            IQueryable<PerformanceEvaluation> query = _context.PerformanceEvaluations.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()) || x.Term.Name.Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    term = x.Term.Name,
                    code = x.Code,
                    name = x.Name,
                    endDate = x.EndDate.ToLocalDateFormat(),
                    startDate = x.StartDate.ToLocalDateFormat(),
                    status = DateTime.UtcNow.ToDefaultTimeZone().Date < x.StartDate.Date ? 0 : DateTime.UtcNow.ToDefaultTimeZone().Date > x.EndDate.Date ? 2 : 1,
                    canExtend = x.StartDate.Date <= DateTime.UtcNow.ToDefaultTimeZone().Date && x.EndDate.Date >= DateTime.UtcNow.ToDefaultTimeZone().Date,
                    target = ConstantHelpers.PERFORMANCE_EVALUATION.TARGET.VALUES[x.Target]
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

        public async Task<object> GetEvaluatorInfo(string userId, string role)
        {
            IQueryable<object> query;
            switch (role)
            {
                case ConstantHelpers.ROLES.TEACHERS:
                    query = _context.Teachers.AsNoTracking().Where(x => x.UserId == userId).Select(x => new
                    {
                        name = x.User.FullName,
                        career = x.Career.Name,
                        date = DateTime.UtcNow.ToLocalDateTimeFormat(),
                        sex = ConstantHelpers.SEX.VALUES[x.User.Sex],
                        academicDepartment = x.AcademicDepartment.Name ?? "Sin Asignar"
                    }); break;
                case ConstantHelpers.ROLES.DEAN:
                    query = _context.Deans.AsNoTracking().Where(x => x.UserId == userId).Select(x => new
                    {
                        name = x.User.FullName,
                        career = x.Faculty.Name,
                        date = DateTime.UtcNow.ToLocalDateFormat(),
                        sex = ConstantHelpers.SEX.VALUES[x.User.Sex]
                    }); break;
                case ConstantHelpers.ROLES.CAREER_DIRECTOR:
                    query = _context.Careers.AsNoTracking().Where(x => x.CareerDirectorId == userId).Select(x => new
                    {
                        name = x.CareerDirector.FullName,
                        career = x.Name,
                        date = DateTime.UtcNow.ToLocalDateFormat(),
                        sex = ConstantHelpers.SEX.VALUES[x.CareerDirector.Sex]
                    }); break;
                case ConstantHelpers.ROLES.TUTORING_COORDINATOR:
                    query = _context.TutoringCoordinators.AsNoTracking().Where(x => x.UserId == userId).Select(x => new
                    {
                        name = x.User.FullName,
                        career = x.Career.Name,
                        date = DateTime.UtcNow.ToLocalDateFormat(),
                        sex = ConstantHelpers.SEX.VALUES[x.User.Sex]
                    }); break;
                case ConstantHelpers.ROLES.RESEARCH_COORDINATOR:
                    query = _context.Careers.AsNoTracking().Where(x => x.ResearchCoordinatorId == userId).Select(x => new
                    {
                        name = x.ResearchCoordinator.FullName,
                        career = x.Name,
                        date = DateTime.UtcNow.ToLocalDateFormat(),
                        sex = ConstantHelpers.SEX.VALUES[x.ResearchCoordinator.Sex]
                    }); break;
                case ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR:
                    query = _context.Careers.AsNoTracking().Where(x => x.AcademicDepartmentDirectorId == userId).Select(x => new
                    {
                        name = x.AcademicDepartmentDirector.FullName,
                        career = x.Name,
                        date = DateTime.UtcNow.ToLocalDateFormat(),
                        sex = ConstantHelpers.SEX.VALUES[x.AcademicDepartmentDirector.Sex]
                    }); break;
                case ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR:
                    query = _context.Careers.AsNoTracking().Where(x => x.SocialResponsabilityCoordinatorId == userId).Select(x => new
                    {
                        name = x.SocialResponsabilityCoordinator.FullName,
                        career = x.Name,
                        date = DateTime.UtcNow.ToLocalDateFormat(),
                        sex = ConstantHelpers.SEX.VALUES[x.SocialResponsabilityCoordinator.Sex]
                    }); break;
                default:
                    return null;
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<object>> GetRoles(string userId)
        {
            var result = _context.UserRoles.AsNoTracking().Where(x => x.UserId == userId).Where(x =>
                            x.Role.Name == ConstantHelpers.ROLES.DEAN ||
                            x.Role.Name == ConstantHelpers.ROLES.CAREER_DIRECTOR ||
                            x.Role.Name == ConstantHelpers.ROLES.TUTORING_COORDINATOR ||
                            x.Role.Name == ConstantHelpers.ROLES.RESEARCH_COORDINATOR ||
                            x.Role.Name == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR ||
                            x.Role.Name == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR).Select(x => new
                            {
                                id = x.RoleId,
                                text = x.Role.Name
                            });
            return await result.ToListAsync();
        }

        public async Task<ReturnedData<object>> GetTeachersByStudentDatatable(SentParameters sentParameters, string userId)
        {
            var result = new DataTablesStructs.ReturnedData<object>()
            {
                Error = null,
                Data = new List<object>()
            };

            Guid? studentId = null;
            studentId = await _context.Students
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                //.AsEnumerable()
                .FirstOrDefaultAsync();

            if (studentId == Guid.Empty)
            {
                result.Error = "No se encontró el alumno.";
                return result;
            }

            Guid? termId = null;
            termId = await _context.Terms
                .Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE && !x.IsSummer)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            if (termId == Guid.Empty)
            {
                result.Error = "No se realiza evaluación al docente en verano.";
                return result;
            }

            var today = DateTime.UtcNow.ToDefaultTimeZone().Date;
            PerformanceEvaluation survey = await _context.PerformanceEvaluations.AsNoTracking().Where(x => x.TermId == termId && x.StartDate.Date <= today && x.EndDate.Date >= today).FirstOrDefaultAsync();

            if (survey == null)
            {
                result.Error = "No hay encuestas disponibles en el periodo actual.";
                return result;
            }
            var sections = await _context.StudentSections
                .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termId && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                .Select(x => new
                {
                    sectionId = x.SectionId,
                    sectionGroupId = x.SectionGroupId
                })
                .ToListAsync();

            if (sections.Count == 0)
            {
                result.Error = "No cuenta con secciones matriculadas";
                return result;
            }

            var roleId = await _context.Roles
                .Where(x => x.Name == ConstantHelpers.ROLES.TEACHERS)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            if (roleId == null)
            {
                result.Error = "No se encuentra registrado el rol Docente";
                return result;
            }

            var evaluations = await _context.PerformanceEvaluationUsers
                .Where(x => x.FromUserId == userId && x.PerformanceEvaluationId == survey.Id && x.FromRole.Name == ConstantHelpers.ROLES.STUDENTS)
                .ToListAsync();

            var sectionsId = sections.Select(x => x.sectionId).ToList();
            var sectionsGroupId = sections.Select(x => x.sectionGroupId).ToList();

            var dataDB = await _context.TeacherSections
                .Where(x => sectionsId.Contains(x.SectionId))
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    userId = x.TeacherId,
                    sectionId = x.SectionId,
                    section = x.Section.Code,
                    teacher = x.Teacher.User.FullName,
                    career = x.Section.CourseTerm.Course.Career.Name,
                    teacherId = x.TeacherId,
                    courseName = x.Section.CourseTerm.Course.Name,
                    courseCode = x.Section.CourseTerm.Course.Code
                })
                .OrderBy(x => x.career)
                .ThenBy(x => x.courseCode)
                .ThenBy(x => x.section)
                .ThenBy(x => x.teacher)
                .ToListAsync();

            var data = dataDB.
                Select(x => new
                {
                    x.userId,
                    x.sectionId,
                    x.section,
                    x.teacher,
                    x.career,
                    value = evaluations.Where(y => y.ToTeacherId == x.teacherId && y.SectionId == x.sectionId).Select(z => new { z.Value, z.Id }).FirstOrDefault(),
                    course = $"{x.courseCode} {x.courseName}"
                })
                .ToList();

            if (data.Count == 0)
            {
                result.Error = "No hay docentes disponibles para evaluar";
                return result;
            }

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<int> GetPendingSurveys(string userId)
        {
            var student = await _context.Students.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
            var evaluation = await _context.PerformanceEvaluations.Where(x => x.TermId == term.Id && x.StartDate.Date <= DateTime.UtcNow.ToDefaultTimeZone().Date && x.EndDate.Date >= DateTime.UtcNow.ToDefaultTimeZone().Date).FirstOrDefaultAsync();
            var sections = await _context.StudentSections
                .Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == term.Id)
                .Select(x => new
                {
                    sectionId = x.SectionId,
                    sectionGroupId = x.SectionGroupId
                })
                .ToListAsync();

            var evaluations = await _context.PerformanceEvaluationUsers
                .Where(x => x.FromUserId == userId && x.PerformanceEvaluationId == evaluation.Id && x.FromRole.Name == ConstantHelpers.ROLES.STUDENTS)
                .ToListAsync();

            var sectionsId = sections.Select(x => x.sectionId).ToList();
            var teacherSections = await _context.TeacherSections.Where(x => sectionsId.Contains(x.SectionId)).ToListAsync();
            var result = teacherSections.Where(x => !evaluations.Any(y => y.ToTeacherId == x.TeacherId && y.SectionId == x.SectionId)).Count();
            return result;
        }

        public async Task<ReturnedData<object>> GetUsersDatatable(SentParameters sentParameters, string userId, string roleId)
        {
            ApplicationRole role = await _context.Roles.FirstAsync(x => x.Id == roleId);

            ReturnedData<object> result = new DataTablesStructs.ReturnedData<object>()
            {
                Error = null,
                Data = new List<object>()
            };

            Guid termId;
            termId = await _context.Terms.AsNoTracking().Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE && !x.IsSummer).Select(x => x.Id).FirstOrDefaultAsync();

            if (termId == Guid.Empty)
            {
                result.Error = "No se realiza evaluación al docente en verano.";
                return result;
            }

            var today = DateTime.UtcNow.ToDefaultTimeZone().Date;
            PerformanceEvaluation survey = await _context.PerformanceEvaluations.AsNoTracking().Where(x => x.TermId == termId && x.StartDate.Date <= today && x.EndDate.Date >= today).FirstOrDefaultAsync();

            if (survey == null)
            {
                result.Error = "No hay encuestas disponibles en el periodo actual.";
                return result;
            }

            if (survey.Target != ConstantHelpers.PERFORMANCE_EVALUATION.TARGET.ALL)
            {
                result.Error = "La encuesta solo esta habilitada para estudiantes.";
                return result;
            }

            var evaluations = await _context.PerformanceEvaluationUsers
                .Where(x => x.FromUserId == userId && x.PerformanceEvaluationId == survey.Id && x.FromRole.Name == role.Name)
                .Select(x => new
                {
                    toUserId = x.ToTeacherId,
                    value = x.Value.ToString("0.00"),
                }).ToListAsync();

            var teachersQuery = _context.Teachers.Where(x => x.UserId != userId && x.TeacherSections.Any(y => y.Section.CourseTerm.TermId == termId)).AsQueryable();

            if (role.Name == ConstantHelpers.ROLES.DEAN || role.Name == ConstantHelpers.ROLES.DEAN_SECRETARY)
            {
                teachersQuery = teachersQuery.Where(x => x.AcademicDepartment.Faculty.DeanId == userId || x.AcademicDepartment.Faculty.SecretaryId == userId);
            }
            else if (role.Name == ConstantHelpers.ROLES.CAREER_DIRECTOR)
            {
                teachersQuery = teachersQuery.Where(x => x.AcademicDepartment.Career.CareerDirectorId == userId);
            }
            else if (role.Name == ConstantHelpers.ROLES.TUTORING_COORDINATOR)
            {
                var careers = await _context.TutoringCoordinators.Where(x => x.UserId == userId).Select(x => x.CareerId).ToListAsync();
                teachersQuery = teachersQuery.Where(x => x.AcademicDepartment.CareerId.HasValue && careers.Contains(x.AcademicDepartment.CareerId.Value));
            }
            else if (role.Name == ConstantHelpers.ROLES.RESEARCH_COORDINATOR)
            {
                teachersQuery = teachersQuery.Where(x => x.AcademicDepartment.Career.ResearchCoordinatorId == userId);
            }
            else if (role.Name == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR)
            {
                teachersQuery = teachersQuery.Where(x => x.AcademicDepartment.AcademicDepartmentDirectorId == userId);
            }
            else if (role.Name == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR)
            {
                teachersQuery = teachersQuery.Where(x => x.AcademicDepartment.Career.SocialResponsabilityCoordinatorId == userId);
            }
            int total = await teachersQuery.CountAsync();

            var dataDB = await teachersQuery
                .Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw)
                .Select(x => new EvaluationDetail
                {
                    AcademicDepartment = x.AcademicDepartment.Name,
                    FromRoleId = role.Id,
                    FromRoleName = role.Name,
                    ToUserId = x.UserId,
                    ToUserName = x.User.UserName,
                    ToUserFullname = x.User.FullName,
                })
                .ToListAsync();

            var data = dataDB
               .Select(x => new EvaluationDetail
               {
                   AcademicDepartment = string.IsNullOrEmpty(x.AcademicDepartment) ? "Sin Asignar" : x.AcademicDepartment,
                   FromRoleId = x.FromRoleId,
                   FromRoleName = x.FromRoleName,
                   ToUserId = x.ToUserId,
                   ToUserName = x.ToUserName,
                   ToUserFullname = x.ToUserFullname,
                   Evaluation = evaluations.Where(y => y.toUserId == x.ToUserId).FirstOrDefault()
               })
               .ToList();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = total,
                RecordsTotal = data.Count
            };
        }

        public async Task<DetailedReport> GetDetailedReportChartJS(Guid evaluationId, Guid academicDepartmentId)
        {
            var academicDepartment = await _context.AcademicDepartments.Where(x => x.Id == academicDepartmentId).Include(x => x.Faculty).FirstOrDefaultAsync();
            var evaluation = await _context.PerformanceEvaluations.Where(x => x.Id == evaluationId).FirstOrDefaultAsync();
            var term = await _context.Terms.Where(x => x.Id == evaluation.TermId).FirstOrDefaultAsync();
            var templateId = await _context.RelatedPerformanceEvaluationTemplates.Where(x => x.PerformanceEvaluationId == evaluationId && x.PerformanceEvaluationTemplate.Role.Name == ConstantHelpers.ROLES.STUDENTS).Select(x => x.PerformanceEvaluationTemplateId).FirstOrDefaultAsync();
            var template = await _context.PerformanceEvaluationTemplates.Where(x => x.Id == templateId).Include(x => x.Questions).FirstOrDefaultAsync();
            var usersRespones = await _context.PerformanceEvaluationResponses
                .Where(x =>
                x.PerformanceEvaluationUser.PerformanceEvaluationTemplateId == template.Id
                && x.PerformanceEvaluationUser.FromRole.Name == ConstantHelpers.ROLES.STUDENTS
                && x.PerformanceEvaluationUser.PerformanceEvaluationId == evaluation.Id
                && x.PerformanceEvaluationUser.ToTeacher.AcademicDepartmentId == academicDepartmentId
                )
                .GroupBy(x => new { x.PerformanceEvaluationQuestionId, x.Value })
                .Select(x => new
                {
                    x.Key,
                    cout = x.Count()
                })
                .ToListAsync();

            var model = new DetailedReport
            {
                Max = template.Max,
                AcademicDepartment = academicDepartment.Name,
                Faculty = academicDepartment.Faculty.Name,
                Term = term.Name,
                Details = new List<DataDetails>(),
                Evaluation = evaluation.Name
                //Details = new List<DetailedReportDetails>(),
                //TableDetails = new List<DetailedTableReport>()
            };

            foreach (var item in template.Questions.OrderBy(x=>x.CreatedAt).ToList())
            {
                var responses = new List<object>();
                var tableDetails = new List<DetailedTableReport>();
                var total = (double)usersRespones.Where(x => x.Key.PerformanceEvaluationQuestionId == item.Id).Sum(x => x.cout);

                if (total == 0)
                    total = 1;

                for (int i = 1; i <= template.Max; i++)
                {
                    var quantity = usersRespones.Where(x => x.Key.PerformanceEvaluationQuestionId == item.Id && x.Key.Value == i).Select(x => x.cout).FirstOrDefault();
                    var response = new object[]
                    {
                        ConstantHelpers.PERFORMANCE_EVALUATION.RATING_SCALE.GET_NAME(template.Max,(byte)i),
                        Math.Round(((double)quantity / total) * 100,2,MidpointRounding.AwayFromZero),
                    };

                    var detailDatatable = new DetailedTableReport
                    {
                        Name = ConstantHelpers.PERFORMANCE_EVALUATION.RATING_SCALE.GET_NAME(template.Max, (byte)i),
                        Percentage = Math.Round(((double)quantity / total) * 100, 2, MidpointRounding.AwayFromZero),
                        Quantity = quantity
                    };

                    tableDetails.Add(detailDatatable);
                    responses.Add(response);
                }

                var detail = new DataDetails
                {
                    Question = item.Description,
                    Details = new DetailedReportDetails
                    {
                        name = item.Description,
                        data = responses.ToArray()
                    },
                    TableDetails = tableDetails
                };

                model.Details.Add(detail);
            }

            return model;

        }

        public async Task<CommentaryReport> GetCommentariesByEvaluation(Guid evaluationId, Guid academicDepartmentId)
        {
            var evaluation = await _context.PerformanceEvaluations.Include(x => x.Term).Where(x => x.Id == evaluationId).FirstOrDefaultAsync();
            var academicDepartment = await _context.AcademicDepartments.Where(x => x.Id == academicDepartmentId).FirstOrDefaultAsync();

            var query = _context.PerformanceEvaluationUsers
                .Where(x => x.PerformanceEvaluationId == evaluation.Id && x.FromRole.Name == ConstantHelpers.ROLES.STUDENTS && !string.IsNullOrEmpty(x.Commentary))
                .AsNoTracking();

            if (academicDepartment is null)
            {
                query = query.Where(x => !x.ToTeacher.AcademicDepartmentId.HasValue);
            }
            else
            {
                query = query.Where(x => x.ToTeacher.AcademicDepartmentId == academicDepartment.Id);
            }

            var commentaries = await query
                .Select(x => new { x.ToTeacher.User.FullName, x.Commentary })
                .ToArrayAsync();

            var model = new CommentaryReport
            {
                Term = evaluation.Term.Name,
                //Career = career.Name,
                AcademicDepartment = academicDepartment?.Name ?? "Sin Asignar",
                //Faculty = career.Faculty.Name,
                Details = commentaries.Select(x => new CommentaryByTeacher
                {
                    Commentary = x.Commentary.Trim(),
                    Teacher = x.FullName
                })
                .ToList()
            };

            return model;
        }

        public async Task<CommentaryReport> GetCommentariesByEvaluation(Guid evaluationId, ClaimsPrincipal user)
        {
            var evaluation = await _context.PerformanceEvaluations.Include(x => x.Term).Where(x => x.Id == evaluationId).FirstOrDefaultAsync();

            var query = _context.PerformanceEvaluationUsers.Where(x => x.PerformanceEvaluationId == evaluation.Id && x.FromRole.Name == ConstantHelpers.ROLES.STUDENTS && !string.IsNullOrEmpty(x.Commentary))
                .AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                query = query.Where(x => x.ToTeacher.AcademicDepartment.AcademicDepartmentDirectorId == userId || x.ToTeacher.AcademicDepartment.AcademicDepartmentSecretaryId == userId);
            }

            var commentaries = await query
                .Select(x => new { x.ToTeacher.User.FullName, x.Commentary, career = x.ToTeacher.Career.Name, academicDepartment = x.ToTeacher.AcademicDepartment.Name })
                .ToArrayAsync();

            var model = new CommentaryReport
            {
                Term = evaluation.Term.Name,
                Details = commentaries.Select(x => new CommentaryByTeacher
                {
                    Commentary = x.Commentary.Trim(),
                    Teacher = x.FullName,
                    AcademicDepartment = x.academicDepartment,
                    Career = x.career
                })
                .ToList()
            };

            return model;
        }

        public async Task<List<string>> GetCommentariesByTeacher(Guid evaluationId, string teacherId)
        {
            var evaluation = await _context.PerformanceEvaluations.Where(x => x.Id == evaluationId).FirstOrDefaultAsync();
            var commentaries = await _context.PerformanceEvaluationUsers.Where(x => x.PerformanceEvaluationId == evaluation.Id && x.ToTeacherId == teacherId && x.FromRole.Name == ConstantHelpers.ROLES.STUDENTS && !string.IsNullOrEmpty(x.Commentary))
                .Select(x => x.Commentary).ToListAsync();

            return commentaries;
        }

        public async Task<bool> OnlyViewStudentPerformanceEvaluation(string userId)
        {
            var termId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE && !x.IsSummer).Select(x => x.Id).FirstOrDefaultAsync();
            var configuration = await _context.Configurations.Where(X => X.Key == ConstantHelpers.Configuration.TeacherManagement.PERFORMANCE_EVALUATION_REQUIRED).FirstOrDefaultAsync();

            if (configuration is null)
            {
                configuration = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.TeacherManagement.PERFORMANCE_EVALUATION_REQUIRED,
                    Value = ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.TeacherManagement.PERFORMANCE_EVALUATION_REQUIRED]
                };
            }

            var isRequired = Convert.ToBoolean(configuration.Value);


            if (termId != null && isRequired)
            {
                var evaluations = await _context.PerformanceEvaluations.Where(x => x.TermId == termId).ToListAsync();

                if (evaluations.Any() && evaluations.Any(x => x.StartDate.Date <= DateTime.UtcNow.ToDefaultTimeZone().Date && x.EndDate.Date >= DateTime.UtcNow.ToDefaultTimeZone()))
                {
                    var evaluation = evaluations.Where(x => x.StartDate.Date <= DateTime.UtcNow.ToDefaultTimeZone().Date && x.EndDate.Date >= DateTime.UtcNow.ToDefaultTimeZone()).FirstOrDefault();
                    var student = await _context.Students.Where(x => x.UserId == userId).FirstOrDefaultAsync();

                    var teacherRole = await _context.Roles.Where(x => x.Name == ConstantHelpers.ROLES.TEACHERS).Select(x => x.Id).FirstOrDefaultAsync();
                    var userEvaluations = await _context.PerformanceEvaluationUsers.Where(x => x.FromUserId == student.UserId && x.PerformanceEvaluationId == evaluation.Id && x.FromRole.Name == ConstantHelpers.ROLES.STUDENTS).CountAsync();
                    var sections = await _context.StudentSections.Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == termId).Select(x => x.SectionId).ToListAsync();
                    var teacherSection = await _context.TeacherSections.Where(x => sections.Contains(x.SectionId)).CountAsync();
                    if (teacherSection > userEvaluations)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        public async Task<string> GetSummaryStudentsByEvaluation(Guid evaluationId)
        {
            var evaluation = await _context.PerformanceEvaluations.Where(x => x.Id == evaluationId).FirstOrDefaultAsync();

            var studentsDB = await _context.Students.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == evaluation.TermId))
                .Select(x => new
                {
                    x.UserId,
                    sections = x.StudentSections.Where(y => y.Section.CourseTerm.TermId == evaluation.TermId).Select(y => y.SectionId).ToList()
                })
                .ToListAsync();


            var students = studentsDB
                .Select(x => new SummaryStudentTemplate
                {
                    UserId = x.UserId,
                    Total = _context.TeacherSections.Where(y => x.sections.Contains(y.SectionId)).Count()
                })
                .ToList();

            var evaluationUsers = await _context.PerformanceEvaluationUsers
                .Where(x => x.PerformanceEvaluationId == evaluation.Id && x.FromRole.Name == ConstantHelpers.ROLES.STUDENTS)
                .GroupBy(x => x.FromUserId)
                .Select(x => new SummaryStudentTemplate
                {
                    UserId = x.Key,
                    Total = x.Count()
                })
                .ToListAsync();

            var studentsWhoResponded = students.Where(x => evaluationUsers.Any(y => y.UserId == x.UserId && y.Total == x.Total)).ToList();

            var pending = students.Count() - studentsWhoResponded.Count();

            return $"{pending}/{students.Count()}";
        }

        public async Task<PerformanceEvaluation> GetPerformanceEvaluationInCourseByTerm()
        {
            var today = DateTime.UtcNow.ToDefaultTimeZone().Date;
            var evaluation = await _context.PerformanceEvaluations.Where(x => x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE && x.StartDate.Date <= today && x.EndDate.Date >= today).FirstOrDefaultAsync();
            return evaluation;
        }

        public async Task<bool> EvaluationHasResponses(Guid evaluationId)
            => await _context.PerformanceEvaluationUsers.AnyAsync(x => x.PerformanceEvaluationId == evaluationId);

        public async Task DeleteRelatedPerformanceEvaluation(Guid evaluationId)
        {
            var templates = await _context.RelatedPerformanceEvaluationTemplates.Where(x => x.PerformanceEvaluationId == evaluationId).ToListAsync();
            _context.RelatedPerformanceEvaluationTemplates.RemoveRange(templates);
            await _context.SaveChangesAsync();
        }

        public async Task<ReportSectionConsolidatedTemplate> GetReportSectionConsolidated(Guid evaluationId, Guid? academicDepartmentId)
        {
            var performanceEvaluation = await _context.PerformanceEvaluations.Where(x => x.Id == evaluationId)
                .Select(x => new
                {
                    x.Id,
                    term = x.Term.Name,
                    x.TermId,
                    x.Name,
                    x.Code
                })
                .FirstOrDefaultAsync();
            var template = await _context.RelatedPerformanceEvaluationTemplates.Where(x => x.PerformanceEvaluationId == evaluationId && x.PerformanceEvaluationTemplate.Role.Name == ConstantHelpers.ROLES.STUDENTS)
                .Select(x => new
                {
                    x.PerformanceEvaluationTemplate.Max,
                    questions = x.PerformanceEvaluationTemplate.Questions.Count(),
                    maximumRaiting = x.PerformanceEvaluationTemplate.Questions.Count() * x.PerformanceEvaluationTemplate.Max
                })
                .FirstOrDefaultAsync();

            var model = new ReportSectionConsolidatedTemplate
            {
                EvaluationName = $"{performanceEvaluation.Code} - {performanceEvaluation.Name}",
                Term = performanceEvaluation.term,
            };

            var queryteachers = _context.Teachers
                .Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.TermId == performanceEvaluation.TermId))
                .OrderBy(x => x.User.FullName)
                .AsNoTracking();

            if (academicDepartmentId.HasValue)
            {
                var academicDepartment = await _context.AcademicDepartments.Where(x => x.Id == academicDepartmentId).Select(x => x.Name).FirstOrDefaultAsync();
                model.AcademicDepartment = academicDepartment;
                queryteachers = queryteachers.Where(x => x.AcademicDepartmentId == academicDepartmentId);
            }

            model.Teachers = await queryteachers
                .Select(x => new TeacherSectionTemplate
                {
                    FullName = x.User.FullName,
                    Sections = x.TeacherSections.Where(x => x.Section.CourseTerm.TermId == performanceEvaluation.TermId).Select(y => new SectionConsolidatedTemplate
                    {
                        Code = y.Section.Code,
                        CourseCode = y.Section.CourseTerm.Course.Code,
                        Course = y.Section.CourseTerm.Course.Name,
                        SumResponses = y.Teacher.PerformanceEvaluationUsers.Where(z => z.PerformanceEvaluationId == evaluationId && z.FromRole.Name == ConstantHelpers.ROLES.STUDENTS && z.SectionId == y.SectionId).Sum(y => y.Responses.Sum(y => y.Value)),
                        AnyEvaluationAnswered = y.Teacher.PerformanceEvaluationUsers.Any(z => z.PerformanceEvaluationId == evaluationId && z.FromRole.Name == ConstantHelpers.ROLES.STUDENTS && z.SectionId == y.SectionId),
                        SurveysAnswered = y.Teacher.PerformanceEvaluationUsers.Where(z => z.PerformanceEvaluationId == evaluationId && z.FromRole.Name == ConstantHelpers.ROLES.STUDENTS && z.SectionId == y.SectionId).Count()
                    }).ToList()
                })
                .ToListAsync();

            foreach (var teacher in model.Teachers)
            {
                teacher.Sections.ForEach(x =>
                {
                    x.Average = x.SurveysAnswered == 0 ? 0 : Math.Round(((x.SumResponses * 20M) / (template.maximumRaiting * x.SurveysAnswered)), 2, MidpointRounding.AwayFromZero);
                });

                teacher.Average = teacher.Sections.Where(y => y.Average != 0).Count() == 0 ? 0 : Math.Round((teacher.Sections.Sum(y => y.Average) / teacher.Sections.Where(y => y.Average != 0).Count()), 2, MidpointRounding.AwayFromZero);
            }

            return model;
        }

        public async Task<int> GenerateResultScale(Guid performanceEvaluationId)
        {
            var evaluation = await _context.PerformanceEvaluations.Where(x => x.Id == performanceEvaluationId).FirstOrDefaultAsync();
            var term = await _context.Terms.Where(x => x.Id == evaluation.TermId).FirstOrDefaultAsync();

            var scales = await _context.ScaleExtraPerformanceEvaluationFields
                .Include(x => x.ScaleResolution)
                .Where(x => x.PerformanceEvaluationId == performanceEvaluationId).ToListAsync();

            var scaleSection = await _context.ScaleSections.Where(x => x.SectionNumber == ConstantHelpers.RESOLUTION_SECTIONS.PERFORMANCE_EVALUATION).FirstOrDefaultAsync();
            var resolutiontype = await _context.ScaleResolutionTypes.Where(x => x.Name == "Evaluación docente").FirstOrDefaultAsync();

            var scaleSectionResolutionType = await _context.ScaleSectionResolutionTypes.Where(x => x.ScaleResolutionTypeId == resolutiontype.Id && x.ScaleSectionId == scaleSection.Id).FirstOrDefaultAsync();

            var entites = new List<ScaleExtraPerformanceEvaluationField>();

            var queryUserGroup = _context.PerformanceEvaluationUsers.Where(x => x.PerformanceEvaluationId == evaluation.Id).AsNoTracking();
            var userGroup = await queryUserGroup
                .GroupBy(x => x.ToTeacherId)
                .Select(x => new
                {
                    x.Key,
                    total = x.Count()
                })
                .ToListAsync();

            var questionsByTemplateDB = await _context.RelatedPerformanceEvaluationTemplates.Where(x => x.PerformanceEvaluationId == evaluation.Id)
                .Select(x => new
                {
                    questions = x.PerformanceEvaluationTemplate.Questions.Count(),
                    role = x.PerformanceEvaluationTemplate.Role.Name,
                    x.PerformanceEvaluationTemplate.Max
                })
                .ToListAsync();

            var questionsByTemplate = questionsByTemplateDB
                .Select(x => new
                {
                    x.questions,
                    x.role,
                    x.Max,
                    maximumRaiting = x.questions * (int)x.Max
                })
                .ToList();

            var teachers = await queryUserGroup.IgnoreQueryFilters().Include(x => x.ToTeacher.User).Select(x => x.ToTeacher).ToListAsync();

            var questionsToStudent = questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.STUDENTS).Select(x => x.questions).FirstOrDefault();
            var questionsToAcademicDepartmentDirector = questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR).Select(x => x.questions).FirstOrDefault();
            var questionsToCareerDirector = questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.CAREER_DIRECTOR).Select(x => x.questions).FirstOrDefault();
            var questionsToDean = questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.DEAN).Select(x => x.questions).FirstOrDefault();
            var questionsToResearchCoordinator = questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.RESEARCH_COORDINATOR).Select(x => x.questions).FirstOrDefault();
            var questionsToSocialResponsbilityCoordinator = questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR).Select(x => x.questions).FirstOrDefault();
            var questionsToTutoringCoordinator = questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.TUTORING_COORDINATOR).Select(x => x.questions).FirstOrDefault();

            var MaximumRaitingStudent = (decimal)(questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.STUDENTS).Select(x => x.maximumRaiting).FirstOrDefault());
            var MaximumRaitingAcademicDepartmentDirector = (decimal)(questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR).Select(x => x.maximumRaiting).FirstOrDefault());
            var MaximumRaitingCareerDirector = (decimal)(questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.CAREER_DIRECTOR).Select(x => x.maximumRaiting).FirstOrDefault());
            var MaximumRaitingDean = (decimal)(questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.DEAN).Select(x => x.maximumRaiting).FirstOrDefault());
            var MaximumRaitingResearchCoordinator = (decimal)(questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.RESEARCH_COORDINATOR).Select(x => x.maximumRaiting).FirstOrDefault());
            var MaximumRaitingToSocialResponsbilityCoordinator = (decimal)(questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR).Select(x => x.maximumRaiting).FirstOrDefault());
            var MaximumRaitingToTutoringCoordinator = (decimal)(questionsByTemplate.Where(x => x.role == ConstantHelpers.ROLES.TUTORING_COORDINATOR).Select(x => x.maximumRaiting).FirstOrDefault());

            var allPerformanceEvaluationUsers = await _context.PerformanceEvaluationUsers.Where(x => x.PerformanceEvaluationId == evaluation.Id)
                  .Select(x => new
                  {
                      x.ToTeacherId,
                      x.FromUserId,
                      FromRoleName = x.FromRole.Name,
                      SumResponses = x.Responses.Sum(y => y.Value)
                  })
                    .ToListAsync();

            var baseScore =
                        (
                        MaximumRaitingStudent +
                        MaximumRaitingAcademicDepartmentDirector +
                        MaximumRaitingCareerDirector +
                        MaximumRaitingDean +
                        MaximumRaitingResearchCoordinator +
                        MaximumRaitingToSocialResponsbilityCoordinator +
                        MaximumRaitingToTutoringCoordinator
                        );


            foreach (var user in userGroup)
            {
                var teacher = teachers.Where(x => x.UserId == user.Key).FirstOrDefault();
                if (teacher is null)
                    continue;

                var performanceEvaluationUser = allPerformanceEvaluationUsers
                    .Where(x => x.ToTeacherId == user.Key)
                    .ToList();

                var finalGradeStudents = performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.STUDENTS).Any() ? (performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.STUDENTS).Sum(x => x.SumResponses)) / (1M * performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.STUDENTS).Count()) : 0;
                var academicDepartmentDirectorFinalGrade = performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR).Any() ? (performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR).Sum(x => x.SumResponses)) / (1M * performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR).Count()) : 0;
                var careerDirectorFinalGrade = performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.CAREER_DIRECTOR).Any() ? (performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.CAREER_DIRECTOR).Sum(x => x.SumResponses)) / (1M * performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.CAREER_DIRECTOR).Count()) : 0;
                var deanFinalGrade = performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.DEAN).Any() ? (performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.DEAN).Sum(x => x.SumResponses)) / (1M * performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.DEAN).Count()) : 0;
                var researchCoordinatorFinalGrade = performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.RESEARCH_COORDINATOR).Any() ? (performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.RESEARCH_COORDINATOR).Sum(x => x.SumResponses)) / (1M * performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.RESEARCH_COORDINATOR).Count()) : 0;
                var socialResponsabilityCoordinatorFinalGrade = performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR).Any() ? (performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR).Sum(x => x.SumResponses)) / (1M * performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.SOCIAL_RESPONSABILITY_COORDINATOR).Count()) : 0;
                var tutoringCoordinatorFinalGrade = performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.TUTORING_COORDINATOR).Any() ? (performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.TUTORING_COORDINATOR).Sum(x => x.SumResponses)) / (1m * performanceEvaluationUser.Where(x => x.FromRoleName == ConstantHelpers.ROLES.TUTORING_COORDINATOR).Count()) : 0;

                decimal dividedBy = 0;

                if (academicDepartmentDirectorFinalGrade != 0)
                    dividedBy += MaximumRaitingAcademicDepartmentDirector;
                if (careerDirectorFinalGrade != 0)
                    dividedBy += MaximumRaitingCareerDirector;
                if (deanFinalGrade != 0)
                    dividedBy += MaximumRaitingDean;
                if (researchCoordinatorFinalGrade != 0)
                    dividedBy += MaximumRaitingResearchCoordinator;
                if (socialResponsabilityCoordinatorFinalGrade != 0)
                    dividedBy += MaximumRaitingToSocialResponsbilityCoordinator;
                if (tutoringCoordinatorFinalGrade != 0)
                    dividedBy += MaximumRaitingToTutoringCoordinator;
                if (dividedBy == 0)
                    dividedBy = 1;

                var finalGradeAuthorities = ((academicDepartmentDirectorFinalGrade * MaximumRaitingAcademicDepartmentDirector) +
                                            (careerDirectorFinalGrade * MaximumRaitingCareerDirector) +
                                            (deanFinalGrade * MaximumRaitingDean) +
                                            (researchCoordinatorFinalGrade * MaximumRaitingResearchCoordinator) +
                                            (socialResponsabilityCoordinatorFinalGrade * MaximumRaitingToSocialResponsbilityCoordinator) +
                                            (tutoringCoordinatorFinalGrade * MaximumRaitingToTutoringCoordinator)) / dividedBy;

                var finalGrade = 0M;
                if (evaluation.Target == ConstantHelpers.PERFORMANCE_EVALUATION.TARGET.ALL)
                {
                    if (finalGradeAuthorities > 0M)
                        finalGrade += finalGradeAuthorities;

                    if (finalGradeStudents > 0M)
                        finalGrade += finalGradeStudents;

                    if (finalGradeAuthorities > 0M && finalGradeStudents > 0M)
                        finalGrade /= 2;
                }
                else
                {
                    finalGrade = finalGradeStudents;
                }

                finalGrade = Math.Round(finalGrade, 2, MidpointRounding.AwayFromZero);

                var scale = scales.Where(x => x.ScaleResolution.UserId == teacher.UserId).FirstOrDefault();


                if (scale is null)
                {
                    entites.Add(new ScaleExtraPerformanceEvaluationField
                    {
                        BaseScore = baseScore,
                        Qualification = finalGrade,
                        TermId = evaluation.TermId,
                        EvaluationType = evaluation.Code,
                        PerformanceEvaluationId = evaluation.Id,
                        ScaleResolution = new ScaleResolution
                        {
                            UserId = teacher.UserId,
                            ScaleSectionResolutionTypeId = scaleSectionResolutionType.Id,
                            BeginDate = evaluation.StartDate,
                            EndDate = evaluation.EndDate,
                            DocumentType = ConstantHelpers.SCALERESOLUTION_DOCUMENT_TYPE.RESOLUTION,
                            ExpeditionDate = DateTime.UtcNow,
                            IssueAgency = ConstantHelpers.Institution.Values[ConstantHelpers.GENERAL.Institution.Value],
                            ResolutionNumber = teacher.User.UserName,
                            Observation = $"Evaluación desempeño docente - {term.Name}"
                        }
                    });
                }
                else
                {
                    scale.BaseScore = baseScore;
                    scale.Qualification = finalGrade;
                }
            }


            for (int i = 0; i < entites.Count(); i = i + 500)
            {
                var temp = entites.Skip(i).Take(500).ToList();
                await _context.ScaleExtraPerformanceEvaluationFields.AddRangeAsync(temp);
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
            return entites.Count();
        }

        public async Task<List<DetailedReportTemplate>> GetDetailedReport(Guid performanceEvaluationId)
        {
            var performanceEvaluation = await _context.PerformanceEvaluations.Where(x => x.Id == performanceEvaluationId).FirstOrDefaultAsync();

            var term = await _context.Terms.Where(x => x.Id == performanceEvaluation.TermId).FirstOrDefaultAsync();

            var teacherSections = await _context.TeacherSections.Where(x => x.Section.CourseTerm.Term.Id == term.Id && x.Section.StudentSections.Any()).ToListAsync();
            var performanceEvaluationUsers = await _context.PerformanceEvaluationUsers
                .Where(x =>
                   x.PerformanceEvaluationId == performanceEvaluationId &&
                    x.SectionId.HasValue
                )
                .Select(x => new
                {
                    x.SectionId,
                    x.ToTeacherId,
                    sumValues = x.Responses.Sum(y => y.Value)
                })
                .ToListAsync();

            var result = await _context.TeacherSections.Where(x => x.Section.CourseTerm.Term.Id == term.Id && x.Section.StudentSections.Any())
                .Select(x => new DetailedReportTemplate
                {
                    Section = x.Section.Code,
                    CodCourse = x.Section.CourseTerm.Course.Code,
                    Course = x.Section.CourseTerm.Course.Name,
                    Teacher = x.Teacher.User.FullName,
                    AcademicDepartment = x.Teacher.AcademicDepartment.Name,
                    Enrollment = x.Section.StudentSections.Count(),
                    Curriculum = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Code).FirstOrDefault(),
                    Career = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Career.Name).FirstOrDefault(),
                    AcademicYear = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault(),
                    ToTeacherId = x.TeacherId,
                    SectionId = x.SectionId
                })
                .ToListAsync();

            foreach (var teacherSection in result)
            {
                var performanceEvaluationUser = performanceEvaluationUsers.Where(x => x.ToTeacherId == teacherSection.ToTeacherId && x.SectionId == teacherSection.SectionId).ToList();

                teacherSection.Value = performanceEvaluationUser.Sum(y => y.sumValues);
                teacherSection.Surveys = performanceEvaluationUser.Count();
            }

            return result;
        }

        public async Task<string> GetNewCode(Guid termId)
        {
            var evaluationsByTerm = await _context.PerformanceEvaluations.Where(x => x.TermId == termId).CountAsync();
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();

            var result = $"{term.Name}-ED{(evaluationsByTerm + 1)}";
            return result;

        }
    }
}
