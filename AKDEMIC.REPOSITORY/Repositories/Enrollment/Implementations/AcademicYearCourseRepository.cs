using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.AcademicYearCourse;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicHistory;
using Flurl.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class AcademicYearCourseRepository : Repository<AcademicYearCourse>, IAcademicYearCourseRepository
    {
        public AcademicYearCourseRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<int?> GetLevelByCourseAndCurriculum(Guid courseId, Guid? curriculumId = null)
        {
            var query = _context.AcademicYearCourses
                .Where(x => x.CourseId == courseId)
                .ToList();

            if (curriculumId.HasValue)
                query = query.Where(x => x.CurriculumId == curriculumId.Value).ToList();

            return query.Select(x => x.AcademicYear).FirstOrDefault();
        }

        public async Task<IEnumerable<object>> GetCurriculumCoursesSelect2(Guid curriculumId)
        {
            var query = _context.AcademicYearCourses
                .Where(c => c.CurriculumId == curriculumId)
                .AsNoTracking();

            var results = await query
                .OrderBy(x => x.Course.Code)
                .ThenBy(x => x.Course.Name)
                .Select(c => new
                {
                    id = c.Id,
                    text = c.Course.AcademicProgramId.HasValue ? c.Course.FullName + " - P.A.: " + c.Course.AcademicProgram.Name : c.Course.FullName
                })
                .ToListAsync();

            return results;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCurriculumCoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid id)
        {
            var student = await _context.Students
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.CurriculumId,
                    CareerCode = x.Career.Code,
                    x.AcademicProgramId
                }).FirstOrDefaultAsync();

            var academicHistories = await _context.AcademicHistories
                .Where(x => x.StudentId == id && !x.Withdraw)
                .Include(x => x.Term).Include(x => x.EvaluationReport)
                .OrderBy(x => x.Term.Name)
                .ToListAsync();

            var query = _context.AcademicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId/* && !x.IsElective*/)
                .Include(x => x.Course.AcademicProgram)
                .AsNoTracking();

            var evaluationReportDateConfi = Convert.ToByte(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE));

            ////FILTRO POR PROGRAMA ACADEMICO

            if (student.AcademicProgramId.HasValue)
                query = query.Where(x => !x.Course.AcademicProgramId.HasValue || (x.Course.AcademicProgramId.HasValue && (x.Course.AcademicProgramId == student.AcademicProgramId || x.Course.AcademicProgram.Code == "00")));
            else
                query = query.Where(x => !x.Course.AcademicProgramId.HasValue);

            var dbData = query
               .Select(x => new
               {
                   x.AcademicYear,
                   CourseCode = x.Course.Code,
                   CourseName = x.Course.Name,
                   x.Course.Credits,
                   x.CourseId,
                   AcademicProgram = x.Course.AcademicProgramId.HasValue ? x.Course.AcademicProgram.Name : "00",
                   x.IsElective
               })
               .ToList();

            var courses = dbData
               .OrderBy(x => x.AcademicYear)
               .ThenBy(x => x.CourseCode)
               .Select(x => new
               {
                   yearName = $"SEMESTRE : {ConstantHelpers.ACADEMIC_YEAR.TEXT[x.AcademicYear]}",
                   year = x.AcademicYear.ToString("D2"),
                   code = x.CourseCode,
                   course = x.IsElective ? $"{x.CourseName} - <strong>(E)</strong>" : x.CourseName,
                   credits = x.Credits.ToString("0.0"),
                   id = x.CourseId,
                   academicProgram = x.AcademicProgram,
                   tries = academicHistories.Any(ah => ah.CourseId == x.CourseId) ? academicHistories.Where(ah => ah.CourseId == x.CourseId).Max(ah => ah.Try) : 0,
                   academicHistoryId = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Grade).Select(ah => ah.Id).FirstOrDefault() : academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Term.Year).ThenByDescending(ah => ah.Term.Number).ThenByDescending(ah => ah.Grade).Select(ah => ah.Id).FirstOrDefault(),
                   grade = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Grade).Select(ah => ah.Grade).FirstOrDefault() : academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Term.Year).ThenByDescending(ah => ah.Term.Number).ThenByDescending(ah => ah.Grade).Select(ah => ah.Grade).FirstOrDefault(),
                   term = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Grade).Select(ah => ah.Term.Name).FirstOrDefault() : academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Term.Year).ThenByDescending(ah => ah.Term.Number).ThenByDescending(ah => ah.Grade).Select(ah => ah.Term.Name).FirstOrDefault(),
                   evaluationReport = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Grade).Select(ah => ah.EvaluationReport?.Code).FirstOrDefault() : academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Term.Year).ThenByDescending(ah => ah.Term.Number).ThenByDescending(ah => ah.Grade).Select(ah => ah.EvaluationReport?.Code).FirstOrDefault(),
                   evaluationReportDate = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ?
                   academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Grade).Select(ah => evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.CreatedAt ? ah.EvaluationReport?.CreatedAt.ToLocalDateFormat() : evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.ReceptionDate ? ah.EvaluationReport?.ReceptionDate.ToLocalDateFormat() : evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.LastGradePublished ? ah.EvaluationReport?.LastGradePublishedDate.ToLocalDateFormat() : "-").FirstOrDefault() :
                   academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Term.Year).ThenByDescending(ah => ah.Term.Number).ThenByDescending(ah => ah.Grade).Select(ah => evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.CreatedAt ? ah.EvaluationReport?.CreatedAt.ToLocalDateFormat() : evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.ReceptionDate ? ah.EvaluationReport?.ReceptionDate.ToLocalDateFormat() : evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.LastGradePublished ? ah.EvaluationReport?.LastGradePublishedDate.ToLocalDateFormat() : "-").FirstOrDefault(),
                   status = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? "Aprobado" : "Pendiente",
                   validated = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved && ah.Validated)
               })
               .ToList();

            var recordsTotal = courses.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = courses,
                DrawCounter = sentParameters.DrawCounter,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetCurriculumElectivesDatatable(DataTablesStructs.SentParameters sentParameters, Guid id)
        {
            Expression<Func<AcademicYearCourse, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = (x) => x.AcademicYear.ToString("D2");
                    break;
            }

            var student = await _context.Students.FindAsync(id);
            var academicHistories = await _context.AcademicHistories.Where(x => x.StudentId == id && !x.Withdraw).Include(x => x.Term).Include(x => x.EvaluationReport).OrderBy(ah => ah.Term.Year).ThenBy(ah => ah.Term.Number).ToListAsync();

            var academicYearCourses = await _context.AcademicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId && x.IsElective)
                .Include(x => x.Course)
                .ToListAsync();

            var courses = academicYearCourses
               .Select(x => new
               {
                   year = x.AcademicYear.ToString("D2"),
                   code = x.Course.Code,
                   course = x.Course.Name,
                   credits = x.Course.Credits.ToString("0.0"),
                   academicHistoryId = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Grade).Select(ah => ah.Id).FirstOrDefault() : academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Term.Year).ThenByDescending(ah => ah.Term.Number).ThenByDescending(ah => ah.Grade).Select(ah => ah.Id).FirstOrDefault(),
                   tries = academicHistories.Where(ah => ah.CourseId == x.CourseId).Max(ah => ah.Try),
                   grade = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Grade).Select(ah => ah.Grade).FirstOrDefault() : academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Term.Year).ThenByDescending(ah => ah.Term.Number).ThenByDescending(ah => ah.Grade).Select(ah => ah.Grade).FirstOrDefault(),
                   term = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Grade).Select(ah => ah.Term.Name).FirstOrDefault() : academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Term.Year).ThenByDescending(ah => ah.Term.Number).ThenByDescending(ah => ah.Grade).Select(ah => ah.Term.Name).FirstOrDefault(),
                   status = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? "Aprobado" : "Pendiente",
                   evaluationReport = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Grade).Select(ah => ah.EvaluationReport?.Code).FirstOrDefault() : academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Term.Year).ThenByDescending(ah => ah.Term.Number).ThenByDescending(ah => ah.Grade).Select(ah => ah.EvaluationReport?.Code).FirstOrDefault(),
                   evaluationReportDate = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Grade).Select(ah => ah.EvaluationReport?.ReceptionDate.ToLocalDateFormat()).FirstOrDefault() : academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Term.Year).ThenByDescending(ah => ah.Term.Number).ThenByDescending(ah => ah.Grade).Select(ah => ah.EvaluationReport?.ReceptionDate.ToLocalDateFormat()).FirstOrDefault(),
               })
               .OrderBy(x => x.year)
               .ThenBy(x => x.code)
               .ToList();

            var recordsTotal = courses.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = courses,
                DrawCounter = sentParameters.DrawCounter,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<IEnumerable<AcademicYearCourse>> GetAllByCurriculumWithPrerequisites(Guid curriculumId)
        {
            return await _context.AcademicYearCourses
                .Include(x => x.Course)
                .ThenInclude(x => x.AcademicProgram)
                .Include(x => x.PreRequisites)
                .Where(x => x.CurriculumId == curriculumId)
                .ToListAsync();

        }
        public async Task<IEnumerable<AcademicYearCourse>> GetAll(Guid? curriculumId = null, byte? academicYear = null, bool? isElective = null, Guid? academicProgramId = null)
        {
            var query = _context.AcademicYearCourses.AsNoTracking();

            if (curriculumId.HasValue)
                query = query.Where(x => x.CurriculumId == curriculumId.Value);

            if (academicYear.HasValue)
                query = query.Where(x => x.AcademicYear == academicYear.Value);

            if (isElective.HasValue)
                query = query.Where(x => x.IsElective == isElective.Value);

            //FILTRO POR PROGRAMA ACADEMICO
            if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
                query = query.Where(x => !x.Course.AcademicProgramId.HasValue || (x.Course.AcademicProgramId.HasValue && (x.Course.AcademicProgramId == academicProgramId || x.Course.AcademicProgram.Code == "00")));

            var academicYearCourses = await query
                .Include(x => x.Course)
                .ThenInclude(x => x.AcademicProgram)
                .Include(x => x.PreRequisites)
                .ToListAsync();

            return academicYearCourses;
        }

        public async Task<int> CountAll(Guid? curriculumId = null, byte? academicYear = null, bool? isElective = null, Guid? academicProgramId = null)
        {
            var query = _context.AcademicYearCourses.Include(x => x.Course).AsNoTracking();

            if (curriculumId.HasValue)
                query = query.Where(x => x.CurriculumId == curriculumId.Value);

            if (academicYear.HasValue)
                query = query.Where(x => x.AcademicYear == academicYear.Value);

            if (isElective.HasValue)
                query = query.Where(x => x.IsElective == isElective.Value);

            if (academicProgramId.HasValue)
                query = query.Where(x => x.Course.AcademicProgramId == academicProgramId.Value);

            return await query.CountAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPendingCoursesStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            var academicHistories = await _context.AcademicHistories.Where(x => x.StudentId == studentId && x.Approved).Select(x => x.CourseId).ToArrayAsync();
            var academicHistoriesHash = new HashSet<Guid>(academicHistories);

            var query = _context.AcademicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId
                && !academicHistoriesHash.Contains(x.CourseId))
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    id = x.CourseId,
                    code = x.Course.Code,
                    course = x.Course.Name,
                    year = x.AcademicYear,
                    isElective = x.IsElective,
                    credits = x.Course.Credits
                })
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesRecognitionStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId)
        {
            var student = await _context.Students.FindAsync(studentId);

            var recognitions = await _context.CoursesRecognition
                .Where(x => x.Recognition.StudentId == studentId)
                .Select(x => x.CourseId)
                .ToArrayAsync();

            var academicHistoriesHash = await _context.AcademicHistories
                .Where(x => x.StudentId == studentId &&
                                recognitions.Contains(x.CourseId))
                .ToListAsync();

            var ayc = await _context.AcademicYearCourses
                .Include(x => x.Course)
               .Where(x => x.CurriculumId == student.CurriculumId)
               //&& academicHistoriesHash.Contains(x.CourseId))
               .ToListAsync();

            var client = ayc.Where(x => academicHistoriesHash.Any(c => c.CourseId == x.CourseId));
            var recordsFiltered = client.Count();

            var data = client
                .Select(x => new
                {
                    id = x.CourseId,
                    code = x.Course.Code,
                    course = x.Course.Name,
                    grade = academicHistoriesHash.FirstOrDefault(c => c.CourseId == x.CourseId)?.Grade ?? 0,
                    year = x.AcademicYear,
                    isElective = x.IsElective,
                    credits = x.Course.Credits,
                    studentId
                }).ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<IEnumerable<Tuple<Guid, string>>> GetTuplesCourseIdAndAcademicYear()
        {
            var academicYearCourses = await _context.AcademicYearCourses
                .Select(x => new Tuple<Guid, string>(x.CourseId, $"{x.AcademicYear}"))
                .ToListAsync();

            return academicYearCourses;
        }

        public async Task<List<PendingCoursesTemplate>> GetPendingCoursesStudent(Guid studentId, Guid curriculumId)
        {
            var academicHistories = await _context.AcademicHistories
                .Where(x => x.StudentId == studentId
                && x.Approved)
                .Select(x => x.CourseId)
                .ToArrayAsync();

            var courses = await _context.AcademicYearCourses
                .Where(x => x.CurriculumId == curriculumId
                && !academicHistories.Contains(x.CourseId))
                .Select(x => new PendingCoursesTemplate
                {
                    id = x.Course.Id,
                    Code = x.Course.Code,
                    text = $"{x.Course.FullName}",
                    Credits = x.Course.Credits
                    //id = x.CourseId,
                    //text = $"{x.Course.FullName}"
                }).ToListAsync();

            return courses;
        }

        public async Task<List<CurriculumTemplate>> GetCurriculumDetail(Guid curriculumId)
        {
            var result = await _context.AcademicYearCourses

                .Include(x => x.PreRequisites).ThenInclude(x => x.Course)
                .Include(x => x.AcademicYearCourseCertificates).ThenInclude(x => x.CourseCertificate)

                .Where(x => x.CurriculumId == curriculumId)
                .Select(x => new CurriculumTemplate
                {
                    Cycle = x.AcademicYear,
                    CodeCourse = x.Course.Code,
                    NameArea = x.Course.Area.Name,
                    Credits = x.Course.Credits,
                    RequiredCredits = x.RequiredCredits,
                    PracticalHours = x.Course.PracticalHours,
                    SeminarHours = x.Course.SeminarHours,
                    TheoreticalHours = x.Course.TheoreticalHours,
                    VirtualHours = x.Course.VirtualHours,
                    NameCourse = x.Course.Name,
                    Requisites = x.PreRequisites.Select(s => new PreRequisiteTemplate { Name = s.Course.Name }).ToList(),
                    Certificates = x.AcademicYearCourseCertificates.Select(s => new CertificatesTemplate { Name = s.CourseCertificate.Name }).ToList(),

                }).OrderBy(x => x.Cycle).ThenBy(x => x.CodeCourse).ToListAsync();

            return result;
        }

        public async Task<object> GetCoursesByCareerIdAndCurriculumActive(Guid careerId)
        {
            var result = await _context.AcademicYearCourses
                .Where(x => x.Curriculum.CareerId == careerId && x.Curriculum.IsActive)
                .Select(x => new
                {
                    id = x.CourseId,
                    text = x.Course.FullName
                })
                .ToArrayAsync();

            return result.OrderBy(x => x.text).ToArray();
        }
        public async Task<AcademicYearCourse> GetAcademicYearCourseByCourseId(Guid courseId)
        {
            var academicYearCourse = await _context.AcademicYearCourses.Include(x => x.Curriculum).FirstOrDefaultAsync(x => x.CourseId == courseId);

            return academicYearCourse;
        }

        public async Task<List<AcademicYearCourse>> GetAllAcademicYearCoursesByCourseId(Guid courseId)
        {
            var academicYearCourse = await _context.AcademicYearCourses.Where(x => x.CourseId == courseId).ToListAsync();
            return academicYearCourse;
        }

        public async Task<List<AcademicYearCourseDetail>> GetAcademicYearDetailByStudentIdAndAcademicYearId(Guid studentId, byte academicYear, ClaimsPrincipal user = null)
        {

            var student = await _context.Students.FindAsync(studentId);

            var academicHistories = await _context.AcademicHistories
                    .Where(x => x.StudentId == studentId)
                    .Include(x => x.Term)
                    //.OrderByDescending(x => x.Term.StartDate)
                    .ToListAsync();

            var academicYearsCourses = await _context.AcademicYearCourses
                .Where(x => x.AcademicYear == academicYear
                && x.CurriculumId == student.CurriculumId
                && !x.IsElective)
                .Include(x => x.Course)
                .ToListAsync();

            var result = academicYearsCourses.Select(ac =>
            {
                var academicHistory = academicHistories
                       .Where(x => x.CourseId == ac.CourseId && !x.Withdraw)
                       .OrderByDescending(x => x.Approved)
                       .ThenByDescending(x => x.Term.Year)
                       .ThenByDescending(x => x.Term.Number)
                       .FirstOrDefault();

                return new AcademicYearCourseDetail
                {
                    CourseCode = ac.Course.Code,
                    CourseName = ac.Course.Name,
                    Course = ac.Course.FullName,
                    Credits = ac.Course.Credits.ToString("0.0"),
                    Try = academicHistory?.Try.ToString() ?? "-",
                    //grade = academicHistory != null ? academicHistory.Validated ? "Convalidado" : academicHistory.Grade.ToString() : "-",
                    Grade = academicHistory != null ? (academicHistory.Validated && academicHistory.Grade <= 0 ? "Convalidado" : academicHistory.Approved ? academicHistory.Grade.ToString() : "-") : "-",
                    Term = academicHistory?.Term.Name ?? "-",
                    Status = academicHistory?.Approved ?? false,
                    Validated = academicHistory != null ? academicHistory.Validated : false
                };
            });

            return result.ToList();
        }

        public async Task<object> GetAcademicSituationElectiveByStudentId(Guid studentId)
        {
            var student = await _context.Students.FindAsync(studentId);

            var academicYearsCourses = await _context.AcademicYearCourses
                .Where(x => x.IsElective && x.CurriculumId == student.CurriculumId)
                .OrderBy(x => x.AcademicYear)
                .ThenByDescending(x => x.Course.Code)
                .Select(x => new
                {
                    x.CourseId,
                    x.Course.Code,
                    x.Course.Name,
                    x.Course.Credits,
                    x.AcademicYear
                })
                .ToListAsync();

            var academicHistories = await _context.AcademicHistories
                .Where(x => x.StudentId == studentId)
                .Select(x => new
                {
                    TermStartDate = x.Term.StartDate,
                    x.CourseId,
                    x.StudentId,
                    x.Approved,
                    x.Validated,
                    TermName = x.Term.Name,
                    x.Grade,
                    x.Try
                }).ToListAsync();

            var result = academicYearsCourses.Select(ac =>
            {
                var academicHistory = academicHistories
                       .Where(x => x.CourseId == ac.CourseId && x.StudentId == studentId)
                       .OrderByDescending(x => x.Approved)
                       .ThenByDescending(x => x.TermStartDate)
                       .FirstOrDefault();

                return new
                {
                    course = $"{ac.Code} - {ac.Name}",
                    credits = ac.Credits.ToString(),
                    academicYear = ac.AcademicYear,
                    @try = academicHistory?.Try.ToString() ?? "-",
                    grade = academicHistory != null ? academicHistory.Validated && academicHistory.Grade <= 0 ? "Convalidado" : academicHistory.Grade.ToString() : "-",
                    term = academicHistory?.TermName ?? "-",
                    status = academicHistory?.Approved ?? false
                };
            });

            return result.Where(x => x.status);
        }

        public async Task<object> GetAcademicSummaryDetail(Guid studentId, Guid termId)
        {
            var student = await _context.Students.FindAsync(studentId);
            var term = await _context.Terms.FindAsync(termId);

            var result = term.Status == ConstantHelpers.TERM_STATES.ACTIVE
                ? await _context.StudentSections.Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == termId)
                .Select(x => new AcademicSummaryDetailTemplate
                {
                    Course = x.Section.CourseTerm.Course.FullName,
                    Credits = x.Section.CourseTerm.Course.Credits.ToString("0.0"),
                    Try = x.Try,
                    FinalGrade = x.Status == ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS ? "-"
                    : (x.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN ? "Retirado" : x.FinalGrade.ToString()),
                    Status = x.Status,
                    AcademicYear = _context.AcademicYearCourses
                        .Where(ayc => ayc.CourseId == x.Section.CourseTerm.CourseId && ayc.CurriculumId == student.CurriculumId)
                        .Select(ayc => ayc.AcademicYear as byte?)
                        .FirstOrDefault()
                    ?? _context.AcademicYearCourses
                    .Where(ayc => ayc.CourseId == x.Section.CourseTerm.CourseId)

                    .Select(ayc => ayc.AcademicYear as byte?)
                    .FirstOrDefault() ?? 0
                }).ToListAsync()

                : await _context.AcademicHistories.Where(x => x.StudentId == student.Id && x.TermId == termId && !x.Validated)
                .Select(x => new AcademicSummaryDetailTemplate
                {
                    Course = x.Course.FullName,
                    Credits = x.Course.Credits.ToString("0.0"),
                    Try = x.Try,
                    FinalGrade = x.Withdraw ? "Retirado" : x.Grade.ToString(),
                    Status = (int)x.Type,
                    AcademicYear = _context.AcademicYearCourses
                        .Where(ayc => ayc.CourseId == x.CourseId && ayc.CurriculumId == student.CurriculumId)
                        .Select(ayc => ayc.AcademicYear as byte?)
                        .FirstOrDefault()
                        ?? _context.AcademicYearCourses
                        .Where(ayc => ayc.CourseId == x.CourseId) //&& ac.AcademicYear.Curriculum.CareerId == _context.AcademicSummaries
                                                                  //.Where(acs => acs.TermId == x.TermId).Select(acs => acs.CareerId).FirstOrDefault())
                        .Select(ayc => ayc.AcademicYear as byte?)
                        .FirstOrDefault() ?? 0
                }).ToListAsync();

            if (term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
            {

                var specialEvaluations = await _context.AcademicHistories.Where(x=>x.StudentId == student.Id && x.TermId == termId
                    && (x.Type == ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION || x.Type == ConstantHelpers.AcademicHistory.Types.SPECIAL || x.Type == ConstantHelpers.AcademicHistory.Types.REEVALUATION || x.Type == ConstantHelpers.AcademicHistory.Types.CONVALIDATION)
                    )
                     .Select(x => new AcademicSummaryDetailTemplate
                     {
                         Course = x.Course.FullName,
                         Credits = x.Course.Credits.ToString("0.0"),
                         Try = x.Try,
                         FinalGrade = x.Withdraw ? "Retirado" : x.Grade.ToString(),
                         Status = (int)x.Type,
                         AcademicYear = _context.AcademicYearCourses
                        .Where(ayc => ayc.CourseId == x.CourseId && ayc.CurriculumId == student.CurriculumId)
                        .Select(ayc => ayc.AcademicYear as byte?)
                        .FirstOrDefault()
                        ?? _context.AcademicYearCourses
                        .Where(ayc => ayc.CourseId == x.CourseId) //&& ac.AcademicYear.Curriculum.CareerId == _context.AcademicSummaries
                                                                  //.Where(acs => acs.TermId == x.TermId).Select(acs => acs.CareerId).FirstOrDefault())
                        .Select(ayc => ayc.AcademicYear as byte?)
                        .FirstOrDefault() ?? 0
                     }).ToListAsync();

                result.AddRange(specialEvaluations);
            }

            return result;
        }

        public Task<bool> AnyByIdAndCourseId(Guid id, Guid courseId)
        {
            return _context.AcademicYearCourses.AnyAsync(x => x.Id == id && x.CourseId == courseId);
        }

        public Task<bool> AnyByCourseIdAndAndCurriculumId(Guid courseId, Guid curriculumId)
        {
            return _context.AcademicYearCourses.AnyAsync(x => x.CourseId == courseId && x.CurriculumId == curriculumId);
        }

        public async Task<IEnumerable<AcademicYearCourseTemplateA>> GetAllAsModelA(Guid? curriculumId = null)
        {
            var query = _context.AcademicYearCourses.AsQueryable();

            if (curriculumId.HasValue)
                query = query.Where(x => x.CurriculumId == curriculumId);

            var random = new Random();

            var curriculumList = await query
                .Select(x => new AcademicYearCourseTemplateA
                {
                    Id = x.Course.Id,
                    Code = x.Course.Code,
                    Name = x.Course.Name,
                    Type = x.IsElective ? "E" : "O",
                    Cycle = x.AcademicYear,
                    Credit = x.Course.Credits,
                    Area = x.Course.Area.Name,
                    Regularized = "Sem",
                    TotalHours = x.Course.TotalHours,
                    PlannedHours = x.Course.TheoreticalHours,
                    PracticalHours = x.Course.PracticalHours,
                    AcademicYearNumber = x.AcademicYear,
                    RequiredCredit = x.RequiredCredits,
                    Requisites = _context.AcademicYearCoursePreRequisites.Where(s => s.AcademicYearCourseId == x.Id).Select(s => s.Course.Name).ToList(),
                    CodRequisites = _context.AcademicYearCoursePreRequisites.Where(s => s.AcademicYearCourseId == x.Id).Select(s => s.Course.Code).ToList(),
                    Certificates = _context.AcademicYearCourseCertificates.Where(s => s.AcademicYearCourseId == x.Id).Select(s => s.CourseCertificate.Name).ToList(),
                    Category = random.Next(3)
                })
                .OrderBy(x => x.AcademicYearNumber)
                .ToListAsync();

            return curriculumList;
        }

        public async Task<IEnumerable<AcademicYearCourseTemplateZ>> GetAllAsModelZ(Guid curriculumId, string academicProgramCode)
        {
            var query = _context.AcademicYearCourses.AsQueryable();

            query = query.Where(x => x.CurriculumId == curriculumId);

            query = query.Where(x => x.Course.AcademicProgram.Code == academicProgramCode || x.Course.AcademicProgram.Code == "00");

            var curriculumList = await query
                .Select(x => new AcademicYearCourseTemplateZ
                {
                    //Id = x.Course.Id,
                    Code = x.Course.Code,
                    Name = x.Course.Name,
                    Speciality = x.Course.AcademicProgram.Code,
                    //Type = x.IsElective ? "E" : "O",
                    Cycle = x.AcademicYear,
                    Credit = x.Course.Credits,
                    //Area = x.Course.Area.Name,
                    //Regularized = "Sem",
                    TotalHours = x.Course.TotalHours,
                    PlannedHours = x.Course.TheoreticalHours,
                    PracticalHours = x.Course.PracticalHours,
                    Requirement = x.PreRequisites.Any() ? string.Join(",", x.PreRequisites.Select(y => y.Course.Code).ToList()) : "NINGUNO"
                    //AcademicYearNumber = x.AcademicYear.Number
                })
                .OrderBy(x => x.Cycle)
                .ToListAsync();

            return curriculumList;
        }

        public async Task<object> GetAllAsSelect2ClientSide(Guid? curriculumId = null, string name = null, Guid? onlycourseId = null, bool? onlyOptional = null)
        {
            var query = _context.AcademicYearCourses.AsQueryable();
            if (curriculumId.HasValue) query = query.Where(x => x.CurriculumId == curriculumId.Value);

            if (!string.IsNullOrEmpty(name)) query = query.Where(x => x.Course.Name.ToUpper().Contains(name.ToUpper()) || x.Course.Code.ToUpper().Contains(name.ToUpper()));

            if (onlycourseId.HasValue && onlycourseId != Guid.Empty)
            {
                var acprog = await _context.Courses.Where(x => x.Id == onlycourseId.Value).Select(x => x.AcademicProgram.Code).FirstOrDefaultAsync();

                query = query.Where(x => x.Course.AcademicProgram.Code == acprog || x.Course.AcademicProgram.Code == "00");
            }

            var courses = await query
                .Select(x => new
                {
                    id = x.CourseId,
                    text = x.Course.FullName
                })
                .Take(10)
                .ToListAsync();

            return courses;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAvailableCoursesStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid? termId = null)
        {
            Term term;
            if (termId.HasValue && termId != Guid.Empty) term = await _context.Terms.FindAsync(termId);
            else term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            var academicYearDispersion = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.ACADEMIC_YEAR_DISPERSION_ADMIN);

            var student = await _context.Students
                .Where(x => x.Id == studentId)
                .Select(x => new
                {
                    x.Id,
                    x.CurriculumId,
                    x.AcademicProgramId,
                    x.CurrentAcademicYear,
                    x.CareerId,
                    CareerCode = x.Career.Code,
                    CurriculumCode = x.Curriculum.Code,
                    x.CampusId
                })
                .FirstOrDefaultAsync();

            var academicHistories = await _context.AcademicHistories
                .Where(x => x.StudentId == studentId && !x.Withdraw)
                .Select(x => new
                {
                    x.CourseId,
                    x.Approved,
                    x.Term.Name,
                    x.Term.Year,
                    x.Term.Number,
                    x.Term.IsSummer,
                    x.Type
                })
                .ToArrayAsync();

            var academicYearCourses = await _context.AcademicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId)
                .Select(x => x.CourseId).ToListAsync();
            academicHistories = academicHistories.Where(x => academicYearCourses.Contains(x.CourseId)).ToArray();

            var approvedAcademicHistoriesList = academicHistories.Where(x => x.Approved).Select(x => x.CourseId).ToList();

            if (term.Status == ConstantHelpers.TERM_STATES.INACTIVE)
            {
                var studentSections = await _context.StudentSections
                    .Where(x => x.StudentId == studentId && x.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
                    .Select(x => x.Section.CourseTerm.CourseId)
                    .ToListAsync();

                approvedAcademicHistoriesList.AddRange(studentSections);
            }

            var approvedAcademicHistoriesHash = approvedAcademicHistoriesList.ToHashSet();
            var academicHistoriesHash = new List<Guid>();

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD)
                academicHistoriesHash = academicHistories
                    .Where(x => x.Number != "A" && !x.IsSummer)
                    .Select(x => x.CourseId).ToList();
            else if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSM)
                academicHistoriesHash = academicHistories
                    .Where(x => !x.IsSummer
                    && x.Type != ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.REEVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.DEFERRED)
                    .Select(x => x.CourseId).ToList();
            else academicHistoriesHash = academicHistories
                    .Where(x => x.Type != ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.REEVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.DEFERRED)
                    .Select(x => x.CourseId).ToList();

            var approvedCredits = await _context.AcademicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId && approvedAcademicHistoriesHash.Contains(x.CourseId))
                .SumAsync(x => x.Course.Credits);

            var query = _context.AcademicYearCourses
                .Where(a => a.CurriculumId == student.CurriculumId
                && !approvedAcademicHistoriesHash.Contains(a.CourseId)
                && a.AcademicYear <= student.CurrentAcademicYear + academicYearDispersion)
                .AsNoTracking();

            if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAMAD)
                query = query.Where(x => x.RequiredCredits <= approvedCredits);

            var parallelCoursesLimit = await _context.CareerParallelCourses.Where(x => x.CareerId == student.CareerId && x.AcademicYear == student.CurrentAcademicYear && x.AppliesForAdmin).FirstOrDefaultAsync();
            if (parallelCoursesLimit != null)
            {
                //query = query.Where(a => a.PreRequisites.All(pr => approvedAcademicHistoriesHash.Contains(pr.CourseId)) || a.AcademicYear <= parallelCoursesLimit.AcademicYear);
            }
            else
                query = query.Where(a => a.PreRequisites.Where(pr => !pr.IsOptional).All(pr => approvedAcademicHistoriesHash.Contains(pr.CourseId))
                    && (!a.PreRequisites.Where(pr => pr.IsOptional).Any() || a.PreRequisites.Where(pr => pr.IsOptional).Any(pr => approvedAcademicHistoriesHash.Contains(pr.CourseId)))
                    && a.RequiredCredits <= approvedCredits);

            //FILTRO POR PROGRAMA ACADEMICO
            query = query.Where(x => !x.Course.AcademicProgramId.HasValue || x.Course.AcademicProgramId == student.AcademicProgramId || x.Course.AcademicProgram.Code == "00");

            var tmpEnrollments = _context.TmpEnrollments
                .Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == term.Id)
                .Select(x => x.Section.CourseTerm.CourseId).ToHashSet();

            var dbData = await query
                .Select(x => new
                {
                    id = x.CourseId,
                    code = x.Course.Code,
                    course = x.Course.Name,
                    year = x.AcademicYear,
                    isElective = x.IsElective,
                    credits = x.Course.Credits,
                    //hasSections = _context.Sections.Count(s => s.CourseTerm.CourseId == x.CourseId && s.CourseTerm.TermId == term.Id) > 0 ? true : false,
                    hasSections = x.Course.CourseTerms.Any(y => y.TermId == term.Id),
                    academicProgram = x.Course.AcademicProgramId.HasValue ? x.Course.AcademicProgram.Name : "00",
                    requisites = x.PreRequisites.Select(y => $"{y.Course.Code} - {y.Course.Name}").ToList()
                })
                .ToListAsync();

            var recordsFiltered = dbData.Count;

            var isSpecialTurn = await _context.EnrollmentTurns
                .Where(x => x.StudentId == studentId && x.TermId == term.Id)
                .Select(x => x.SpecialEnrollment)
                .FirstOrDefaultAsync();
            var isLockedOut = false;

            if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAMAD && !isSpecialTurn
                && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNSCH)
            {
                var pendingCoursesTime = dbData.Select(x => academicHistoriesHash.Count(ah => x.id == ah) + 1).ToList();
                isLockedOut = pendingCoursesTime.Any(x => x > 4);
            }

            if (await _context.EnrollmentReservations.AnyAsync(x => x.StudentId == studentId && x.TermId == term.Id))
                isLockedOut = true;

            var data = dbData
                .Select(x => new
                {
                    x.id,
                    x.code,
                    x.course,
                    isLocked = isLockedOut,
                    x.year,
                    x.isElective,
                    x.credits,
                    time = academicHistoriesHash.Count(ah => x.id == ah) + 1,
                    lastTerm = academicHistories.Where(ah => x.id == ah.CourseId).OrderByDescending(y => y.Year).ThenByDescending(y => y.Number).Select(y => y.Name).FirstOrDefault(),
                    x.hasSections,
                    x.academicProgram,
                    isSelected = tmpEnrollments.Contains(x.id),
                    requisites = string.Join(Environment.NewLine, x.requisites)
                })
                .OrderBy(x => x.year)
                .ThenBy(x => x.code)
                .ToList();

            if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNSCH)
                if (data.Any(x => x.time >= 4) && !isSpecialTurn)
                    data = data.Where(x => x.time >= 4).ToList();

            data = data.Where(x => x.hasSections).ToList();

            var limitEnrollmentToCampus = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.LIMIT_ENROLLMENT_TO_STUDENT_CAMPUS));
            if (limitEnrollmentToCampus)
            {
                var schedules = await _context.ClassSchedules
                    .Where(x => x.Section.CourseTerm.TermId == term.Id && x.Section.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == student.CurriculumId))
                    .Select(x => new
                    {
                        x.Section.CourseTerm.CourseId,
                        x.Classroom.Building.CampusId
                    }).ToListAsync();
                var availableCoursesInCampus = schedules.Where(x => x.CampusId == student.CampusId).Select(x => x.CourseId).ToHashSet();

                data = data.Where(x => availableCoursesInCampus.Contains(x.id)).ToList();
            }

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<AcademicYearCourse> GetWithCourseAndCurriculum(Guid courseId, Guid curriculumId)
        {
            var result = await _context.AcademicYearCourses
                .Where(y => y.CurriculumId == curriculumId && y.CourseId == courseId)
                .Include(x => x.Curriculum)
                .Include(x => x.Course)
                .FirstOrDefaultAsync();

            return result;
        }
        public async Task<List<AcademicYearCourse>> GetCourseEnrollment(Guid careerId, byte academicYear)
        {
            var courses = await _context.AcademicYearCourses
                .Where(ay =>
                    ay.Curriculum.CareerId == careerId &&
                    ay.Curriculum.IsActive && ay.AcademicYear == academicYear)
                .Include(x => x.Course)
                .ToListAsync();

            return courses;
        }

        public async Task<object> GetStudentCourse(Guid careerId, Guid studentId, Guid? termId = null)
        {
            //var qry = await (from ay in _context.AcademicYearCourses
            //                 where ay.Curriculum.CareerId == careerId &&
            //          ay.Curriculum.IsActive && ay.AcademicYear == 1
            //                 from ss in _context.StudentSections.Where(ss => ss.StudentId == studenId && ss.Section.CourseTerm.TermId == termId && ss.Section.CourseTerm.CourseId == ay.CourseId).DefaultIfEmpty()
            //                 select new
            //                 {
            //                     id = ss != null ? ss.Id.ToString() : "---",
            //                     courseId = ay.CourseId,
            //                     code = ay.Course.Code,
            //                     course = ay.Course.Name,
            //                     section = ss != null ? ss.Section.Code : "---",
            //                     vacancies = ss != null ? ss.Section.Vacancies - ss.Section.StudentSections.Count : -1,
            //                     credits = ay.Course.Credits
            //                 }).ToListAsync();

            var data = await _context.StudentSections
                .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termId)
                .OrderBy(x => x.Section.CourseTerm.Course.Code)
                .Select(x => new
                {
                    id = x.Id,
                    courseId = x.Section.CourseTerm.CourseId,
                    code = x.Section.CourseTerm.Course.Code,
                    course = x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code,
                    vacancies = x.Section.Vacancies - x.Section.StudentSections.Count < 0 ? 0 : x.Section.Vacancies - x.Section.StudentSections.Count,
                    //studentSections = x.Section.StudentSections.Count,
                    credits = x.Section.CourseTerm.Course.Credits
                })
                .ToListAsync();

            //var result = data
            //    .Select(x => new
            //    {

            //    }).ToList();
            return data;
        }

        public async Task<List<Guid>> GetCourseId(Guid careerId, Guid curriculumId)
        {
            var coursesIds = await _context.AcademicYearCourses
                .Where(ay =>
                    ay.Curriculum.CareerId == careerId
                    //&& ay.AcademicYear.Curriculum.IsActive 
                    && ay.CurriculumId == curriculumId
                    && ay.AcademicYear == 1)
                .Select(ay => ay.CourseId)
                .ToListAsync();

            return coursesIds;
        }
        public async Task<int> GetCourseCount(Guid id, int number)
        {
            var courses = await _context.AcademicYearCourses.Where(ay => ay.Curriculum.CareerId == id &&
                 ay.Curriculum.IsActive && ay.AcademicYear == 1).CountAsync();

            return courses;
        }
        public async Task<IEnumerable<AcademicYearCourse>> GetAllWithData()
        {
            var result = await _context.AcademicYearCourses
                .Include(x => x.Course)
                .Include(x => x.PreRequisites)
                .ToListAsync();

            return result;

        }

        public async Task<string> GetCourseEquivalences(Guid studentId, Guid courseId)
        {
            var equivalences = await _context.CourseEquivalences.Where(x => x.NewAcademicYearCourse.CourseId == courseId).Include(x => x.OldAcademicYearCourse).ToListAsync();

            var academicHistories = await _context.AcademicHistories
              .Where(x => x.StudentId == studentId && x.Approved)
              .Include(x => x.Course).ToListAsync();

            var text = academicHistories
                .Where(x => equivalences.Any(e => e.OldAcademicYearCourse.CourseId == x.CourseId))
                .Select(x => "CURSO " + x.Course.Code + " POR CAMBIO CURRICULAR")
                .ToList();

            return string.Join(Environment.NewLine, text);
        }

        public async Task<List<AcademicYearCourse>> GetWithDataByCurriculumIdAndAcademic(Guid curriculumId, int academicYear, int academicYearDispersion, IEnumerable<AcademicHistory> academicHistories)
        {
            var result = await _context.AcademicYearCourses
                .Where(a => a.CurriculumId == curriculumId
                && a.AcademicYear <= academicYear + academicYearDispersion)
                .Include(x => x.Course)
                .Include(x => x.PreRequisites)
                .ToListAsync();

            result = result.Where(a => !academicHistories.Any(ah => ah.CourseId == a.CourseId && ah.Approved)
                && a.PreRequisites.All(p => academicHistories.Any(h => h.CourseId == p.CourseId && h.Approved)))
                .ToList();

            return result;

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAcademicHistoryV2CoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId)
        {
            var student = await _context.Students.FindAsync(studentId);

            var academicYearCourses = await _context.AcademicYearCourses
                .Select(x => new
                {
                    x.CurriculumId,
                    x.CourseId,
                    Curriculum = x.Curriculum.Code,
                    x.AcademicYear
                }).ToListAsync();

            var academicYearCoursesStudent = academicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId)
                .ToList();

            var query = _context.AcademicHistories
                .Where(x => x.StudentId == studentId)
                .OrderBy(x => x.Term.EndDate)
                .AsNoTracking();

            var recordsFiltered = query.Count();

            var dataDB = query
                .OrderBy(x => x.Course.AcademicYearCourses.Where(y => y.CurriculumId == student.CurriculumId && y.CourseId == x.CourseId).Select(y => y.AcademicYear).FirstOrDefault())
                .ThenBy(x => x.Course.Code)
                .ThenBy(X => X.Term.Name)
                .Select(x => new
                {
                    courseId = x.CourseId,
                    code = x.Course.Code,
                    course = x.Course.Name,
                    credits = x.Course.Credits.ToString("0.0"),
                    grade = x.Grade,
                    term = x.Term.Name,
                    status = x.Withdraw ? "Retirado" : x.Approved ? "Aprobado" : "Reprobado",
                    withdrawn = x.Withdraw
                })

                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

            var data = dataDB
                .Select(x => new
                {
                    curriculum = academicYearCourses.Where(y => y.CourseId == x.courseId).Select(x => x.Curriculum).FirstOrDefault(),
                    year = academicYearCoursesStudent.Where(y => y.CourseId == x.courseId).Select(y => y.AcademicYear.ToString("D2")).FirstOrDefault(),
                    x.code,
                    x.course,
                    x.credits,
                    x.grade,
                    x.term,
                    x.status,
                    x.withdrawn
                })
                .OrderBy(x => x.year)
                .ThenBy(x => x.code)
                .ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<AcademicHistoryCourseTemplate>> GetAcademicHistoryV2Courses(Guid studentId)
        {
            var student = await _context.Students.FindAsync(studentId);

            var query = _context.AcademicHistories
                .Where(x => x.StudentId == studentId)
                .AsNoTracking();

            var academicYearCoursesStudent = await _context.AcademicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId)
                .Select(x => new
                {
                    x.CourseId,
                    x.AcademicYear
                })
                .ToListAsync();

            var recordsFiltered = query.Count();

            var data = await query
                .Select(x => new
                {
                    //Year = academicYearCoursesStudent.Where(y => y.CourseId == x.CourseId).Select(y => y.AcademicYear.ToString("D2")).FirstOrDefault(),
                    x.Course.Code,
                    Course = x.Course.Name,
                    x.Course.Credits,
                    x.Grade,
                    Term = x.Term.Name,
                    x.Approved,
                    x.Withdraw,
                    x.Validated,
                    x.CourseId,
                    x.Type,
                    x.Try,
                    x.Observations,
                    EvaluationReportCode = x.EvaluationReport.Code,
                    EvaluationReportCreatedAt = x.EvaluationReport.CreatedAt,
                    EvaluationReportLastGradePublishedDate = x.EvaluationReport.LastGradePublishedDate,
                    EvaluationReportReceptionDate = x.EvaluationReport.ReceptionDate
                })
                .ToListAsync();

            var equivalences = await _context.CourseEquivalences
                 .Where(x => x.NewAcademicYearCourse.CurriculumId == student.CurriculumId)
                 .Select(x => new
                 {
                     x.NewAcademicYearCourse.CourseId,
                     OldCourseId = x.OldAcademicYearCourse.CourseId,
                     OldCourseCode = x.OldAcademicYearCourse.Course.Code,
                     Curriculum = x.OldAcademicYearCourse.Curriculum.Code
                 })
                 .ToListAsync();

            var evaluationReportDateConfi = Convert.ToByte(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_FORMAT_DATE));

            var model = data
                .Select(x => new AcademicHistoryCourseTemplate
                {
                    AcademicYear = academicYearCoursesStudent.Where(y => y.CourseId == x.CourseId).Select(y => y.AcademicYear).FirstOrDefault(),
                    Year = academicYearCoursesStudent.Where(y => y.CourseId == x.CourseId).Select(y => y.AcademicYear.ToString("D2")).FirstOrDefault(),
                    Code = x.Code,
                    Course = x.Course,
                    Credits = x.Credits,
                    Grade = x.Grade,
                    Term = x.Term,
                    Approved = x.Approved,
                    Withdraw = x.Withdraw,
                    Status = x.Withdraw ? "Retirado" : x.Approved ? "Aprobado" : "Reprobado",
                    Validated = x.Validated,
                    Observations = x.Validated ? string.Join(", ", equivalences.Where(y => x.CourseId == y.CourseId).Select(y => y.OldCourseCode + "-" + y.Curriculum)) : "",
                    AcademicObservations = x.Observations,
                    Type = x.Type,
                    Try = x.Try,
                    EvaluationReportCode = x.EvaluationReportCode,
                    EvaluationReportDate = 
                    evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.CreatedAt ? x.EvaluationReportCreatedAt : 
                    evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.ReceptionDate ? x.EvaluationReportReceptionDate : 
                    evaluationReportDateConfi == ConstantHelpers.Intranet.EvaluationReportConfigurationFormatDate.LastGradePublished ? x.EvaluationReportLastGradePublishedDate : null
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Code)
                .ToList();

            return model;
        }
        public async Task<AcademicYearCourse> GetWithData(Guid id)
        {
            var result = await _context.AcademicYearCourses
                .Where(y => y.Id == id)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<object> GetCoursesByCurriculum(Guid curriculumId, byte? academicYear = null)
        {
            var query = _context.AcademicYearCourses
                .Where(x => x.CurriculumId == curriculumId)
                .AsNoTracking();

            if (academicYear.HasValue) query = query.Where(x => x.AcademicYear == academicYear);

            var result = await query
                .OrderBy(x => x.Course.Name)
                .Select(x => new
                {
                    id = x.CourseId,
                    text = x.Course.Name
                }).ToListAsync();

            return result;
        }

        public async Task<object> GetCareerAcademicYear(Guid id)
        {
            var result = await _context.AcademicYearCourses
                .Where(ay => ay.Curriculum.CareerId == id)
                .Select(ay => new
                {
                    id = ay.AcademicYear,
                    text = ay.AcademicYear > 0 && ay.AcademicYear <= 20 ? ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[ay.AcademicYear] : "-"
                })
                .OrderBy(x => x.id)
                .ToListAsync();

            result = result.Distinct().ToList();

            return result;
        }

        public async Task<object> GetCurriculumAcademicYearsJson(Guid curriculumId, ClaimsPrincipal user = null)
        {
            var result = await _context.AcademicYearCourses
            .Where(ay => ay.CurriculumId == curriculumId)
            .OrderBy(x => x.AcademicYear)
            .Select(ay => new
            {
                id = ay.AcademicYear,
                text = ay.AcademicYear > 0 && ay.AcademicYear <= 20 ? ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[ay.AcademicYear] : "-"
            })
            .ToListAsync();

            result = result.Distinct().OrderBy(x => x.id).ToList();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR_GENERAL))
                {
                    var configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.TeacherManagement.CAREER_DIRECTOR_GENERAL_ACADEMIC_YEAR).FirstOrDefaultAsync();
                    if (configuration != null)
                    {
                        Int32.TryParse(configuration.Value, out var maxAcademicYear);
                        result = result.Where(x => x.id <= maxAcademicYear).ToList();
                    }
                }
            }

            return result;
        }

        public async Task<object> GetCurriculumAcademicYearsAsSelect2(Guid? careerId = null, Guid? curriculumId = null, bool isActive = false, string coordinatorId = null)
        {
            var query = _context.AcademicYearCourses
                .AsNoTracking();

            if (curriculumId.HasValue && curriculumId != Guid.Empty) query = query.Where(x => x.CurriculumId == curriculumId);

            if (isActive) query = query.Where(x => x.Curriculum.IsActive);

            if (!string.IsNullOrEmpty(coordinatorId))
            {
                var careers = GetCoordinatorCareers(coordinatorId);
                if (careerId.HasValue && careerId != Guid.Empty) careers = careers.Where(x => x == careerId).ToList();
                query = query.Where(ay => careers.Any(y => y == ay.Curriculum.CareerId));
            }
            else if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Curriculum.CareerId == careerId);

            var result = await query
                .Where(x => x.AcademicYear != 0)
                .Select(ay => new
                {
                    id = ay.AcademicYear,
                    text = ay.AcademicYear > 0 && ay.AcademicYear <= 20 ? ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[ay.AcademicYear] : "-"
                })
                .ToListAsync();

            result = result
                .OrderBy(x => x.id)
                .Distinct().ToList();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCurriculumCourses2Datatable(DataTablesStructs.SentParameters sentParameters, Guid curriculumId)
        {
            var query = _context.AcademicYearCourses
                .Where(x => x.CurriculumId == curriculumId && x.IsElective)
                .AsQueryable();

            var pagedList = query.Select(x => new
            {
                id = x.Id,
                courseId = x.CourseId,
                code = x.Course.Code,
                name = x.Course.Name,
                credits = x.Course.Credits,
                year = x.AcademicYear,
                requisite1 = x.PreRequisites.Count > 0 ? x.PreRequisites.Select(y => y.Course.Code).FirstOrDefault() : "---",
                requisite2 = x.PreRequisites.Count > 1 ? x.PreRequisites.Select(y => y.Course.Code).Skip(1).FirstOrDefault() : "---",
                requisite3 = x.PreRequisites.Count > 2 ? x.PreRequisites.Select(y => y.Course.Code).Skip(2).FirstOrDefault() : "---"
            }).OrderBy(x => x.year).ThenBy(x => x.code);

            return await pagedList.ToDataTables<object>(sentParameters);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetCurriculumCourses2Datatable(DataTablesStructs.SentParameters sentParameters, Guid careerId, Guid? academicProgramId, Guid curriculumId, int? cycle, Guid? termId, string groupCode, ClaimsPrincipal user = null)
        {
            try
            {
                if (!termId.HasValue || termId == Guid.Empty)
                    termId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();

                var term = await _context.Terms.FindAsync(termId);

                var query = _context.AcademicYearCourses
                    .Where(x => x.CurriculumId == curriculumId)
                    .AsQueryable();

                if (cycle.HasValue && cycle != 0)
                    query = query.Where(x => x.AcademicYear == cycle);

                    if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
                        query = query.Where(x => x.Curriculum.AcademicProgramId == academicProgramId || x.Curriculum.AcademicProgramId == null);
                
                if (user != null)
                {
                    if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR_GENERAL))
                    {
                        var configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.TeacherManagement.CAREER_DIRECTOR_GENERAL_ACADEMIC_YEAR).FirstOrDefaultAsync();
                        if (configuration != null)
                        {
                            Int32.TryParse(configuration.Value, out var maxAcademicYear);
                            query = query.Where(x => x.AcademicYear <= maxAcademicYear);
                        }
                    }
                }

                var recordsFiltered = await query.CountAsync();

                var dataDb = await query
                    .Select(x => new
                    {
                        id = x.Id,
                        courseId = x.CourseId,
                        code = x.Course.Code,
                        isElective = x.IsElective,
                        name = x.Course.Name,
                        vacancies = _context.Sections.Where(y => y.Code == groupCode && y.CourseTerm.CourseId == x.CourseId && y.CourseTerm.TermId == termId).Select(y => y.Vacancies).FirstOrDefault(),
                        credits = x.Course.Credits,
                        program = x.Course.AcademicProgramId.HasValue ? x.Course.AcademicProgram.Code + " - " + x.Course.AcademicProgram.Name : "---"
                    })
                    .OrderBy(x => x.code)
                    .ThenBy(x => x.name)
                    .ToListAsync();


                var data = dataDb
                    .Select(x => new
                    {
                        x.id,
                        x.courseId,
                        x.code,
                        x.isElective,
                        name = x.isElective ? $"{x.name} (Elect.)" : $"{x.name} (Oblig.)",
                        x.vacancies,
                        x.credits,
                        x.program,
                        finished = term.Status == ConstantHelpers.TERM_STATES.FINISHED
                    })
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .ToList();


                var recordsTotal = data.Count;

                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = data,
                    DrawCounter = sentParameters.DrawCounter,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsFiltered,
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<object> GetCurriculumCoursesDatatableClientSide(Guid curriculumId, Guid? academicProgramId = null, Guid? competencieId = null)
        {
            var prerequisites = await _context.AcademicYearCoursePreRequisites
                .Where(x => x.AcademicYearCourse.CurriculumId == curriculumId)
                .OrderBy(x => x.Course.Code)
                .Select(x => new
                {
                    x.AcademicYearCourseId,
                    x.Course.Code,
                    x.Course.FullName,
                    x.CourseId,
                    x.IsOptional
                }).ToListAsync();

            var certificates = await _context.AcademicYearCourseCertificates
                .Where(x => x.AcademicYearCourse.CurriculumId == curriculumId)
                .Select(x => new
                {
                    x.AcademicYearCourseId,
                    x.CourseCertificateId,
                    x.CourseCertificate.Name
                }).ToListAsync();

            var query = _context.AcademicYearCourses
                .Where(x => x.CurriculumId == curriculumId).AsQueryable();

            if (academicProgramId != null)
                query = query.Where(x => x.Course.AcademicProgramId == academicProgramId);

            if (competencieId.HasValue)
                query = query.Where(x => x.CompetencieId == competencieId);

            var dataDB = await query
                .OrderBy(x => x.AcademicYear)
                .Select(x => new
                {
                    id = x.Id,
                    year = x.AcademicYear,
                    code = x.Course.Code,
                    course = x.Course.Name,
                    academicProgram = x.Course.AcademicProgram.Name,
                    credits = x.Course.Credits,
                    requiredCredits = x.RequiredCredits,
                    isElective = x.IsElective,
                    competencieId = x.CompetencieId,
                    competencie = x.Competencie.Name,
                    isExonerable = x.IsExonerable
                })
                .ToListAsync();

            var courses = dataDB
                .Select(x => new
                {
                    order = x.year,
                    x.id,
                    yearCredits = $"Ciclo académico {x.year} ({dataDB.Where(y=>y.year == x.year && !y.isElective).Sum(y=>y.credits)} crd. obli. - {dataDB.Where(y => y.year == x.year && y.isElective).Sum(y => y.credits)} crd. elec.)",
                    year = $"Ciclo académico {x.year}",
                    x.code,
                    x.course,
                    x.academicProgram,
                    x.credits,
                    x.requiredCredits,
                    x.isElective,
                    requisites = string.Join(", ", prerequisites.Where(y => y.AcademicYearCourseId == x.id).Select(y => y.Code).ToList()),
                    certificates = certificates.Where(y => y.AcademicYearCourseId == x.id).Select(y => y.CourseCertificateId).ToArray(),
                    requisitesIds = prerequisites.Where(y => y.AcademicYearCourseId == x.id && !y.IsOptional).Select(y => $"{y.CourseId}|{y.FullName}").ToList(),
                    optionalRequisitesIds = prerequisites.Where(y => y.AcademicYearCourseId == x.id && y.IsOptional).Select(y => $"{y.CourseId}|{y.FullName}").ToList(),
                    requisite1 = prerequisites.Where(y => y.AcademicYearCourseId == x.id).Count() > 0 ? prerequisites.Where(y => y.AcademicYearCourseId == x.id).FirstOrDefault().Code : "---",
                    requisite2 = prerequisites.Where(y => y.AcademicYearCourseId == x.id).Count() > 1 ? prerequisites.Where(y => y.AcademicYearCourseId == x.id).Skip(1).FirstOrDefault().Code : "---",
                    requisite3 = prerequisites.Where(y => y.AcademicYearCourseId == x.id).Count() > 2 ? prerequisites.Where(y => y.AcademicYearCourseId == x.id).Skip(2).FirstOrDefault().Code : "---",
                    count = prerequisites.Where(y => y.AcademicYearCourseId == x.id).Count(),
                    x.competencieId,
                    x.competencie,
                    x.isExonerable
                })
                .OrderBy(x => x.order)
                .ThenBy(x => x.code)
                .ToList();

            var result = new
            {
                data = courses
            };

            return result;
        }


        public async Task<object> GetCurriculumCourseTermsDatatableClientSide(Guid curriculumId, Guid? termId = null)
        {
            if (termId == null)
            {
                var term = await _context.Terms.Where(x => x.Status != ConstantHelpers.TERM_STATES.INACTIVE).OrderBy(x => x.Status).ThenByDescending(x => x.Name).FirstOrDefaultAsync();
                if (term != null) termId = term.Id;
            }

            var query = await _context.CourseTerms
                .Where(x => x.TermId == termId && x.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId))
                 .Select(x => new
                 {
                     id = x.Id,
                     year = x.Course.AcademicYearCourses.FirstOrDefault(y => y.CurriculumId == curriculumId).AcademicYear,
                     code = x.Course.Code,
                     course = x.Course.Name,
                     academicProgram = x.Course.AcademicProgramId.HasValue ? x.Course.AcademicProgram.Name : "---",
                     credits = x.Course.Credits,
                     isElective = x.Course.AcademicYearCourses.FirstOrDefault(y => y.CurriculumId == curriculumId).IsElective,
                     teachers = x.Sections.Select(y => y.TeacherSections.Select(z => z.Teacher.User.FullName).ToList()).ToList(),
                     modality = x.Modality
                 })
                .ToListAsync();

            var courses = query
                .Select(x => new
                {
                    order = x.year,
                    x.id,
                    year = $"Ciclo académico {x.year}",
                    x.code,
                    x.course,
                    x.academicProgram,
                    x.credits,
                    x.isElective,
                    teachers = string.Join(", ", x.teachers.Where(y => y.Count != 0).Select(y => string.Join(", ", y)).ToList()),
                    x.modality
                })
                .OrderBy(x => x.order)
                .ThenBy(x => x.code)
                .ToList();

            var result = new
            {
                data = courses
            };

            return result;
        }

        public async Task<IEnumerable<CourseTermTemplate>> GetCurriculumCourseTermsAsModelA(Guid curriculumId, Guid? termId = null)
        {
            if (termId == null)
            {
                var term = await _context.Terms.Where(x => x.Status != ConstantHelpers.TERM_STATES.INACTIVE).OrderBy(x => x.Status).ThenByDescending(x => x.Name).FirstOrDefaultAsync();
                if (term != null) termId = term.Id;
            }

            var query = await _context.CourseTerms
                .Where(x => x.TermId == termId && x.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId))
                 .Select(x => new
                 {
                     id = x.Id,
                     year = x.Course.AcademicYearCourses.FirstOrDefault(y => y.CurriculumId == curriculumId).AcademicYear,
                     code = x.Course.Code,
                     course = x.Course.Name,
                     academicProgram = x.Course.AcademicProgramId.HasValue ? x.Course.AcademicProgram.Name : "---",
                     credits = x.Course.Credits,
                     isElective = x.Course.AcademicYearCourses.FirstOrDefault(y => y.CurriculumId == curriculumId).IsElective,
                     teachers = x.Sections.Select(y => y.TeacherSections.Select(z => z.Teacher.User.FullName).ToList()).ToList(),
                     modality = x.Modality
                 })
                .ToListAsync();

            var courses = query
                .Select(x => new CourseTermTemplate
                {
                    Code = x.code,
                    Name = x.course,
                    AcademicYear = x.year,
                    Credits = x.credits,
                    AcademicProgram = x.academicProgram,
                    Modality = ConstantHelpers.Course.Modality.VALUES[x.modality],
                    Teachers = string.Join(", ", x.teachers.Where(y => y.Count != 0).Select(y => string.Join(", ", y)).ToList())
                })
                .OrderBy(x => x.AcademicYear)
                .ThenBy(x => x.Code)
                .ToList();

            return courses;
        }

        public async Task<object> GetStudentCourses(Guid id, string userId, int currentAcademicYear, Guid curriculumId)
        {
            return await (from ay in _context.AcademicYearCourses
                          where ay.CurriculumId == curriculumId
                                && !ay.Course.AcademicHistories.Any(ah => ah.Approved && ah.Student.UserId == userId)
                                && ay.AcademicYear <= currentAcademicYear
                          from s in _context.Sections.Where(s => s.CourseTerm.CourseId == ay.CourseId && s.GroupId != null && s.GroupId == id).DefaultIfEmpty()
                          select new
                          {
                              id = ay.Course.Id,

                              code = ay.Course.Code,
                              course = ay.Course.Name,
                              credits = ay.Course.Credits,

                              section = (s != null ? s.Code : " - "),
                              vacancies = (s != null ? (s.Vacancies - s.StudentSections.Count) : -1)
                          }).ToListAsync();
        }
        public async Task<IEnumerable<AcademicYearCourse>> GetWithHistoriesByCurriculumAndStudent(Guid curriculumId, Guid studentId)
        {
            var query = _context.AcademicYearCourses.AsNoTracking();
            var student = await _context.Students.Include(x => x.Career).Where(x => x.Id == studentId).FirstOrDefaultAsync();

            if (student.AcademicProgramId.HasValue)
                query = query.Where(x => !x.Course.AcademicProgramId.HasValue || (x.Course.AcademicProgramId.HasValue && (x.Course.AcademicProgramId == student.AcademicProgramId || x.Course.AcademicProgram.Code == "00")));
            else
                query = query.Where(x => !x.Course.AcademicProgramId.HasValue);

            var dbData = query
               .Select(x => new
               {
                   x.AcademicYear,
                   CourseCode = x.Course.Code,
                   CourseName = x.Course.Name,
                   x.Course.Credits,
                   x.CourseId,
                   AcademicProgram = x.Course.AcademicProgramId.HasValue ? x.Course.AcademicProgram.Name : "00",
                   x.IsElective
               })
               .ToList();

            var courses = await query
            .Where(x => x.CurriculumId == curriculumId)
            .OrderBy(x => x.AcademicYear)
            .Select(x => new AcademicYearCourse
            {
                AcademicYear = x.AcademicYear,
                CourseId = x.CourseId,
                IsElective = x.IsElective,
                CompetencieId = x.CompetencieId,
                Course = new Course
                {
                    Id = x.Course.Id,
                    Name = x.Course.Name,
                    Code = x.Course.Code,
                    Credits = x.Course.Credits,
                    AcademicHistories = x.Course.AcademicHistories
                    .Where(ah => ah.StudentId == studentId)
                    .OrderBy(ah => ah.Term.StartDate)
                    .Select(ah => new AcademicHistory
                    {
                        Approved = ah.Approved,
                        Validated = ah.Validated,
                        Grade = ah.Grade,
                        TermId = ah.TermId,
                        Term = new Term
                        {
                            Name = ah.Term.Name,
                            Number = ah.Term.Number,
                            Year = ah.Term.Year,
                            StartDate = ah.Term.StartDate
                        },
                        Withdraw = ah.Withdraw,
                        SectionId = ah.SectionId,
                        EvaluationReportId = ah.EvaluationReportId,
                        EvaluationReport = ah.EvaluationReport,
                        Observations = ah.Observations
                    }).ToList(),
                    EvaluationReports = x.Course.EvaluationReports
                }
            }).ToListAsync();

            return courses;
        }

        public async Task<object> GetAllAcademicYearByCareerId(Guid careerId)
        {

            var data = await _context.AcademicYearCourses
                .Where(x => x.Curriculum.CareerId == careerId && x.AcademicYear != 0)
                .Select(x => new
                {
                    id = (int)x.AcademicYear,
                    text = x.AcademicYear > 0 && x.AcademicYear <= 20 ? ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[x.AcademicYear] : "-"
                })
                .Distinct()
                .ToListAsync();

            var result = data.OrderBy(x => x.id).ToList();

            return result;
        }

        public async Task<List<AcademicProgramByCurriculumTemplate>> GetAcademicProgramByCurriculum(Guid curriculumId, Guid? academicProgramId = null)
        {
            var query = _context.AcademicYearCourses
                .Where(x => x.CurriculumId == curriculumId)
                .AsQueryable();

            if (academicProgramId != null)
                query = query.Where(x => x.Course.AcademicProgramId == academicProgramId);

            var result = await query
                .Select(x => new AcademicProgramByCurriculumTemplate
                {
                    Text = x.Course.AcademicProgram.Code + " - " + x.Course.AcademicProgram.Name,
                    Id = x.Course.AcademicProgramId.Value
                })
                .Distinct()
                .ToListAsync();

            return result;

        }

        public async Task<object> GetCareerAcademicProgramsByCurriculumSelect2(Guid curriculumId)
        {
            var curriculum = await _context.Curriculums.FindAsync(curriculumId);
            var query = _context.AcademicYearCourses
                .Where(x => x.CurriculumId == curriculumId && x.Course.AcademicProgram.CareerId == curriculum.CareerId)
                .AsQueryable();

            //var courses = await query
            //    .Select(x => new
            //    {
            //        x.CourseId,
            //        x.Course.Code,
            //        x.Course.Name,
            //        x.Course.CareerId,
            //        text = x.Course.AcademicProgram.Code + " - " + x.Course.AcademicProgram.Name,
            //        id = x.Course.AcademicProgramId.Value
            //    })
            //    .ToListAsync();

            var result = await query
                .Select(x => new
                {
                    text = x.Course.AcademicProgram.Code + " - " + x.Course.AcademicProgram.Name,
                    id = x.Course.AcademicProgramId.Value
                })
                .Distinct()
                .ToListAsync();

            return result;

        }

        public async Task<IEnumerable<AcademicYearCourseTemplateZ>> GetAllAsModelZById(Guid curriculumId, Guid? academicProgramId = null)
        {
            var query = _context.AcademicYearCourses.AsQueryable();

            query = query.Where(x => x.CurriculumId == curriculumId);

            if (academicProgramId != null)
                query = query.Where(x => x.Course.AcademicProgramId == academicProgramId);

            var curriculumList = await query
                .Select(x => new AcademicYearCourseTemplateZ
                {
                    //Id = x.Course.Id,
                    Code = x.Course.Code,
                    Name = x.Course.Name,
                    Speciality = x.Course.AcademicProgram.Code,
                    //Type = x.IsElective ? "E" : "O",
                    Cycle = x.AcademicYear,
                    Credit = x.Course.Credits,
                    RequeridCredit = x.RequiredCredits,
                    //Area = x.Course.Area.Name,
                    //Regularized = "Sem",
                    TotalHours = x.Course.TotalHours,
                    PlannedHours = x.Course.TheoreticalHours,
                    PracticalHours = x.Course.PracticalHours,
                    SeminarHours = x.Course.SeminarHours,
                    VirtualHours = x.Course.VirtualHours,
                    Requirement = x.PreRequisites.Any() ? string.Join(", ", x.PreRequisites.Select(y => y.Course.Code).ToList()) : "NINGUNO"
                    //AcademicYearNumber = x.AcademicYear.Number
                })
                .OrderBy(x => x.Cycle).ThenBy(x => x.Code)
                .ToListAsync();

            return curriculumList;
        }

        public async Task<object> GetCoursesByCurriculumActive()
        {
            return await _context.AcademicYearCourses.Where(x => x.Curriculum.IsActive)
                 .Select(x => new
                 {
                     NameFaculties = x.Curriculum.Career.Faculty.Name,
                     IdCareer = x.Curriculum.Career.Id,
                     NameCareer = x.Curriculum.Career.Name,
                     NameCurriculum = x.Curriculum.Code,
                 })
                 .Distinct()
                 .OrderBy(x => x.NameFaculties)
                 .ToListAsync();
        }

        public async Task<List<CurriculumTemplate>> GetCurriculumsByCareerId(Guid id)
        {
            return await _context.AcademicYearCourses
                .Where(x => x.Curriculum.Career.Id == id && x.Curriculum.IsActive)
                .Select(x => new CurriculumTemplate
                {
                    Cycle = x.AcademicYear,
                    CodeCourse = x.Course.Code,
                    Credits = x.Course.Credits,
                    PracticalHours = x.Course.PracticalHours,
                    SeminarHours = x.Course.SeminarHours,
                    TheoreticalHours = x.Course.TheoreticalHours,
                    VirtualHours = x.Course.VirtualHours,
                    NameCourse = x.Course.Name,
                    Requisites = _context.AcademicYearCoursePreRequisites.Where(s => s.AcademicYearCourseId == x.Id).Select(s => new PreRequisiteTemplate { Name = s.Course.Name }).ToList()
                })
                .OrderBy(x => x.Cycle)
                .ToListAsync();
        }
        public async Task<List<byte>> GetAcademicYearsByCurriculumId(Guid curriculumId)
        {
            return await _context.AcademicYearCourses.Where(x => x.CurriculumId == curriculumId).OrderBy(x => x.AcademicYear).Select(x => x.AcademicYear).Distinct().ToListAsync();
        }
        public async Task<List<AcademicYearCourse>> GetAcademicYearsByCurriculumIdAndAcademicYear(Guid curriculumId, byte academicYear)
        {
            return await _context.AcademicYearCourses
                .Where(x => x.CurriculumId == curriculumId && x.AcademicYear == academicYear)
                .OrderBy(x => x.AcademicYear)
                .Include(x => x.Course)
                .ToListAsync();
        }

        public async Task UpdateAcademicYearCoursesJob()
        {
            var courses = await _context.AcademicYearCourses
                            .ToListAsync();

            foreach (var course in courses)
            {
                course.AcademicYear = course.AcademicYear;
                course.CurriculumId = course.CurriculumId;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<AcademicYearCourse> GetFirstByCurriculumAndCode(Guid curriculumId, string courseCode)
        {
            return await _context.AcademicYearCourses.Where(x => x.CurriculumId == curriculumId && x.Course.Code == courseCode).FirstOrDefaultAsync();
        }

        public async Task<object> GetDataDetailById(Guid id, string groupCode, Guid? termId = null)
        {
            if (!termId.HasValue)
            {
                var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
                termId = term.Id;
            }

            var data = await _context.AcademicYearCourses.Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Course.AcademicProgram.Name,
                    sectionDetail = _context.Sections.Include(y => y.StudentSections).Where(y => y.Code == groupCode && y.CourseTerm.CourseId == x.CourseId && y.CourseTerm.TermId == termId).Select(y => new { y.Vacancies, enrolled = y.StudentSections.Count() }).FirstOrDefault(),
                })
                .FirstOrDefaultAsync();

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesToManageSyllabus(DataTablesStructs.SentParameters parameters, Guid termId, Guid? careerId = null, Guid? academicProgramId = null, Guid? curriculumId = null, int? cycle = null, ClaimsPrincipal user = null, string searchValue = null, byte? status = null)
        {

            var request = await _context.SyllabusRequests.FirstOrDefaultAsync(x => x.TermId == termId);
            var query = _context.AcademicYearCourses.Where(x => x.Course.CourseTerms.Any(y => y.TermId == request.TermId && y.Sections.Any(y => y.CourseTerm.TermId == request.TermId))).AsQueryable();

            Expression<Func<AcademicYearCourse, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Course.Code); break;
                case "1":
                    orderByPredicate = ((x) => x.Course.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.Course.Area.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.Course.Career.Name); break;
                case "4":
                    orderByPredicate = ((x) => x.Course.AcademicProgram.Name); break;
                case "5":
                    orderByPredicate = ((x) => x.AcademicYear); break;
                default:
                    query = query.OrderBy(x => x.Curriculum.Code).ThenBy(x => x.AcademicYear); break;
            }

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.Course.CareerId == careerId);

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
                query = query.Where(x => x.Course.AcademicProgramId == academicProgramId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.CurriculumId == curriculumId);

            if (cycle.HasValue && cycle != 0)
                query = query.Where(x => x.AcademicYear == cycle);

            if (status.HasValue && status != 0)
            {
                if (ConstantHelpers.SYLLABUS_TEACHER.STATUS.VALUES.ContainsKey(status.Value))
                {
                    query = query.Where(x => x.Course.CourseTerms.Any(y => y.TermId == termId && y.SyllabusTeachers.Any(z => z.Status == status)));
                }
                else
                {
                    query = query.Where(x => x.Course.CourseTerms.Any(y => y.TermId == termId && !y.SyllabusTeachers.Any()));
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Course.Name.ToLower().Contains(searchValue.Trim().ToLower()));

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    var careers = await _context.Careers.Where(x =>
                    x.CareerDirectorId == userId ||
                    x.AcademicCoordinatorId == userId ||
                    x.AcademicSecretaryId == userId)
                        .Select(x => x.Id).ToListAsync();

                    query = query.Where(x => x.Course.CareerId.HasValue && careers.Contains(x.Course.CareerId.Value));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    var careerIds = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId).Select(x => x.CareerId).ToListAsync();
                    query = query.Where(x => careerIds.Contains(x.Course.CareerId));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR_GENERAL))
                {
                    var configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.TeacherManagement.CAREER_DIRECTOR_GENERAL_ACADEMIC_YEAR).FirstOrDefaultAsync();
                    if (configuration != null)
                    {
                        Int32.TryParse(configuration.Value, out var maxAcademicYear);
                        query = query.Where(x => x.AcademicYear <= maxAcademicYear);
                    }
                }
            }

            var syllabusTeachers = _context.SyllabusTeachers.Include(x => x.CourseTerm.Course).Include(x => x.CourseTerm.Term).Where(x => x.SyllabusRequest.TermId == termId);

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    courseId = x.CourseId,
                    curriculumId = x.CurriculumId,
                    curriculum = x.Curriculum.Name,
                    termId,
                    code = x.Course.Code,
                    name = x.Course.Name,
                    area = string.IsNullOrEmpty(x.Course.Area.Name) ? "-" : x.Course.Area.Name,
                    career = x.Course.Career.Name,
                    academicProgram = string.IsNullOrEmpty(x.Course.AcademicProgram.Name) ? "-" : x.Course.AcademicProgram.Name,
                    cycle = ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS.ContainsKey(x.AcademicYear) ? ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[x.AcademicYear] : "-",
                    syllabusStatus = _context.SyllabusTeachers.Where(y => y.CourseTerm.CourseId == x.Course.Id && y.CourseTerm.TermId == termId).Select(y => y.Status).FirstOrDefault(),
                    syllabus = _context.SyllabusTeachers.Where(y => y.CourseTerm.CourseId == x.Course.Id && y.CourseTerm.TermId == termId).Select(y => new { y.IsDigital, y.Id, outOfDate = y.PresentationDate.HasValue ? y.PresentationDate.Value.ToDefaultTimeZone().Date > request.End.Date : false }).FirstOrDefault(),
                    onlyDigital = request.Type == ConstantHelpers.SYLLABUS_REQUEST.TYPE.DIGITAL
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<int> GetMaxAcademic(Guid curriculinId)
        {
            var academicYear = await _context.AcademicYearCourses.Where(x => x.CurriculumId == curriculinId).ToListAsync();
            var result = academicYear.Max(x => x.AcademicYear);

            return result;
        }

        public async Task<object> GetAllSelect2()
        {
            var result = await _context.AcademicYearCourses
                .Where(ay => ay.AcademicYear != 0)
                    .OrderBy(ay => ay.AcademicYear)
                    .Select(ay => new
                    {
                        id = ay.AcademicYear,
                        text = ay.AcademicYear > 0 && ay.AcademicYear <= 20 ? ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[ay.AcademicYear] : "-"
                    })
                    .Distinct()
                    .ToListAsync();

            result.Insert(0, new { id = new byte(), text = "Todas" });

            return result.OrderBy(x => x.id);
        }

        public async Task<List<CourseSyllabusCompliance>> GetCoursesToManageSyllabusCompliance(Guid termId, Guid? careerId, Guid? academicProgramId, Guid? curriculumId, int? cycle, ClaimsPrincipal user, byte? status)
        {
            var request = await _context.SyllabusRequests.FirstOrDefaultAsync(x => x.TermId == termId);
            var query = _context.AcademicYearCourses.Where(x => x.Course.CourseTerms.Any(y => y.TermId == request.TermId && y.Sections.Any(y => y.CourseTerm.TermId == request.TermId))).AsNoTracking();

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.Course.CareerId == careerId);

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
                query = query.Where(x => x.Course.AcademicProgramId == academicProgramId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.CurriculumId == curriculumId);

            if (cycle.HasValue && cycle != 0)
                query = query.Where(x => x.AcademicYear == cycle);

            if (status.HasValue && status != 0)
            {
                if (status == ConstantHelpers.SYLLABUS_TEACHER.STATUS.IN_PROCESS || status == ConstantHelpers.SYLLABUS_TEACHER.STATUS.PRESENTED)
                {
                    query = query.Where(x => x.Course.CourseTerms.Any(y => y.TermId == termId && y.SyllabusTeachers.Any(z => z.Status == status)));
                }
                else
                {
                    query = query.Where(x => x.Course.CourseTerms.Any(y => y.TermId == termId && !y.SyllabusTeachers.Any()));
                }
            }


            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    var careers = await _context.Careers.Where(x =>
                    x.CareerDirectorId == userId ||
                    x.AcademicCoordinatorId == userId ||
                    x.AcademicSecretaryId == userId)
                        .Select(x => x.Id).ToListAsync();

                    query = query.Where(x => x.Course.CareerId.HasValue && careers.Contains(x.Course.CareerId.Value));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    var careerIds = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId).Select(x => x.CareerId).ToListAsync();
                    query = query.Where(x => careerIds.Contains(x.Course.CareerId));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR_GENERAL))
                {
                    var configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.TeacherManagement.CAREER_DIRECTOR_GENERAL_ACADEMIC_YEAR).FirstOrDefaultAsync();
                    if (configuration != null)
                    {
                        Int32.TryParse(configuration.Value, out var maxAcademicYear);
                        query = query.Where(x => x.AcademicYear <= maxAcademicYear);
                    }
                }
            }

            var syllabusTeachers = _context.SyllabusTeachers.Include(x => x.CourseTerm.Course).Include(x => x.CourseTerm.Term).Where(x => x.SyllabusRequest.TermId == termId);

            var data = await query
                .Select(x => new CourseSyllabusCompliance
                {
                    CourseId = x.CourseId,
                    CourseCode = x.Course.Code,
                    Course = x.Course.Name,
                    AcademicYear = ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS.ContainsKey(x.AcademicYear) ? ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[x.AcademicYear] : "-",
                    Career = x.Course.Career.Name,
                    Curriculum = x.Curriculum.Code,
                    //Status = syllabusTeachers.Where(y => y.CourseTerm.CourseId == x.Course.Id && y.CourseTerm.TermId == termId).Select(y => y.Status).FirstOrDefault(),
                    //OutOfDate = syllabusTeachers.Where(y => y.CourseTerm.CourseId == x.Course.Id && y.CourseTerm.TermId == termId).Select(y => y.PresentationDate.Value.ToDefaultTimeZone().Date > request.End.Date).FirstOrDefault()
                })
                .ToListAsync();

            foreach (var item in data)
            {
                item.Status = syllabusTeachers.Where(y => y.CourseTerm.CourseId == item.CourseId && y.CourseTerm.TermId == termId).Select(y => y.Status).FirstOrDefault();
                item.OutOfDate = syllabusTeachers.Where(y => y.CourseTerm.CourseId == item.CourseId && y.CourseTerm.TermId == termId).Select(y => y.PresentationDate.Value.ToDefaultTimeZone().Date > request.End.Date).FirstOrDefault();
            }

            return data;

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrollmentCurriculumCoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid id)
        {
            var student = await _context.Students
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.CurriculumId,
                    Curriculum = x.Curriculum.Code,
                    CareerCode = x.Career.Code,
                    x.AcademicProgramId,
                    x.CurrentAcademicYear,
                    x.CareerId,
                    x.User.UserName
                }).FirstOrDefaultAsync();

            var academicHistories = await _context.AcademicHistories
                .Where(x => x.StudentId == id && !x.Withdraw)
                .Include(x => x.Term).Include(x => x.EvaluationReport)
                .OrderBy(x => x.Term.Name)
                .ToListAsync();


            var availableCoursesHash = new HashSet<Guid>();
            #region Cursos disponibles
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            if (term != null)
            {
                var academicYearDispersion = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.ACADEMIC_YEAR_DISPERSION);
                var irregularDissaprovedCoursesLimit = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.IRREGULAR_DISAPPROVED_COURSES_LIMIT);

                var enrollmentTurn = await _context.EnrollmentTurns.FirstOrDefaultAsync(x => x.StudentId == student.Id && x.TermId == term.Id);
                var academicYearCoursesQry = _context.AcademicYearCourses
                    .Where(x => x.CurriculumId == student.CurriculumId)
                    .AsNoTracking();

                academicYearCoursesQry = academicYearCoursesQry.Where(x => !x.Course.AcademicProgramId.HasValue || x.Course.AcademicProgramId == student.AcademicProgramId || x.Course.AcademicProgram.Code == "00");

                var academicYearCourses = await academicYearCoursesQry
                    .Select(x => new
                    {
                        x.CourseId,
                        x.AcademicYear,
                        x.Course.AcademicProgramId,
                        AcademicProgramCode = x.Course.AcademicProgramId.HasValue ? x.Course.AcademicProgram.Code : "--",
                        AcademicProgramName = x.Course.AcademicProgramId.HasValue ? x.Course.AcademicProgram.Name : "--",
                        PreRequisites = x.PreRequisites
                            .Select(x => new
                            {
                                x.CourseId,
                                x.Course.Code,
                                x.IsOptional
                            }).ToList(),
                        CourseCode = x.Course.Code,
                        CourseName = x.Course.Name,
                        x.Course.Credits,
                        x.IsElective,
                        x.RequiredCredits,
                        curriculum = x.Curriculum.Code,
                        HasSections = x.Course.CourseTerms.Any(y => y.TermId == term.Id)
                    }).ToListAsync();

                var approvedCredits = academicYearCourses.Where(x => academicHistories.Any(y => y.CourseId == x.CourseId && y.Approved)).Sum(x => x.Credits);

                var availableCoursesQry = academicYearCourses
                    .Where(x => !academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved));

                if (enrollmentTurn != null && !enrollmentTurn.SpecialEnrollment)
                    availableCoursesQry = availableCoursesQry.Where(x => x.AcademicYear <= student.CurrentAcademicYear + academicYearDispersion);

                var parallelCoursesLimit = await _context.CareerParallelCourses
                        .Where(x => x.CareerId == student.CareerId && x.AcademicYear == student.CurrentAcademicYear && x.AppliesForStudents)
                        .FirstOrDefaultAsync();

                if (parallelCoursesLimit != null)
                {
                    availableCoursesQry = availableCoursesQry.Where(x =>
                    ((x.PreRequisites.Where(p => !p.IsOptional).All(p => academicHistories.Any(h => h.CourseId == p.CourseId && h.Approved))
                    && (!x.PreRequisites.Where(p => p.IsOptional).Any() || x.PreRequisites.Where(p => p.IsOptional).Any(p => academicHistories.Any(h => h.CourseId == p.CourseId && h.Approved))))
                    || x.AcademicYear <= parallelCoursesLimit.AcademicYear)
                    && x.RequiredCredits <= approvedCredits);
                }
                else availableCoursesQry = availableCoursesQry
                        .Where(x => x.PreRequisites.Where(x => !x.IsOptional).All(p => academicHistories.Any(h => h.CourseId == p.CourseId && h.Approved))
                        && (!x.PreRequisites.Where(p => p.IsOptional).Any() || x.PreRequisites.Where(x => x.IsOptional).Any(p => academicHistories.Any(h => h.CourseId == p.CourseId && h.Approved)))
                        && x.RequiredCredits <= approvedCredits);

                var academicHistoriesHash = new List<Guid>();

                if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD)
                    academicHistoriesHash = academicHistories.Where(x => x.Term.Number != "A" && !x.Term.IsSummer).Select(x => x.CourseId).ToList();
                else academicHistoriesHash = academicHistories.Where(x => x.Term.Number != "A" && x.Type != ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION).Select(x => x.CourseId).ToList();

                var availableCourses = availableCoursesQry
                    .Select(a => new
                    {
                        Id = a.CourseId,
                        AcademicYear = a.AcademicYear,
                        Time = academicHistoriesHash.Count(ah => a.CourseId == ah) + 1,
                        HasSections = a.HasSections,
                        IsElective = a.IsElective,
                        Code = a.CourseCode,
                        AcademicProgram = a.AcademicProgramId.HasValue ? a.AcademicProgramName : "--"
                    }).ToList();

                if (term.IsSummer)
                {
                    var coursesToRemove = availableCourses.Where(x => x.AcademicYear >= student.CurrentAcademicYear && x.Time == 1).ToList();
                    foreach (var item in coursesToRemove)
                        availableCourses.Remove(item);
                }

                if (availableCourses.Where(x => x.Time > 1).Count() > irregularDissaprovedCoursesLimit)
                {
                    foreach (var item in availableCourses.Where(x => x.Time == 1).ToList())
                        availableCourses.Remove(item);
                }

                if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD && int.Parse(student.UserName.Substring(0, 3)) <= 141)
                {
                }
                else
                {
                    if (availableCourses.Any(x => x.Time >= 4))
                    {
                        if (term.IsSummer)
                        {
                            availableCourses.Clear();
                        }
                        else
                        {
                            availableCourses = availableCourses.Where(x => x.Time >= 4).ToList();
                        }
                    }
                }

                availableCourses = availableCourses.Where(x => x.HasSections).ToList();

                availableCoursesHash = availableCourses.Select(x => x.Id).ToHashSet();
            }
            #endregion

            var query = _context.AcademicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId)
                .Include(x => x.Course.AcademicProgram)
                .AsNoTracking();

            if (student.AcademicProgramId.HasValue)
                query = query.Where(x => !x.Course.AcademicProgramId.HasValue || (x.Course.AcademicProgramId.HasValue && (x.Course.AcademicProgramId == student.AcademicProgramId || x.Course.AcademicProgram.Code == "00")));
            else
                query = query.Where(x => !x.Course.AcademicProgramId.HasValue);

            var dbData = query
               .Select(x => new
               {
                   x.AcademicYear,
                   CourseCode = x.Course.Code,
                   CourseName = x.Course.Name,
                   x.Course.Credits,
                   x.CourseId,
                   AcademicProgram = x.Course.AcademicProgramId.HasValue ? x.Course.AcademicProgram.Name : "00"
               })
               .ToList();

            var courses = dbData
               .OrderBy(x => x.AcademicYear)
               .ThenBy(x => x.CourseCode)
               .Select(x => new
               {
                   yearName = $"SEMESTRE : {ConstantHelpers.ACADEMIC_YEAR.TEXT[x.AcademicYear]}",
                   year = x.AcademicYear.ToString("D2"),
                   code = x.CourseCode,
                   course = x.CourseName,
                   credits = x.Credits.ToString("0.0"),
                   id = x.CourseId,
                   academicProgram = x.AcademicProgram,
                   tries = academicHistories.Where(ah => ah.CourseId == x.CourseId && ah.Type != ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION && ah.Type != ConstantHelpers.AcademicHistory.Types.REEVALUATION && ah.Type != ConstantHelpers.AcademicHistory.Types.DEFERRED).Count() == 0 && academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? 1
                   : academicHistories.Where(ah => ah.CourseId == x.CourseId && ah.Type != ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION && ah.Type != ConstantHelpers.AcademicHistory.Types.REEVALUATION && ah.Type != ConstantHelpers.AcademicHistory.Types.DEFERRED).Count(),
                   academicHistoryId = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Grade).Select(ah => ah.Id).FirstOrDefault() : academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Term.Year).ThenByDescending(ah => ah.Term.Number).ThenByDescending(ah => ah.Grade).Select(ah => ah.Id).FirstOrDefault(),
                   grade = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Grade).Select(ah => ah.Grade).FirstOrDefault() : academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Term.Year).ThenByDescending(ah => ah.Term.Number).ThenByDescending(ah => ah.Grade).Select(ah => ah.Grade).FirstOrDefault(),
                   term = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Grade).Select(ah => ah.Term.Name).FirstOrDefault() : academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Term.Year).ThenByDescending(ah => ah.Term.Number).ThenByDescending(ah => ah.Grade).Select(ah => ah.Term.Name).FirstOrDefault(),
                   evaluationReport = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Grade).Select(ah => ah.EvaluationReport?.Code).FirstOrDefault() : academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Term.Year).ThenByDescending(ah => ah.Term.Number).ThenByDescending(ah => ah.Grade).Select(ah => ah.EvaluationReport?.Code).FirstOrDefault(),
                   status = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? "Aprobado" : "Pendiente",
                   validated = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved && ah.Validated),
                   available = availableCoursesHash.Contains(x.CourseId)
               })
               .ToList();

            var recordsTotal = courses.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = courses,
                DrawCounter = sentParameters.DrawCounter,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<AcademicYearCourse>> GetAcademicYearCoursesByCurriculumId(Guid curriculumId)
            => await _context.AcademicYearCourses.Where(x => x.CurriculumId == curriculumId).ToListAsync();

        public async Task<string> GetNextCorrelative(Guid curriculumId)
        {
            var coursesCodes = await _context.AcademicYearCourses.Where(x => x.CurriculumId == curriculumId).Select(x => x.Course.Code).OrderByDescending(x => x).ToListAsync();

            var lastCode = coursesCodes.FirstOrDefault();

            if (int.TryParse(lastCode, out int lastCodeInt))
            {
                return $"{(lastCodeInt + 1):000}";
            }

            return $"{(coursesCodes.Count() + 1):000}";
        }
    }
}