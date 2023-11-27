using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EvaluationReport;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class EvaluationReportRepository : Repository<EvaluationReport>, IEvaluationReportRepository
    {
        public EvaluationReportRepository(AkdemicContext context) : base(context) { }
        public async Task<DataTablesStructs.ReturnedData<object>> GetAllEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty = null, Guid? school = null, Guid? career = null, string searchValue = null, ClaimsPrincipal user = null, byte? status = null, Guid? termId = null, byte? type = null)
        {
            Expression<Func<EvaluationReport, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Section.CourseTerm.Course.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Section.Code;
                    break;
                case "3":
                    orderByPredicate = (x) => x.LastReportGeneratedDate;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Status;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }

            var query = _context.EvaluationReports
                .AsNoTracking();

            if (type.HasValue)
                query = query.Where(x => x.Type == type);

            if (!termId.HasValue)
            {
                var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
                if (term == null)
                    term = new ENTITIES.Models.Enrollment.Term();
                termId = term.Id;
            }

            query = _context.EvaluationReports
                .Where(x => x.TermId == termId)
                .AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();
                    query = query.Where(x => (x.Course.CareerId.HasValue && careers.Contains(x.Course.CareerId.Value)) || (x.Section.CourseTerm.Course.CareerId.HasValue && careers.Contains(x.Section.CourseTerm.Course.CareerId.Value)));
                }
            }

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x => x.Section.CourseTerm.Course.Name.Contains(searchValue) || x.Section.Code.Contains(searchValue) || x.Code.Contains(searchValue));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    type = x.Type,
                    typeText = ConstantHelpers.Intranet.EvaluationReportType.NAMES.ContainsKey(x.Type) ? ConstantHelpers.Intranet.EvaluationReportType.NAMES[x.Type] : "-",
                    x.Code,
                    courseName = x.CourseId.HasValue ? x.Course.FullName : x.SectionId.HasValue ? x.Section.CourseTerm.Course.FullName : "-",
                    lastGenerated = x.LastReportGeneratedDate.ToLocalDateTimeFormat(),
                    status = ConstantHelpers.Intranet.EvaluationReport.NAMES[x.Status],
                    isReceived = x.Status == ConstantHelpers.Intranet.EvaluationReport.RECEIVED,
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<EvaluationReport> GetEvaluationReportBySectionId(Guid sectionId)
        {
            var evaluationReport = await _context.EvaluationReports.FirstOrDefaultAsync(x => x.SectionId == sectionId);
            return evaluationReport;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSearchEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters, int? resolutionNumber = null, Guid? termId = null, Guid? careerId = null, Guid? curriculumId = null, Guid? courseId = null, string code = null, string courseSearch = null, bool? onlyReceived = null)
        {
            Expression<Func<EvaluationReport, dynamic>> orderByPredicate = null;


            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Section.Code;
                    break;
                case "3":
                    orderByPredicate = (x) => x.LastReportGeneratedDate;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Status;
                    break;
                default:
                    orderByPredicate = (x) => x.Id;
                    break;
            }


            var query = _context.EvaluationReports
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .AsNoTracking();

            if (courseId != null)
                query = query.Where(x => x.Section.CourseTerm.CourseId == courseId || x.CourseId == courseId);

            if (termId != null)
                query = query.Where(x => x.TermId == termId);

            if (careerId != null)
                query = query.Where(x => x.Section.CourseTerm.Course.CareerId == careerId || x.Course.CareerId == careerId);

            if (curriculumId != null)
                query = query.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId)
                 || x.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (resolutionNumber != null)
                query = query.Where(x => x.GeneratedId == resolutionNumber);

            if (code != null)
                query = query.Where(x => x.Code == code);

            if(onlyReceived.HasValue && onlyReceived.Value)
            {
                query = query.Where(x => x.Status == ConstantHelpers.Intranet.EvaluationReport.RECEIVED);
            }

            if (!string.IsNullOrEmpty(courseSearch))
            {
                courseSearch = courseSearch.ToLower().Trim();
                query = query.Where(x => x.Section.CourseTerm.Course.Code.ToLower().Contains(courseSearch) || x.Section.CourseTerm.Course.Name.ToLower().Contains(courseSearch) || x.Course.Name.ToLower().Contains(courseSearch) || x.Course.Code.ToLower().Contains(courseSearch));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    term = x.Term.Name,
                    code = x.Code,
                    section = x.SectionId.HasValue ? x.Section.Code : "CURSO DIRIGIDO",
                    course = x.SectionId.HasValue ? x.Section.CourseTerm.Course.FullName : x.Course.FullName,
                    teachers = x.SectionId.HasValue ?
                    string.Join("; ", x.Section.TeacherSections.Select(t => t.Teacher.User.FullName).ToArray())
                    : x.Teacher.User.FullName,
                    resolutionNumber = x.GeneratedId,
                    type = x.Type,
                    courseId = x.CourseId,
                    teacherId = x.TeacherId,
                    ReceptionDate = x.ReceptionDate.ToLocalDateFormat(),
                    //curriculum = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear.Curriculum.Name).FirstOrDefault(),
                    //career = x.Section.CourseTerm.Course.Career.Name,
                    //lastGenerated = x.LastReportGeneratedDate.ToLocalDateTimeFormat(),
                    //status = ConstantHelpers.Intranet.EvaluationReport.NAMES[x.Status],
                    //term = x.Section.CourseTerm.Term.Name
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

        public async Task<EvaluationReport> GetEvalutionReportByTeacherIdAndCourseId(Guid courseId, string teacherId)
            => await _context.EvaluationReports.Where(x => x.TeacherId == teacherId && x.CourseId == courseId).FirstOrDefaultAsync();

        public async Task<IEnumerable<EvaluationReportExcelTemplate>> GetEvaluationReportExcel(Guid termId, byte? status)
        {
            var query = _context.EvaluationReports.AsNoTracking();
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();
            query = _context.EvaluationReports.Where(x => x.Section.CourseTerm.TermId == term.Id).AsNoTracking();

            if (status.HasValue)
            {
                if (status == ConstantHelpers.Intranet.EvaluationReport.GENERATED)
                {
                    query = query.Where(x => x.Status == ConstantHelpers.Intranet.EvaluationReport.GENERATED || x.Status == ConstantHelpers.Intranet.EvaluationReport.GENERATED);
                }
                else if (status == ConstantHelpers.Intranet.EvaluationReport.RECEIVED)
                {
                    query = query.Where(x => x.Status == ConstantHelpers.Intranet.EvaluationReport.RECEIVED);
                }
            }

            var data = await query
                .Select(x => new EvaluationReportExcelTemplate
                {
                    Course = x.SectionId.HasValue ? x.Section.CourseTerm.Course.FullName : x.Course.FullName,
                    CurriculumId = x.SectionId.HasValue
                    ? x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.CurriculumId).FirstOrDefault() :
                    x.Course.AcademicYearCourses.Select(y => y.CurriculumId).FirstOrDefault(),
                    Curriculum = x.SectionId.HasValue
                    ? x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Code).FirstOrDefault() :
                    x.Course.AcademicYearCourses.Select(y => y.Curriculum.Code).FirstOrDefault(),
                    Section = x.SectionId.HasValue ? x.Section.Code : "CURSO DIRIGIDO",
                    LastGenerated = x.LastReportGeneratedDate.ToLocalDateTimeFormat(),
                    Status = ConstantHelpers.Intranet.EvaluationReport.NAMES[x.Status],
                    Code = x.Code,
                    Career = x.SectionId.HasValue ? x.Section.CourseTerm.Course.Career.Name : x.Course.Career.Name,
                    CareerId = x.SectionId.HasValue ? x.Section.CourseTerm.Course.CareerId : x.Course.CareerId,
                    CreatedAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    Teacher = x.SectionId.HasValue ?
                    string.Join(", ", x.Section.TeacherSections.Select(t => t.Teacher.User.FullName).ToArray())
                    : x.Teacher.User.FullName,
                    ReceptionDate = x.Status == ConstantHelpers.Intranet.EvaluationReport.RECEIVED ? x.ReceptionDate.ToLocalDateTimeFormat() : "-"
                })
                .ToArrayAsync();

            return data;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<EvaluationReport> GetEvaluationReportByFilters(Guid? sectionId, Guid? courseId, Guid? termId, byte type)
        {
            var query = _context.EvaluationReports.AsQueryable();

            if (sectionId.HasValue)
                query = query.Where(x => x.SectionId == sectionId);

            if (courseId.HasValue)
                query = query.Where(x => x.CourseId == courseId);

            if (termId.HasValue)
                query = query.Where(x => x.TermId == termId);

            query = query.Where(x => x.Type == type);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> GetNumberByFilter(byte type)
            => await _context.EvaluationReports.Where(x => x.Type == type && x.Term.Year == DateTime.UtcNow.Year).CountAsync();

        public async Task<int> GetMaxNumber(Guid termId)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();
            var result = await _context.EvaluationReports.Where(x => x.Term.Year == term.Year).OrderByDescending(x => x.Number).Select(x => x.Number).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<EvaluationReport>> GetEvaluationReportsByCode(string code, Guid termId)
        {
            var evaluation = await _context.EvaluationReports.Where(x => (x.TermId == termId || x.Section.CourseTerm.TermId == termId) && x.Code.Trim().ToLower() == code.Trim().ToLower()).ToListAsync();
            return evaluation;
        }

        public async Task<EvaluationReportInformationTemplate> GetEvaluationReportInformation(Guid sectionId, int? code = null, string issueDate = null, string receptionDate = null, bool isRegister = false)
        {
            var evaluationReport = await _context.EvaluationReports.Where(x => x.SectionId == sectionId).FirstOrDefaultAsync();

            var section = await _context.Sections.Where(x => x.Id == sectionId)
                .Select(x => new
                {
                    x.Id,
                    x.IsDirectedCourse,
                    x.CourseTerm.CourseId,
                    x.CourseTerm.TermId,
                    x.CourseTermId
                })
                .FirstOrDefaultAsync();

            #region Creacion acta

            if (evaluationReport == null)
            {
                evaluationReport = new EvaluationReport
                {
                    PrintQuantity = 1,
                    LastReportGeneratedDate = DateTime.UtcNow,
                    SectionId = sectionId,
                    CourseId = section.CourseId,
                    Type = section.IsDirectedCourse ? ConstantHelpers.Intranet.EvaluationReportType.DIRECTED_COURSE : ConstantHelpers.Intranet.EvaluationReportType.REGULAR,
                    Status = CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.GENERATED,
                    TermId = section.TermId
                };

                if (code.HasValue && code != 0)
                {
                    evaluationReport.Number = code.Value;
                    evaluationReport.Code = $"{code.Value:000000}";
                }
                else
                {
                    var evaluationReportNumber = await GetMaxNumber(section.TermId);

                    evaluationReport.Number = evaluationReportNumber + 1;
                    evaluationReport.Code = $"{(evaluationReportNumber + 1):000000}";
                }

                if (!string.IsNullOrEmpty(receptionDate))
                    evaluationReport.ReceptionDate = ConvertHelpers.DatepickerToUtcDateTime(receptionDate);

                if (!string.IsNullOrEmpty(issueDate))
                    evaluationReport.CreatedAt = ConvertHelpers.DatepickerToUtcDateTime(issueDate);

                if (!isRegister)
                {
                    await _context.EvaluationReports.AddAsync(evaluationReport);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                if (!isRegister)
                {
                    evaluationReport.PrintQuantity = evaluationReport.PrintQuantity is null ? 1 : evaluationReport.PrintQuantity + 1;
                }
            }

            if (!evaluationReport.CreatedAt.HasValue)
                evaluationReport.CreatedAt = DateTime.UtcNow;

            #endregion

            #region Datos de configuración

            var confiHeader = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER).FirstOrDefaultAsync();
            var confiSubheader = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_SUBHEADER).FirstOrDefaultAsync();
            var confiEvaluationsByUnits = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT).FirstOrDefaultAsync();
            var confiGradeRecoveryModality = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_MODALITY).FirstOrDefaultAsync();
            var confiDateFormat = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE).FirstOrDefaultAsync();
            var confiEvaluationsBySection = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_SECTION).FirstOrDefaultAsync();

            if (confiEvaluationsByUnits is null)
            {
                confiEvaluationsByUnits = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT,
                    Value = ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT]
                };
            }

            if (confiSubheader is null)
            {
                confiSubheader = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_SUBHEADER,
                    Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_SUBHEADER]
                };
            }

            if (confiHeader is null)
            {
                confiHeader = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER,
                    Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER]
                };
            }

            if (confiGradeRecoveryModality is null)
            {
                confiGradeRecoveryModality = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_MODALITY,
                    Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_MODALITY]
                };
            }

            if (confiDateFormat is null)
            {
                confiDateFormat = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE,
                    Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE]
                };
            }

            if(confiEvaluationsBySection is null)
            {
                confiEvaluationsBySection = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_SECTION,
                    Value = ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_SECTION]
                };
            }

            var confiDateFormatValue = Convert.ToByte(confiDateFormat.Value);

            #endregion

            #region Modelo Base

            var model = await _context.Sections.Where(x => x.Id == sectionId)
                .Select(x => new EvaluationReportInformationTemplate
                {
                    Header = confiHeader.Value,
                    SubHeader = confiSubheader.Value,
                    BasicInformation = new EvaluationReportBasicInformationTemplate
                    {
                        Id = evaluationReport.Id,
                        Code = evaluationReport.Code,
                        CreatedAt = evaluationReport.CreatedAt,
                        ReceptionDate = evaluationReport.ReceptionDate,
                        Status = evaluationReport.Status,
                        Type = evaluationReport.Type,
                        LastGradeRegistration = evaluationReport.LastGradePublishedDate
                    },
                    Term = new EvaluationReportTermTemplate
                    {
                        Name = x.CourseTerm.Term.Name,
                        IsSummer = x.CourseTerm.Term.IsSummer,
                        MinGrade = x.CourseTerm.Term.MinGrade,
                        Status = x.CourseTerm.Term.Status,
                        EndDate = x.CourseTerm.Term.EndDate.ToDefaultTimeZone()
                    },
                    Course = new EvaluationReportCourseTemplate
                    {
                        Career = new EvaluationReportCareerTemplate
                        {
                            Code = x.CourseTerm.Course.Career.Code ?? "-",
                            Name = x.CourseTerm.Course.Career.Name ?? "-",
                            Faculty = x.CourseTerm.Course.Career.Faculty.Name ?? "-",
                            CareerDirector = x.CourseTerm.Course.Career.CareerDirector.FullName
                        },
                        CampusName = x.ClassSchedules.Select(y => y.Classroom.Building.Campus.Name).FirstOrDefault(),
                        AcademicYear = x.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault(),
                        Credits = x.CourseTerm.Course.Credits.ToString("0.00"),
                        PracticalHours = x.CourseTerm.Course.PracticalHours,
                        TheoreticalHours = x.CourseTerm.Course.TheoreticalHours,
                        Code = x.CourseTerm.Course.Code,
                        Name = x.CourseTerm.Course.Name,
                        IsElective = x.CourseTerm.Course.AcademicYearCourses.Select(y => y.IsElective).FirstOrDefault(),
                        Curriculum = x.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Code).FirstOrDefault(),
                        Section = new EvaluationReportSectionTemplate
                        {
                            Code = x.Code,
                            AcademicDepartment = x.TeacherSections.Where(y => y.IsPrincipal).Count() > 1 ? "CARGA COMPARTIDA" : x.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.AcademicDepartment.Name).FirstOrDefault(),
                            Teacher = x.TeacherSections.Where(y => y.IsPrincipal).Count() > 1 ? "CARGA COMPARTIDA" : x.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.FullName).FirstOrDefault(),
                            Students = x.StudentSections.OrderBy(y => y.Student.User.FullName).Select(y => new EvaluationReportStudent
                            {
                                Id = y.StudentId,
                                StudentSectionId = y.Id,
                                SectionGroupId = y.SectionGroupId,
                                FullName = y.Student.User.FullName,
                                UserName = y.Student.User.UserName,
                                FinalGrade = y.FinalGrade,
                                Try = y.Try,
                                Status = y.Status,
                                StatusText = ConstantHelpers.STUDENT_SECTION_STATES.VALUES.ContainsKey(y.Status) ? ConstantHelpers.STUDENT_SECTION_STATES.VALUES[y.Status] : "",
                                Averages = new List<EvaluationReportPartialAverageTemplate>()
                            }).ToList()
                        }
                    }
                })
                .FirstOrDefaultAsync();

            #endregion

            #region Información extra NOTAS, UNIDADES, EVALUACIONES, RECUPERACIONES DE NOTA, EXAMENES SUSTI, ASISTENCIA

            var evaluationsByUnits = Convert.ToBoolean(confiEvaluationsByUnits.Value);
            var evaluationsBySection = Convert.ToBoolean(confiEvaluationsBySection.Value);

            var grades = await _context.Grades.Where(x => x.StudentSection.SectionId == sectionId)
                .Select(x => new
                {
                    x.StudentSectionId,
                    x.Value,
                    x.CreatedAt,
                    x.EvaluationId,
                    x.Evaluation.CourseUnitId,
                    x.Id
                })
                .ToListAsync();

            var courseUnits = await _context.CourseUnits.Where(x => x.CourseSyllabus.CourseId == section.CourseId && x.CourseSyllabus.TermId == section.TermId)
                .OrderBy(x => x.Number)
                .Select(x => new
                {
                    x.Id,
                    x.Number,
                    x.Name,
                    Evaluations = x.Evaluations
                    .Select(y => new
                    {
                        y.Id,
                        Percentage = evaluationsBySection ? _context.SectionEvaluations.Where(z=>z.SectionId == section.Id && z.EvaluationId == y.Id).Select(z=>z.Percentage).FirstOrDefault() : y.Percentage
                    })
                    .ToList()
                })
                .ToListAsync();

            var evaluations = await _context.Evaluations
                .Where(x => x.CourseTerm.TermId == section.TermId && x.CourseTerm.CourseId == section.CourseId)
                .OrderBy(x => x.Week)
                .ThenBy(x => x.Percentage)
                .Select(x => new
                {
                    x.Id,
                    Percentage = evaluationsBySection ? _context.SectionEvaluations.Where(y=>y.SectionId == section.Id && y.EvaluationId == x.Id).Select(y=>y.Percentage).FirstOrDefault() : x.Percentage,
                    x.Name,
                    x.Week,
                    x.CourseUnitId,
                    EvaluationTypeName = x.EvaluationType.Name
                })
                .ToListAsync();

            var gradesRecoveriesQuery = _context.GradeRecoveries
                .Where(x => x.GradeRecoveryExam.Status == ConstantHelpers.GRADE_RECOVERY_EXAM_STATUS.EXECUTED && x.GradeRecoveryExam.SectionId == sectionId);

            if (Convert.ToByte(confiGradeRecoveryModality.Value) == ConstantHelpers.GRADE_RECOVERY_EXAM_MODALITY.HIGHEST_GRADE)
                gradesRecoveriesQuery = gradesRecoveriesQuery.Where(x => x.ExamScore > x.PrevFinalScore);

            var gradesRecoveries = await gradesRecoveriesQuery
                .Select(x => new
                {
                    x.StudentId,
                    x.GradeId,
                    x.ExamScore,
                    x.PrevFinalScore
                })
                .ToListAsync();

            var sustituteExams = await _context.SubstituteExams.Where(x => (x.SectionId == sectionId || x.CourseTermId == section.CourseTermId) && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED).ToListAsync();
            var classStudents = await _context.ClassStudents.Where(x => x.Class.ClassSchedule.SectionId == sectionId)
                .Select(x => new
                {
                    x.Class.ClassSchedule.SectionGroupId,
                    x.StudentId,
                    x.IsAbsent
                })
                .ToListAsync();

            var classesBySubGroup = await _context.Classes.Where(x => x.ClassSchedule.SectionId == sectionId)
                .GroupBy(x => x.ClassSchedule.SectionGroupId)
                .Select(x => new
                {
                    x.Key,
                    count = x.Count()
                })
                .ToListAsync();

            var maxAbsencesPercentage = await _context.Sections.Where(x => x.Id == sectionId).Select(x => x.CourseTerm.Term.AbsencePercentage).FirstOrDefaultAsync();

            if (!evaluationReport.LastGradePublishedDate.HasValue)
            {
                evaluationReport.LastGradePublishedDate = grades.Select(x => x.CreatedAt).OrderByDescending(x => x).FirstOrDefault();
                model.BasicInformation.LastGradeRegistration = evaluationReport.LastGradePublishedDate;
                await _context.SaveChangesAsync();
            }

            model.Course.Evaluations = evaluations.Select(x => new EvaluationReportEvaluationTemplate
            {
                Id = x.Id,
                Name = x.Name,
                Week = x.Week,
                Percentage = x.Percentage,
                EvaluationType = x.EvaluationTypeName,
                CourseUnitId = x.CourseUnitId,
            }).ToList();
            model.Course.CourseUnits = courseUnits.Select(x => new EvaluationReportCourseUnitTemplate
            {
                Id = x.Id,
                Name = x.Name,
                Number = x.Number
            }).ToList();
            model.Course.EvaluationByUnits = evaluationsByUnits;
            model.Course.PartialAveragesCount = evaluationsByUnits ? courseUnits.Count() : evaluations.Count();

            switch (confiDateFormatValue)
            {
                case ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.CreatedAt:
                    model.BasicInformation.DateByConfiguration = evaluationReport.CreatedAt;
                    break;
                case ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.ReceptionDate:
                    model.BasicInformation.DateByConfiguration = evaluationReport.ReceptionDate;
                    break;
                case ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.LastGradePublished:
                    model.BasicInformation.DateByConfiguration = evaluationReport.LastGradePublishedDate;
                    break;
            }

            #endregion

            #region Formula

            var formula = string.Empty;

            if (evaluationsByUnits)
            {
                for (int i = 1; i <= model.Course.PartialAveragesCount; i++)
                {
                    if (i == 1)
                    {
                        formula += $"{i}PP";
                    }
                    else
                    {
                        formula += $" + {i}PP";
                    }
                }

                model.Course.Formula = $"({formula})/{model.Course.PartialAveragesCount}";
            }
            else
            {
                for (int i = 1; i <= evaluations.Count(); i++)
                {
                    var name = (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAH || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAB) ? evaluations[(i - 1)].Name : $"{i}PP";

                    if (i == 1)
                    {
                        formula += $"({name} * {evaluations[(i - 1)].Percentage})";
                    }
                    else
                    {
                        formula += $"+ ({name} * {evaluations[(i - 1)].Percentage})";
                    }
                }

                model.Course.Formula = $"({formula})/{evaluations.Sum(x => x.Percentage)}";
            }

            #endregion

            #region Carga de Estudiantes

            var academicHistories = await _context.AcademicHistories.Where(x => x.SectionId == sectionId).ToListAsync();

            if (model.Term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
            {
                model.Course.Section.Students.ForEach(x =>
                {
                    x.FinalGrade = academicHistories.Where(y => y.StudentId == x.Id).Select(y => y.Grade).FirstOrDefault();
                });
            }

            foreach (var student in model.Course.Section.Students)
            {
                var classes = classesBySubGroup.Where(x => x.Key is null || x.Key == student.SectionGroupId).Sum(x => x.count);

                if (classes > 0)
                {
                    var absences = classStudents.Where(x => x.StudentId == student.Id && x.IsAbsent && (!x.SectionGroupId.HasValue || x.SectionGroupId == student.SectionGroupId)).Count();
                    var absencesPercentage = ((decimal)absences / (decimal)classes) * 100M;
                    student.AttendancePercentage = (int)(100M - absencesPercentage);
                    student.DPI = absencesPercentage > (decimal)maxAbsencesPercentage;
                }

                var gradesByStudent = grades.Where(x => x.StudentSectionId == student.StudentSectionId).ToList();

                var susti = sustituteExams.Where(x => x.StudentId == student.Id).FirstOrDefault();

                student.TryText = ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES.ContainsKey(student.Try) ? ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES[student.Try] : string.Empty;

                if (model.Term.IsSummer)
                {
                    student.TryText = "NIVELACIÓN";
                }
                else if (model.BasicInformation.Type == ConstantHelpers.Intranet.EvaluationReportType.DIRECTED_COURSE)
                {
                    student.TryText = "DIRIGIDO";
                }
                else if (model.BasicInformation.Type == ConstantHelpers.Intranet.EvaluationReportType.EXTRAORDINARY_EVALUATION)
                {
                    student.TryText = "EXTRAORDINARIO";
                }

                if (susti != null)
                {
                    student.HasSusti = susti.ExamScore > susti.PrevFinalScore;
                    student.GradeBeforeSusti = susti.PrevFinalScore;
                }

                student.Approved = model.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE ? (!student.DPI && student.FinalGrade >= model.Term.MinGrade) : student.Status == ConstantHelpers.STUDENT_SECTION_STATES.APPROVED;
                student.HasGradeRecovery = gradesRecoveries.Any(y => y.StudentId == student.Id);
                student.GradeRecoveryValue = student.HasGradeRecovery ? gradesRecoveries.Where(x => x.StudentId == student.Id).Select(x => x.ExamScore).FirstOrDefault() : null;
                student.GradeBeforeGradeRecovery = student.HasGradeRecovery ? gradesRecoveries.Where(x => x.StudentId == student.Id).Select(x => x.PrevFinalScore).FirstOrDefault() : null;
                student.GradeIdUpdatedByGradeRecovery = student.HasGradeRecovery ? gradesRecoveries.Where(x => x.StudentId == student.Id).Select(x => x.GradeId).FirstOrDefault() : null;
                student.Grades = gradesByStudent.Select(x => new EvaluationReportGradeTempate
                {
                    EvaluationId = x.EvaluationId,
                    Id = x.Id,
                    StudentSectionId = x.StudentSectionId,
                    Value = x.Value
                }).ToList();
                student.HasAllGradesPublished = student.Grades.Where(x => x.EvaluationId.HasValue).GroupBy(x => x.EvaluationId).Select(x => x.Key.Value).ToList().Contains(evaluations.Select(x => x.Id).ToList());

                if (evaluationsByUnits)
                {
                    for (int i = 0; i < courseUnits.Count(); i++)
                    {
                        var studentAverage = new EvaluationReportPartialAverageTemplate
                        {
                            Id = courseUnits[i].Id,
                            Number = courseUnits[i].Number,
                        };

                        var evaluationCalc = 0M;

                        if (gradesByStudent.Any(y => y.CourseUnitId == courseUnits[i].Id))
                        {

                            var evaluationTotalPercentage = courseUnits[i].Evaluations.Sum(x => x.Percentage) * 1.0M;

                            foreach (var evaluation in courseUnits[i].Evaluations)
                            {
                                var gradeByEvaluation = gradesByStudent.Where(x => x.EvaluationId == evaluation.Id).Select(x => x.Value).FirstOrDefault();
                                evaluationCalc += gradeByEvaluation * evaluation.Percentage / evaluationTotalPercentage;
                            }

                            //Promedio parcial redondeado a 2 decimales para algunas univ.
                            if (
                                ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNIFSLB ||
                                ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNF ||
                                ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNJBG
                                )
                            {
                                evaluationCalc = Math.Round(evaluationCalc, 2, MidpointRounding.AwayFromZero);
                                studentAverage.Average = evaluationCalc;
                            }
                            //Promedio parcial redondeado a entero por defecto
                            else
                            {
                                evaluationCalc = Math.Round(evaluationCalc, 0, MidpointRounding.AwayFromZero);
                                studentAverage.Average = (int)evaluationCalc;
                            }

                            studentAverage.Approved = studentAverage.Average >= model.Term.MinGrade;
                        }

                        student.Averages.Add(studentAverage);
                    }
                }
                else
                {
                    for (int i = 0; i < evaluations.Count; i++)
                    {
                        var average = gradesByStudent.Where(x => x.EvaluationId == evaluations[i].Id).FirstOrDefault();

                        var averageTemplate = new EvaluationReportPartialAverageTemplate
                        {
                            Id = evaluations[i].Id,
                            Number = (i + 1)
                        };

                        if (average != null)
                        {
                            averageTemplate.Average = average.Value;
                            averageTemplate.GradeId = average?.Id;
                            averageTemplate.Approved = average?.Value >= model.Term.MinGrade;
                        }
                        else
                        {
                            if (student.DPI)
                                averageTemplate.Average = 0;
                        }

                        if (student.HasGradeRecovery)
                        {
                            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAH)
                            {
                                var gradeRecovery = gradesRecoveries.Where(x => x.GradeId == average.Id).FirstOrDefault();
                                if (gradeRecovery != null && gradeRecovery.PrevFinalScore.HasValue)
                                {
                                    averageTemplate.Average = (int)Math.Round(gradeRecovery.PrevFinalScore.Value, 0, MidpointRounding.AwayFromZero);
                                    averageTemplate.Approved = gradeRecovery.PrevFinalScore >= model.Term.MinGrade;
                                }
                            }
                        }

                        student.Averages.Add(averageTemplate);
                    }
                }
            }

            if (!isRegister)
            {
                academicHistories.ForEach(x => x.EvaluationReportId = evaluationReport.Id);
                await _context.SaveChangesAsync();
            }

            #endregion

            return model;

        }

        public async Task<EvaluationReportInformationTemplate> GetEvaluationReportDeferredExamInformation(Guid deferredExamId)
        {
            var evaluationReport = await _context.EvaluationReports.Where(x => x.Type == ConstantHelpers.Intranet.EvaluationReportType.DEFERRED && x.EntityId == deferredExamId).FirstOrDefaultAsync();
            var deferredExam = await _context.DeferredExams.Where(x => x.Id == deferredExamId)
                .Select(x => new
                {
                    x.CreatedAt,
                    x.Id,
                    x.SectionId,
                    x.Section.CourseTerm.TermId,
                    Campus = x.Classroom.Building.Campus.Name
                })
                .FirstOrDefaultAsync();

            #region Creacion acta

            if (evaluationReport == null)
            {
                var evaluationReportNumber = await GetMaxNumber(deferredExam.TermId);

                evaluationReport = new EvaluationReport
                {
                    PrintQuantity = 1,
                    LastReportGeneratedDate = DateTime.UtcNow,
                    Type = ConstantHelpers.Intranet.EvaluationReportType.DEFERRED,
                    SectionId = deferredExam.SectionId,
                    Status = CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.GENERATED,
                    TermId = deferredExam.TermId,
                    Number = (evaluationReportNumber + 1),
                    Code = $"{(evaluationReportNumber + 1):000000}",
                    EntityId = deferredExam.Id
                };

                await _context.EvaluationReports.AddAsync(evaluationReport);
                await _context.SaveChangesAsync();
            }
            else
            {
                evaluationReport.PrintQuantity = evaluationReport.PrintQuantity is null ? 1 : evaluationReport.PrintQuantity + 1;
            }

            if (!evaluationReport.CreatedAt.HasValue)
                evaluationReport.CreatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            #endregion

            var confiHeader = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER).FirstOrDefaultAsync();
            var confiSubheader = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_SUBHEADER).FirstOrDefaultAsync();
            var confiDateFormat = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE).FirstOrDefaultAsync();

            if (confiSubheader is null)
            {
                confiSubheader = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_SUBHEADER,
                    Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_SUBHEADER]
                };
            }

            if (confiHeader is null)
            {
                confiHeader = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER,
                    Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER]
                };
            }

            if (confiDateFormat is null)
            {
                confiDateFormat = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE,
                    Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE]
                };
            }

            var confiDateFormatValue = Convert.ToByte(confiDateFormat.Value);

            #region Modelo Base

            var model = await _context.DeferredExams.Where(x => x.Id == deferredExamId)
                .Select(x => new EvaluationReportInformationTemplate
                {
                    Header = confiHeader.Value,
                    SubHeader = confiSubheader.Value,
                    BasicInformation = new EvaluationReportBasicInformationTemplate
                    {
                        Id = evaluationReport.Id,
                        Code = evaluationReport.Code,
                        CreatedAt = evaluationReport.CreatedAt,
                        ReceptionDate = evaluationReport.ReceptionDate,
                        Status = evaluationReport.Status,
                        Type = evaluationReport.Type,
                        LastGradeRegistration = evaluationReport.LastGradePublishedDate
                    },
                    Term = new EvaluationReportTermTemplate
                    {
                        Name = x.Section.CourseTerm.Term.Name,
                        IsSummer = x.Section.CourseTerm.Term.IsSummer,
                        MinGrade = x.Section.CourseTerm.Term.MinGrade,
                        Status = x.Section.CourseTerm.Term.Status,
                        EndDate = x.Section.CourseTerm.Term.EndDate.ToDefaultTimeZone()
                    },
                    Course = new EvaluationReportCourseTemplate
                    {
                        CampusName = deferredExam.Campus,
                        Career = new EvaluationReportCareerTemplate
                        {
                            Code = x.Section.CourseTerm.Course.Career.Code ?? "-",
                            Name = x.Section.CourseTerm.Course.Career.Name ?? "-",
                            Faculty = x.Section.CourseTerm.Course.Career.Faculty.Name ?? "-",
                            CareerDirector = x.Section.CourseTerm.Course.Career.CareerDirector.FullName
                        },
                        AcademicYear = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault(),
                        Credits = x.Section.CourseTerm.Course.Credits.ToString("0.00"),
                        PracticalHours = x.Section.CourseTerm.Course.PracticalHours,
                        TheoreticalHours = x.Section.CourseTerm.Course.TheoreticalHours,
                        Code = x.Section.CourseTerm.Course.Code,
                        Name = x.Section.CourseTerm.Course.Name,
                        IsElective = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.IsElective).FirstOrDefault(),
                        Curriculum = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Code).FirstOrDefault(),
                        Section = new EvaluationReportSectionTemplate
                        {
                            Code = "APLAZADO",
                            AcademicDepartment = x.Section.TeacherSections.Where(y => y.IsPrincipal).Count() > 1 ? "CARGA COMPARTIDA" : x.Section.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.AcademicDepartment.Name).FirstOrDefault(),
                            //AcademicDepartment = x.AssignedTeacher.AcademicDepartment.Name,
                            Teacher = x.Section.TeacherSections.Where(y => y.IsPrincipal).Count() > 1 ? "CARGA COMPARTIDA" : x.Section.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.FullName).FirstOrDefault(),
                            //Teacher = x.AssignedTeacher.User.FullName
                        }
                    }
                })
                .FirstOrDefaultAsync();

            model.Course.Section.Students = await _context.DeferredExamStudents.Where(x => x.DeferredExamId == deferredExamId && x.Grade.HasValue)
                .Select(x => new EvaluationReportStudent
                {
                    Id = x.StudentId,
                    FinalGradePublishedDate = x.GradePublicationDate,
                    FullName = x.Student.User.FullName,
                    HasAllGradesPublished = true,
                    UserName = x.Student.User.UserName,
                    FinalGrade = x.Grade.Value,
                    Try = 1,
                    TryText = "APLAZADO",
                    Status = x.Grade.Value >= model.Term.MinGrade ? ConstantHelpers.STUDENT_SECTION_STATES.APPROVED : ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED,
                    Approved = x.Grade.Value >= model.Term.MinGrade,
                    Averages = new List<EvaluationReportPartialAverageTemplate>()
                })
                .ToListAsync();

            if (!evaluationReport.LastGradePublishedDate.HasValue)
            {
                evaluationReport.LastGradePublishedDate = model.Course.Section.Students.Select(y => y.FinalGradePublishedDate).OrderByDescending(y => y).FirstOrDefault();
                model.BasicInformation.LastGradeRegistration = evaluationReport.LastGradePublishedDate;
                await _context.SaveChangesAsync();
            }

            switch (confiDateFormatValue)
            {
                case ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.CreatedAt:
                    model.BasicInformation.DateByConfiguration = evaluationReport.CreatedAt;
                    break;
                case ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.ReceptionDate:
                    model.BasicInformation.DateByConfiguration = evaluationReport.ReceptionDate;
                    break;
                case ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.LastGradePublished:
                    model.BasicInformation.DateByConfiguration = evaluationReport.LastGradePublishedDate;
                    break;
            }

            #endregion

            return model;
        }

        public async Task<EvaluationReportInformationTemplate> GetEvaluationReportCorrectionExamInformation(Guid correctionExamId)
        {
            var evaluationReport = await _context.EvaluationReports.Where(x => x.Type == ConstantHelpers.Intranet.EvaluationReportType.CORRECTION_EXAM && x.EntityId == correctionExamId).FirstOrDefaultAsync();
            var correctionExam = await _context.CorrectionExams.Where(x => x.Id == correctionExamId)
                .Select(x => new
                {
                    x.CreatedAt,
                    x.Id,
                    x.SectionId,
                    x.Section.CourseTerm.TermId,
                    Campus = x.Classroom.Building.Campus.Name
                })
                .FirstOrDefaultAsync();

            #region Creacion acta

            if (evaluationReport == null)
            {
                var evaluationReportNumber = await GetMaxNumber(correctionExam.TermId);

                evaluationReport = new EvaluationReport
                {
                    PrintQuantity = 1,
                    LastReportGeneratedDate = DateTime.UtcNow,
                    Type = ConstantHelpers.Intranet.EvaluationReportType.CORRECTION_EXAM,
                    SectionId = correctionExam.SectionId,
                    Status = CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.GENERATED,
                    TermId = correctionExam.TermId,
                    Number = (evaluationReportNumber + 1),
                    Code = $"{(evaluationReportNumber + 1):000000}",
                    EntityId = correctionExam.Id
                };

                await _context.EvaluationReports.AddAsync(evaluationReport);
                await _context.SaveChangesAsync();
            }
            else
            {
                evaluationReport.PrintQuantity = evaluationReport.PrintQuantity is null ? 1 : evaluationReport.PrintQuantity + 1;
            }

            if (!evaluationReport.CreatedAt.HasValue)
                evaluationReport.CreatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            #endregion

            var confiHeader = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER).FirstOrDefaultAsync();
            var confiSubheader = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_SUBHEADER).FirstOrDefaultAsync();
            var confiDateFormat = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE).FirstOrDefaultAsync();

            if (confiSubheader is null)
            {
                confiSubheader = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_SUBHEADER,
                    Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_SUBHEADER]
                };
            }

            if (confiHeader is null)
            {
                confiHeader = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER,
                    Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER]
                };
            }

            if (confiDateFormat is null)
            {
                confiDateFormat = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE,
                    Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE]
                };
            }

            var confiDateFormatValue = Convert.ToByte(confiDateFormat.Value);

            #region Modelo Base

            var model = await _context.CorrectionExams.Where(x => x.Id == correctionExamId)
                .Select(x => new EvaluationReportInformationTemplate
                {
                    Header = confiHeader.Value,
                    SubHeader = confiSubheader.Value,
                    BasicInformation = new EvaluationReportBasicInformationTemplate
                    {
                        Id = evaluationReport.Id,
                        Code = evaluationReport.Code,
                        CreatedAt = evaluationReport.CreatedAt,
                        ReceptionDate = evaluationReport.ReceptionDate,
                        Status = evaluationReport.Status,
                        Type = evaluationReport.Type,
                        LastGradeRegistration = evaluationReport.LastGradePublishedDate
                    },
                    Term = new EvaluationReportTermTemplate
                    {
                        Name = x.Section.CourseTerm.Term.Name,
                        IsSummer = x.Section.CourseTerm.Term.IsSummer,
                        MinGrade = x.Section.CourseTerm.Term.MinGrade,
                        Status = x.Section.CourseTerm.Term.Status,
                        EndDate = x.Section.CourseTerm.Term.EndDate.ToDefaultTimeZone()
                    },
                    Course = new EvaluationReportCourseTemplate
                    {
                        CampusName = correctionExam.Campus,
                        Career = new EvaluationReportCareerTemplate
                        {
                            Code = x.Section.CourseTerm.Course.Career.Code ?? "-",
                            Name = x.Section.CourseTerm.Course.Career.Name ?? "-",
                            Faculty = x.Section.CourseTerm.Course.Career.Faculty.Name ?? "-",
                            CareerDirector = x.Section.CourseTerm.Course.Career.CareerDirector.FullName
                        },
                        AcademicYear = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault(),
                        Credits = x.Section.CourseTerm.Course.Credits.ToString("0.00"),
                        PracticalHours = x.Section.CourseTerm.Course.PracticalHours,
                        TheoreticalHours = x.Section.CourseTerm.Course.TheoreticalHours,
                        Code = x.Section.CourseTerm.Course.Code,
                        Name = x.Section.CourseTerm.Course.Name,
                        IsElective = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.IsElective).FirstOrDefault(),
                        Curriculum = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Code).FirstOrDefault(),
                        Section = new EvaluationReportSectionTemplate
                        {
                            Code = "-",
                            AcademicDepartment = x.Teacher.AcademicDepartment.Name,
                            Teacher = x.Teacher.User.FullName
                        }
                    }
                })
                .FirstOrDefaultAsync();

            model.Course.Section.Students = await _context.CorrectionExamStudents.Where(x => x.CorrectionExamId == correctionExamId && x.Grade.HasValue)
                .Select(x => new EvaluationReportStudent
                {
                    FinalGradePublishedDate = x.GradePublicationDate,
                    Id = x.StudentId,
                    FullName = x.Student.User.FullName,
                    HasAllGradesPublished = true,
                    UserName = x.Student.User.UserName,
                    FinalGrade = x.Grade.Value,
                    Try = 1,
                    TryText = "SUBSANACIÓN",
                    Status = x.Grade.Value >= model.Term.MinGrade ? ConstantHelpers.STUDENT_SECTION_STATES.APPROVED : ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED,
                    Approved = x.Grade.Value >= model.Term.MinGrade,
                    Averages = new List<EvaluationReportPartialAverageTemplate>()
                })
                .ToListAsync();

            if (!evaluationReport.LastGradePublishedDate.HasValue)
            {
                evaluationReport.LastGradePublishedDate = model.Course.Section.Students.Select(y => y.FinalGradePublishedDate).OrderByDescending(y => y).FirstOrDefault();
                model.BasicInformation.LastGradeRegistration = evaluationReport.LastGradePublishedDate;
                await _context.SaveChangesAsync();
            }

            switch (confiDateFormatValue)
            {
                case ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.CreatedAt:
                    model.BasicInformation.DateByConfiguration = evaluationReport.CreatedAt;
                    break;
                case ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.ReceptionDate:
                    model.BasicInformation.DateByConfiguration = evaluationReport.ReceptionDate;
                    break;
                case ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.LastGradePublished:
                    model.BasicInformation.DateByConfiguration = evaluationReport.LastGradePublishedDate;
                    break;
            }

            #endregion

            return model;
        }

        public async Task<EvaluationReportInformationTemplate> GetEvaluationReportExtraordinaryEvaluationInformation(Guid extraordinaryEvaluationId)
        {
            var evaluation = await _context.ExtraordinaryEvaluations.Where(x => x.Id == extraordinaryEvaluationId)
                .Select(x => new
                {
                    x.Id,
                    Teacher = x.Teacher.User.FullName,
                    x.CourseId,
                    x.TermId,
                    CourseCode = x.Course.Code,
                    CourseName = x.Course.Name,
                    x.Type
                })
                .FirstOrDefaultAsync();

            var evaluationReport = await _context.EvaluationReports.Where(x => x.Type == ConstantHelpers.Intranet.EvaluationReportType.EXTRAORDINARY_EVALUATION && x.EntityId == evaluation.Id).FirstOrDefaultAsync();

            #region Creacion acta

            if (evaluationReport == null)
            {
                var evaluationReportNumber = await GetMaxNumber(evaluation.TermId);

                evaluationReport = new EvaluationReport
                {
                    PrintQuantity = 1,
                    LastReportGeneratedDate = DateTime.UtcNow,
                    CourseId = evaluation.CourseId,
                    TermId = evaluation.TermId,
                    Type = CORE.Helpers.ConstantHelpers.Intranet.EvaluationReportType.EXTRAORDINARY_EVALUATION,
                    Status = CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.GENERATED,
                    Number = evaluationReportNumber + 1,
                    Code = $"{(evaluationReportNumber + 1):000000}",
                    EntityId = evaluation.Id
                };

                await _context.EvaluationReports.AddAsync(evaluationReport);
                await _context.SaveChangesAsync();
            }
            else
            {
                evaluationReport.PrintQuantity = evaluationReport.PrintQuantity is null ? 1 : evaluationReport.PrintQuantity + 1;
            }

            if (!evaluationReport.CreatedAt.HasValue)
                evaluationReport.CreatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            #endregion

            var confiHeader = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER).FirstOrDefaultAsync();
            var confiSubheader = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_SUBHEADER).FirstOrDefaultAsync();
            var confiDateFormat = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE).FirstOrDefaultAsync();

            if (confiSubheader is null)
            {
                confiSubheader = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_SUBHEADER,
                    Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_SUBHEADER]
                };
            }

            if (confiHeader is null)
            {
                confiHeader = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER,
                    Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER]
                };
            }

            if (confiDateFormat is null)
            {
                confiDateFormat = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE,
                    Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE]
                };
            }

            var confiDateFormatValue = Convert.ToByte(confiDateFormat.Value);

            #region Modelo Base

            var model = await _context.ExtraordinaryEvaluations.Where(x => x.Id == extraordinaryEvaluationId)
                .Select(x => new EvaluationReportInformationTemplate
                {
                    Header = confiHeader.Value,
                    SubHeader = confiSubheader.Value,
                    BasicInformation = new EvaluationReportBasicInformationTemplate
                    {
                        Id = evaluationReport.Id,
                        Code = evaluationReport.Code,
                        CreatedAt = evaluationReport.CreatedAt,
                        ReceptionDate = evaluationReport.ReceptionDate,
                        Status = evaluationReport.Status,
                        Type = evaluationReport.Type,
                        ExtraordinaryEvaluationType = evaluation.Type,
                        LastGradeRegistration = evaluationReport.LastGradePublishedDate
                    },
                    Term = new EvaluationReportTermTemplate
                    {
                        Name = x.Term.Name,
                        IsSummer = x.Term.IsSummer,
                        MinGrade = x.Term.MinGrade,
                        Status = x.Term.Status,
                        EndDate = x.Term.EndDate.ToDefaultTimeZone()
                    },
                    Course = new EvaluationReportCourseTemplate
                    {
                        Career = new EvaluationReportCareerTemplate
                        {
                            Code = x.Course.Career.Code ?? "-",
                            Name = x.Course.Career.Name ?? "-",
                            Faculty = x.Course.Career.Faculty.Name ?? "-",
                            CareerDirector = x.Course.Career.CareerDirector.FullName
                        },
                        AcademicYear = x.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault(),
                        Credits = x.Course.Credits.ToString("0.00"),
                        PracticalHours = x.Course.PracticalHours,
                        TheoreticalHours = x.Course.TheoreticalHours,
                        Code = x.Course.Code,
                        Name = x.Course.Name,
                        IsElective = x.Course.AcademicYearCourses.Select(y => y.IsElective).FirstOrDefault(),
                        Curriculum = x.Course.AcademicYearCourses.Select(y => y.Curriculum.Code).FirstOrDefault(),
                        Section = new EvaluationReportSectionTemplate
                        {
                            Code = "EVALUACIÓN EXTRAORDINARIA",
                            AcademicDepartment = x.Teacher.AcademicDepartment.Name,
                            Teacher = x.Teacher.User.FullName
                        }
                    }
                })
                .FirstOrDefaultAsync();

            model.Course.Section.Students = await _context.ExtraordinaryEvaluationStudents.Where(x => x.ExtraordinaryEvaluationId == extraordinaryEvaluationId)
                .Select(x => new EvaluationReportStudent
                {
                    FinalGradePublishedDate = x.GradePublicationDate,
                    Id = x.StudentId,
                    FullName = x.Student.User.FullName,
                    HasAllGradesPublished = true,
                    UserName = x.Student.User.UserName,
                    FinalGrade = (int)Math.Round(x.Grade, 0, MidpointRounding.AwayFromZero),
                    Try = 1,
                    TryText = "EXTRAORDINARIO",
                    Status = x.Grade >= model.Term.MinGrade ? ConstantHelpers.STUDENT_SECTION_STATES.APPROVED : ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED,
                    Approved = x.Grade >= model.Term.MinGrade,
                    Averages = new List<EvaluationReportPartialAverageTemplate>()
                })
                .ToListAsync();

            if (!evaluationReport.LastGradePublishedDate.HasValue)
            {
                evaluationReport.LastGradePublishedDate = model.Course.Section.Students.Select(y => y.FinalGradePublishedDate).OrderByDescending(y => y).FirstOrDefault();
                model.BasicInformation.LastGradeRegistration = evaluationReport.LastGradePublishedDate;
                await _context.SaveChangesAsync();
            }

            switch (confiDateFormatValue)
            {
                case ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.CreatedAt:
                    model.BasicInformation.DateByConfiguration = evaluationReport.CreatedAt;
                    break;
                case ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.ReceptionDate:
                    model.BasicInformation.DateByConfiguration = evaluationReport.ReceptionDate;
                    break;
                case ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.LastGradePublished:
                    model.BasicInformation.DateByConfiguration = evaluationReport.LastGradePublishedDate;
                    break;
            }

            #endregion

            var academicHistories = await _context.ExtraordinaryEvaluationStudents.Where(x => x.ExtraordinaryEvaluationId == extraordinaryEvaluationId).Select(x => x.AcademicHistory).ToListAsync();
            academicHistories.ForEach(x=>x.EvaluationReportId = evaluationReport.Id);
            await _context.SaveChangesAsync();

            return model;
        }
    }
}
