using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.GradeReportByCompetences;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.CORE.Services;
using System.IO;
using OpenXmlPowerTools;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using AKDEMIC.ENTITIES.Models.Laurassia;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class StudentSectionRepository : Repository<StudentSection>, IStudentSectionRepository
    {
        public StudentSectionRepository(AkdemicContext context) : base(context)
        {
        }

        #region PRIVATE
        private Expression<Func<CourseReportCardTemplate, dynamic>> GetCourseReportCardDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Curriculum);
                case "1":
                    return ((x) => x.Year);
                case "2":
                    return ((x) => x.Code);
                case "3":
                    return ((x) => x.Course);
                case "4":
                    return ((x) => x.Section);
                case "5":
                    return ((x) => x.Credits);
                case "6":
                    return ((x) => x.TheoreticalHours);
                case "7":
                    return ((x) => x.PracticalHours);
                case "8":
                    return ((x) => x.Grade);
                case "9":
                    return ((x) => x.Status);
                case "10":
                    return ((x) => x.Teacher);
                default:
                    return ((x) => x.Year);
            }
        }

        private Expression<Func<StudentSectionTemplate, dynamic>> GetStudentSectionDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.CourseName);
                case "1":
                    return ((x) => x.ClassCount);
                case "2":
                    return ((x) => x.MaxAbsences);
                case "3":
                    return ((x) => x.Dictated);
                case "4":
                    return ((x) => x.Assisted);
                case "5":
                    return ((x) => x.Absences);
                default:
                    return ((x) => x.CourseName);
            }
        }

        //private Func<CourseReportCardTemplate, string[]> GetCourseReportCardDatatableSearchValuePredicate()
        //{
        //    return (x) => new[]
        //    {
        //        x.Curriculum+"",
        //        x.Year+"",
        //        x.Code+"",
        //        x.Course+"",
        //        x.Section+"",
        //        x.Credits+"",
        //        x.TheoreticalHours+"",
        //        x.PracticalHours+"",
        //        x.Grade+"",
        //        x.Status+"",
        //        x.Teacher+""
        //    };
        //}

        //private Func<StudentSectionTemplate, string[]> GetStudentSectionDatatableSearchValuePredicate()
        //{
        //    return (x) => new[]
        //    {
        //        x.SectionId  +"",
        //        x.CourseName +"",
        //        x.ClassCount +"",
        //        x.MaxAbsences+"",
        //        x.Dictated+"",
        //        x.Assisted+"",
        //        x.Absences+"",
        //    };
        //}

        private async Task<DataTablesStructs.ReturnedData<CourseReportCardTemplate>> GetCourseReportCardDatatable(
          DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid termId, string search,
          Expression<Func<CourseReportCardTemplate, CourseReportCardTemplate>> selectPredicate = null,
          Expression<Func<CourseReportCardTemplate, dynamic>> orderByPredicate = null)
        {

            var query = _context.StudentSections
                .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termId && !x.Section.IsDirectedCourse).AsQueryable();

            var result = query
                .Select(x => new CourseReportCardTemplate
                {
                    Curriculum = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Code).FirstOrDefault(),
                    Year = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault(),
                    Code = x.Section.CourseTerm.Course.Code,
                    Course = x.Section.CourseTerm.Course.Name,
                    Section = x.Section.Code,
                    Credits = x.Section.CourseTerm.Course.Credits,
                    TheoreticalHours = x.Section.CourseTerm.Course.TheoreticalHours,
                    PracticalHours = x.Section.CourseTerm.Course.PracticalHours,
                    Grade = x.FinalGrade,
                    Status = x.Status,
                    Teacher = x.Section.TeacherSections.Select(t => t.Teacher.User.FullName).FirstOrDefault()
                })
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                result = result
                    .Where(x => x.Curriculum.ToUpper().Contains(search.ToUpper())
                    || x.Year.ToString().Contains(search.ToUpper())
                    || x.Code.ToUpper().Contains(search.ToUpper())
                    || x.Course.ToUpper().Contains(search.ToUpper())
                    || x.Section.ToUpper().Contains(search.ToUpper()));
            }

            return await result.ToDataTables(sentParameters, selectPredicate);
        }

        private async Task<DataTablesStructs.ReturnedData<StudentSectionTemplate>> GetStudentSectionDatatable(
          DataTablesStructs.SentParameters sentParameters, Guid sid, Guid pid, string search,
          Expression<Func<StudentSectionTemplate, StudentSectionTemplate>> selectPredicate = null,
          Expression<Func<StudentSectionTemplate, dynamic>> orderByPredicate = null)
        {
            var student = await _context.Students.Include(x => x.Career).Where(x => x.Id == sid).FirstOrDefaultAsync();
            var term = await _context.Terms.FindAsync(pid);
            if (term == null)
                term = new Term();
            var query = _context.StudentSections
                .Include(x => x.Section.Classes)
                .Include(x => x.Section.CourseTerm).ThenInclude(x => x.Course)
                .Where(x => x.StudentId.Equals(student.Id) && x.Section.CourseTerm.TermId.Equals(term.Id) && !x.Section.IsDirectedCourse).AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.FullName.ToLower().Contains(search.Trim().ToLower()) ||
                x.Section.CourseTerm.Course.Name.ToLower().Contains(search.Trim().ToLower()));

            int recordsFiltered = await query.CountAsync();

            var dataDB = await query
                .OrderBy(x => x.Section.CourseTerm.Course.Name)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new StudentSectionTemplate
                {
                    SectionId = x.SectionId,
                    CourseName = x.Section.CourseTerm.Course.FullName,
                    ClassCount = x.Section.Classes.Count(),
                    Dictated = x.Section.Classes.Count(c => c.IsDictated),
                    Assisted = x.Section.Classes.Count(c => c.ClassStudents.Any(cs => cs.StudentId.Equals(student.Id) && !cs.IsAbsent) && c.IsDictated),
                    Absences = x.Section.Classes.Count(c => c.ClassStudents.Any(cs => cs.StudentId.Equals(student.Id) && cs.IsAbsent) && c.IsDictated)
                })
                .ToListAsync();

            var result = dataDB
                .Select(x => new StudentSectionTemplate
                {
                    SectionId = x.SectionId,
                    CourseName = x.CourseName,
                    ClassCount = x.ClassCount,
                    MaxAbsences = x.ClassCount * (1 - (ConstantHelpers.COURSES.ATTENDANCE_MIN_PERCENTAGE / 100)),
                    Dictated = x.Dictated,
                    Assisted = x.Assisted,
                    Absences = x.Absences,
                })
                .ToList();

            int recordsTotal = result.Count;

            return new DataTablesStructs.ReturnedData<StudentSectionTemplate>
            {
                Data = result,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        private Expression<Func<ReportCourseDetailTemplate, dynamic>> GetReportCourseDetailDataTableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Fullname);
                case "1":
                    return ((x) => x.Absences);
                case "2":
                    return ((x) => x.Dictated);
                case "3":
                    return ((x) => x.MaxAbsences);
                default:
                    return ((x) => x.Fullname);
            }
        }
        private Func<ReportCourseDetailTemplate, string[]> GetReportCourseDetailDataTableValuePredicate()
        {
            return (x) => new[]
            {
                x.Fullname  +"",
                x.MaxAbsences+"",
                x.Dictated+"",
                x.Assisted+"",
                x.Absences+"",
            };
        }
        private async Task<DataTablesStructs.ReturnedData<ReportCourseDetailTemplate>> GetReportCourseDetailDataTable(
          DataTablesStructs.SentParameters sentParameters, Guid ctid,
          Expression<Func<ReportCourseDetailTemplate, ReportCourseDetailTemplate>> selectPredicate = null,
          Expression<Func<ReportCourseDetailTemplate, dynamic>> orderByPredicate = null)
        {
            var query = _context.StudentSections
                              .Include(x => x.Section.Classes)
                              .Where(x => x.Section.CourseTermId.Equals(ctid) && !x.Section.IsDirectedCourse).AsQueryable();

            var result = query
      .Select(x => new ReportCourseDetailTemplate
      {
          Fullname = x.Student.User.FullName,
          Absences = x.Section.Classes.Count(c => c.ClassStudents.Any(cs => cs.IsAbsent && c.StartTime.ToLocalTime() < DateTime.Now)),
          Assisted = x.Section.Classes.Count(c => c.ClassStudents.Any(cs => !cs.IsAbsent && c.StartTime.ToLocalTime() < DateTime.Now)),
          MaxAbsences = x.Section.Classes.Count() * (1 - (ConstantHelpers.COURSES.ATTENDANCE_MIN_PERCENTAGE / 100)),
          Dictated = x.Section.Classes.Count(c => c.StartTime.ToLocalTime() < DateTime.Now)
      })
      .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
      .AsNoTracking();

            return await result.ToDataTables(sentParameters, selectPredicate);
        }
        #endregion

        #region PUBLIC
        public override async Task<StudentSection> Get(Guid id)
        {
            return await _context.StudentSections
                .Include(x => x.Section.CourseTerm)
                .Include(x => x.Student)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<DataTablesStructs.ReturnedData<StudentSectionTemplate>> GetStudentSectionDatatable(DataTablesStructs.SentParameters parameters, Guid sid, Guid pid, string search)
        {
            return await GetStudentSectionDatatable(parameters, sid, pid, search, null, GetStudentSectionDatatableOrderByPredicate(parameters));
        }

        public async Task<DataTablesStructs.ReturnedData<CourseReportCardTemplate>> GetCourseReportCardDatatable(DataTablesStructs.SentParameters parameters, Guid studentId, Guid termId, string search)
        {
            return await GetCourseReportCardDatatable(parameters, studentId, termId, search, null, GetCourseReportCardDatatableOrderByPredicate(parameters));
        }

        public async Task<IEnumerable<StudentSection>> GetStudentSectionsByStudentId(Guid studentId)
        {
            return await _context.StudentSections.Where(x => x.StudentId == studentId && !x.Section.IsDirectedCourse).Include(x => x.Section).ThenInclude(x => x.CourseTerm).ThenInclude(x => x.Term).ToArrayAsync();
        }

        public async Task<IEnumerable<StudentSection>> GetStudentSectionsBySectionId(Guid sectionId)
            => await _context.StudentSections.Where(x => x.SectionId == sectionId).ToArrayAsync();

        public async Task<List<StudentSectionRegisterGradeTemplate>> GetStudentSectionsRegisterGradeTemplate(Guid sectionId, string teacherId = null)
        {

            var query = _context.StudentSections.Where(x => x.SectionId == sectionId).AsNoTracking();

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.Section.TeacherSections.Any(y => y.TeacherId == teacherId));


            var result = await query
                .Select(x => new StudentSectionRegisterGradeTemplate
                {
                    Id = x.Id,
                    SectionGroupId = x.SectionGroupId,
                    Status = x.Status,
                    StudentFullName = x.Student.User.FullName,
                    StudentId = x.StudentId,
                    StudentUserName = x.Student.User.UserName,
                    UserId = x.Student.UserId
                })
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<CourseTemplate>> GetGradesByStudentIdAndTermId(Guid studentId, Guid termId)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();

            var data = await _context.StudentSections
                .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == term.Id && !x.Section.IsDirectedCourse)
                .Select(x => new
                {
                    Try = x.Try,
                    Course = x.Section.CourseTerm.Course.FullName,
                    FinalGrade = x.FinalGrade,
                    Observations = x.Observations,
                    Status = x.Status,
                    Formula = "",
                    Section = x.Section.Code,
                    SectionId = x.SectionId,
                    Credits = x.Section.CourseTerm.Course.Credits,
                    PercentageProgress = x.Grades.Sum(g => g.Evaluation.Percentage),
                    Grades = x.Grades
                    .Select(g => new
                    {
                        Name = g.Evaluation.Name,
                        Percentage = g.Evaluation.Percentage,
                        Grade = g.Value,
                        Approved = g.Value >= term.MinGrade,
                        Attended = g.Attended,
                        Taked = true
                    }).ToList(),

                    Evaluations = x.Section.CourseTerm.Evaluations
                    .Where(e => !x.Grades.Any(gr => gr.EvaluationId == e.Id))
                    .Select(e => new
                    {
                        Approved = false,
                        Attended = false,
                        Grade = 0.00M,
                        Name = e.Name,
                        Percentage = e.Percentage
                    }).ToList()
                }).ToArrayAsync();

            var model = data
                .Select(x => new CourseTemplate
                {
                    Try = x.Try,
                    Course = x.Course,
                    FinalGrade = x.FinalGrade,
                    Observations = x.Observations,
                    Status = x.Status,
                    Formula = "",
                    Section = x.Section,
                    SectionId = x.SectionId,
                    Credits = x.Credits,
                    PercentageProgress = x.PercentageProgress,
                    Evaluations = x.Grades
                    .Select(g => new CourseEvaluationTemplate
                    {
                        Name = g.Name,
                        Percentage = g.Percentage,
                        Grade = g.Grade,
                        Approved = g.Approved,
                        Attended = g.Attended,
                        Taked = g.Taked
                    }).Concat(x.Evaluations
                    .Select(e => new CourseEvaluationTemplate
                    {
                        Approved = false,
                        Attended = false,
                        Grade = 0.00M,
                        Name = e.Name,
                        Percentage = e.Percentage
                    })).ToList()
                }).ToArray();

            //var model = await _context.StudentSections
            //    .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == term.Id && !x.Section.IsDirectedCourse)
            //    .Select(x => new CourseTemplate
            //    {
            //        Try = x.Try,
            //        Course = x.Section.CourseTerm.Course.FullName,
            //        FinalGrade = x.FinalGrade,
            //        Observations = x.Observations,
            //        Status = x.Status,
            //        Formula = "",
            //        Section = x.Section.Code,
            //        SectionId = x.SectionId,
            //        Credits = x.Section.CourseTerm.Course.Credits,
            //        PercentageProgress = x.Grades.Sum(g => g.Evaluation.Percentage),
            //        Evaluations = x.Grades
            //        .Select(g => new CourseEvaluationTemplate
            //        {
            //            Name = g.Evaluation.Name,
            //            Percentage = g.Evaluation.Percentage,
            //            Grade = g.Value,
            //            Approved = g.Value >= term.MinGrade,
            //            Attended = g.Attended,
            //            Taked = true
            //        }).Concat(x.Section.CourseTerm.Evaluations
            //        .Where(e => !x.Grades.Any(gr => gr.EvaluationId == e.Id))
            //        .Select(e => new CourseEvaluationTemplate
            //        {
            //            Approved = false,
            //            Attended = false,
            //            Grade = 0.00M,
            //            Name = e.Name,
            //            Percentage = e.Percentage
            //        })).ToList()
            //    }).ToArrayAsync();

            return model;
        }

        public async Task<bool> AnyByTermIdCourseIdStudentIdAndValidSubstituteExam(Guid termId, Guid courseId, Guid studentId)
        {
            var min_substitute_examen = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.MIN_SUBSTITUTE_EXAMEN));

            return await _context.StudentSections.AnyAsync(x => x.Section.CourseTerm.TermId == termId && x.Section.CourseTerm.CourseId == courseId && x.StudentId == studentId && x.FinalGrade >= min_substitute_examen);
        }
        public async Task<bool> AnyByTermIdCourseIdStudentIdAndValidSubstituteExamV2(Guid termId, Guid studentSectionId, Guid studentId)
        {
            var min_substitute_examen = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.MIN_SUBSTITUTE_EXAMEN));

            return await _context.StudentSections.AnyAsync(x => x.Section.CourseTerm.TermId == termId && x.Id == studentSectionId && x.StudentId == studentId && x.FinalGrade >= min_substitute_examen);
        }

        public async Task<List<StudentGradeTemplate>> GetStudentGradesTemplate(Guid studentSectionId)
        {
            var data = await _context.StudentSections.Where(x => x.Id == studentSectionId)
                .Select(x => new
                {
                    x.Id,
                    x.Student.User.FullName,
                    x.SectionId,
                    x.Section.CourseTermId,
                    x.Grades
                })
                .FirstOrDefaultAsync();

            var evaluations = await _context.Evaluations.Where(x => x.CourseTermId == data.CourseTermId)
                .OrderBy(x => x.CourseUnit.Number)
                .ThenBy(x => x.Week)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Percentage,
                    CourseUnit = x.CourseUnit.Name
                })
                .ToListAsync();

            var result =
                evaluations.Select(x => new StudentGradeTemplate
                {
                    Evaluation = $"{x.CourseUnit} - {x.Name}",
                    Percentage = x.Percentage,
                    Value = data.Grades.Any(y => y.EvaluationId == x.Id) ? data.Grades.Where(y => y.EvaluationId == x.Id).Select(y => y.Value).FirstOrDefault() : null
                })
                .ToList();

            return result;
        }

        public async Task<object> GetAllAsModelA(Guid? studentId = null, Guid? termId = null, bool acta = false)
        {
            var query = _context.StudentSections.Where(x => !x.Section.IsDirectedCourse).AsQueryable();

            if (studentId.HasValue)
                query = query.Where(x => x.StudentId == studentId);

            if (termId.HasValue)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId.Value);

            if (acta)
                query = query.Where(x => x.Section.EvaluationReports == null);

            var courses = await query
                .Select(
                    x => new
                    {
                        Id = x.Section.CourseTerm.CourseId,
                        Text = x.Section.CourseTerm.Course.Name
                    }
                ).ToListAsync();

            return courses;
        }

        public async Task<IEnumerable<StudentSection>> GetAll(Guid? studentId = null, Guid? termId = null, Guid? sectionId = null)
        {
            var query = _context.StudentSections
                .Include(x => x.Student.User)
                .Include(x => x.Section.CourseTerm.Course)
                .Include(x => x.Section.Classes)
                .ThenInclude(x => x.ClassStudents)
                .AsQueryable();

            if (studentId.HasValue)
                query = query.Where(x => x.StudentId == studentId.Value);

            if (termId.HasValue)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId.Value);

            if (sectionId.HasValue)
                query = query.Where(x => x.SectionId == sectionId.Value); ;

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<StudentSection>> GetStudentSectionsByTeacherId(Guid sectionId, string teacherId, Guid? studentId = null)
        {
            var query = _context.StudentSections
                .Where(x => x.SectionId == sectionId)
                .Include(x => x.Student)
                .ThenInclude(x => x.User)
                .Include(x => x.Section.CourseTerm.Course)
                .Include(x => x.Section.Classes)
                .ThenInclude(x => x.ClassStudents)
                .Include(x => x.Section.ClassSchedules)
                .AsNoTracking();

            if (studentId != null && studentId != Guid.Empty)
            {
                query = query.Where(x => x.StudentId == studentId);
            }

            var result = await query.ToListAsync();

            return result;
        }


        public async Task<IEnumerable<StudentSection>> GetAllWithGradesAndEvaluations(Guid? studentId = null, Guid? termId = null, Guid? sectionId = null, string teacherId = null)
        {
            var query = _context.StudentSections
                .AsNoTracking();

            if (studentId.HasValue)
                query = query.Where(x => x.StudentId == studentId.Value);

            if (termId.HasValue)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId.Value);

            if (sectionId.HasValue)
                query = query.Where(x => x.SectionId == sectionId.Value);

            //if (!string.IsNullOrEmpty(teacherId))
            //{
            //var teacherSection = await _context.TeacherSections.Where(x => x.TeacherId == teacherId && x.SectionId == sectionId).FirstOrDefaultAsync();
            //if (teacherSection.SectionGroupId.HasValue && !teacherSection.IsPrincipal)
            //    query = query.Where(x => x.SectionGroupId == teacherSection.SectionGroupId);
            //}

            var studentsections = await query
                .Include(x => x.Student.User)
                .Include(x => x.Section.CourseTerm.Course)
                .Include(x => x.Section.CourseTerm.Evaluations)
                .Include(x => x.Grades)
                .ThenInclude(x => x.Evaluation.CourseUnit)
                .ToListAsync();

            return studentsections;
        }

        public async Task<DataTablesStructs.ReturnedData<ReportCourseDetailTemplate>> GetReportCourseDetailDataTable(DataTablesStructs.SentParameters parameters, Guid ctid)
        {
            var query = _context.StudentSections
                            .Where(x => x.Section.CourseTermId.Equals(ctid)).AsQueryable();

            var sectioonsclasses = await _context.ClassStudents
              .Where(x => x.Class.Section.CourseTermId == ctid)
              .Include(x => x.Class)
              .ToListAsync();

            var totalClasesBySection = await _context.Sections.Where(x => x.CourseTermId == ctid)
                .Select(x => new
                {
                    totalClases = x.Classes.Count(),
                    sectionId = x.Id
                })
                .ToListAsync();

            var data = await query
                .Select(x => new
                {
                    x.StudentId,
                    x.SectionId,
                    Fullname = x.Student.User.FullName,
                })
                .ToListAsync();

            var result = data
                .Select(x => new ReportCourseDetailTemplate
                {
                    Fullname = x.Fullname,
                    Absences = sectioonsclasses.Where(y => y.StudentId == x.StudentId).Count(cs => cs.IsAbsent && cs.Class.StartTime.ToLocalTime() < DateTime.Now),
                    Assisted = sectioonsclasses.Where(y => y.StudentId == x.StudentId).Count(cs => !cs.IsAbsent && cs.Class.StartTime.ToLocalTime() < DateTime.Now),
                    MaxAbsences = (int)Math.Round((totalClasesBySection.Where(y => y.sectionId == x.SectionId).Select(y => y.totalClases).FirstOrDefault()) * (1 - ((decimal)ConstantHelpers.COURSES.ATTENDANCE_MIN_PERCENTAGE / 100M)), 0, MidpointRounding.AwayFromZero),
                    Dictated = sectioonsclasses.Where(y => y.StudentId == x.StudentId).Count(c => c.Class.StartTime.ToLocalTime() < DateTime.Now)
                })
                .ToList();

            return new DataTablesStructs.ReturnedData<ReportCourseDetailTemplate>
            {
                Data = result,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = result.Count,
                //RecordsFiltered = recordsFiltered,
                RecordsTotal = result.Count,
            };
        }
        public async Task<List<SelectListItem>> GetTermSelectListByStudent(Guid id)
        {
            var terms = await _context.StudentSections
                .Where(x => x.StudentId == id)
                .OrderByDescending(x => x.Section.CourseTerm.Term.Year).ThenByDescending(x => x.Section.CourseTerm.Term.Number)
                .Select(x => new
                {
                    Value = x.Section.CourseTerm.TermId.ToString(),
                    Text = x.Section.CourseTerm.Term.Name,
                }).ToListAsync();

            var result = terms.GroupBy(x => x.Value)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Select(y => y.Text).FirstOrDefault()
                })
                .ToList();

            return result;
        }
        public async Task<List<CourseTemplate>> GetGradesByStudentAnTerm(Guid studentId, Guid termid)
        {
            var term = await _context.Terms.FindAsync(termid);

            var model = await _context.StudentSections
                   .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termid)
                   .Select(x => new CourseTemplate
                   {
                       Try = x.Try,
                       Course = x.Section.CourseTerm.Course.FullName,
                       FinalGrade = x.FinalGrade,
                       Observations = x.Observations,
                       Status = x.Status,
                       Formula = "",
                       Section = x.Section.Code,
                       Credits = x.Section.CourseTerm.Course.Credits,
                       PercentageProgress = x.Grades.Sum(g => g.Evaluation.Percentage),
                       Evaluations = x.Grades
                       .Select(g => new CourseEvaluationTemplate
                       {
                           Name = g.Evaluation.Name,
                           Percentage = g.Evaluation.Percentage,
                           Grade = g.Value,
                           Approved = term == null ? false : g.Value >= term.MinGrade,
                           Attended = g.Attended,
                           Taked = true
                       }).ToList(),
                       //.Concat(x.Section.CourseTerm.Evaluations
                       //.Where(e => !x.Grades.Any(gr => gr.EvaluationId == e.Id))
                       //.Select(e => new CourseEvaluationTemplate
                       //{
                       //    Approved = false,
                       //    Attended = false,
                       //    Grade = 0.00M,
                       //    Name = e.Name,
                       //    Percentage = e.Percentage
                       //})).ToList()
                       Evaluations2 = x.Section.CourseTerm.Evaluations
                       .Where(e => !x.Grades.Any(gr => gr.EvaluationId == e.Id))
                       .Select(e => new CourseEvaluationTemplate
                       {
                           EvaluationId = e.Id,
                           Approved = false,
                           Attended = false,
                           Grade = 0.00M,
                           Name = e.Name,
                           Percentage = e.Percentage
                       }).ToList()
                   }).ToListAsync();

            foreach (var item in model)
            {
                item.Evaluations = item.Evaluations
                    .Concat(item.Evaluations2).ToList();
            }

            return model;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesAssistanceByStudentAndTermDatatable(DataTablesStructs.SentParameters parameters, Guid studentId, Guid termid)
        {
            Expression<Func<StudentSection, dynamic>> orderByPredicate = null;


            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Section.CourseTerm.Course.FullName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Section.Code;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Status;
                    break;
                default:
                    orderByPredicate = (x) => x.Section.CourseTerm.Course.FullName;
                    break;
            }


            var query = _context.Sections
                .Where(x => x.StudentSections.Any(y => y.StudentId == studentId)
                && x.CourseTerm.TermId == termid && !x.IsDirectedCourse)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var absencePercentage = await _context.Terms.Where(x => x.Id == termid).Select(x => x.AbsencePercentage).FirstOrDefaultAsync();

            var queryclient = await query
                .Select(x => new
                {
                    id = x.Id,
                    sectiongroupId = x.StudentSections.Where(y => y.StudentId == studentId).Select(y => y.SectionGroupId).FirstOrDefault(),
                    course = x.CourseTerm.Course.FullName,
                    total = x.Classes.Count(),
                    classes = x.Classes.Select(c => new
                    {
                        c.IsDictated,
                        sectionGroupId = c.ClassSchedule.SectionGroupId,
                        classstudents = c.ClassStudents.Select(cs => new
                        {
                            cs.Class.ClassSchedule.SectionGroupId,
                            cs.StudentId,
                            cs.IsAbsent
                        }),
                        starttime = c.StartTime,
                    }),
                })
                .ToListAsync();

            var dataDb = queryclient
                .Select(x => new
                {
                    id = x.id,
                    course = x.course,
                    total = x.classes.Where(y => (y.sectionGroupId == x.sectiongroupId || !y.sectionGroupId.HasValue)).Count(),
                    maxAbsences = (int)Math.Floor(x.classes.Where(y => y.sectionGroupId == x.sectiongroupId || !y.sectionGroupId.HasValue).Count() * (decimal)absencePercentage / 100M),
                    //maxAbsences = x.classes.Count() * (1 - (ConstantHelpers.COURSES.ATTENDANCE_MIN_PERCENTAGE / 100)),
                    dictated = x.classes.Where(y => (y.sectionGroupId == x.sectiongroupId || !y.sectionGroupId.HasValue) && y.IsDictated).Count(),
                    //dictated = x.classes.Count(c => c.starttime.ToLocalTime() < DateTime.Now && c.IsDictated),
                    //assisted = x.classes.Count(c => c.classstudents.Any(cs => cs.StudentId == studentId && !cs.IsAbsent) && c.starttime.ToLocalTime() < DateTime.Now && c.IsDictated),
                    assisted = x.classes.Where(y => (y.sectionGroupId == x.sectiongroupId || !y.sectionGroupId.HasValue) && y.IsDictated && y.classstudents.Any(z => z.StudentId == studentId && !z.IsAbsent)).Count(),
                    //absences = x.classes.Count(c => c.classstudents.Any(cs => cs.StudentId == studentId && cs.IsAbsent) && c.starttime.ToLocalTime() < DateTime.Now && c.IsDictated)
                    absences = x.classes.Where(y => (y.sectionGroupId == x.sectiongroupId || !y.sectionGroupId.HasValue) && y.IsDictated && y.classstudents.Any(z => z.StudentId == studentId && z.IsAbsent)).Count(),
                }).ToList();

            var data = dataDb
                .Select(x => new
                {
                    x.id,
                    x.course,
                    x.total,
                    x.maxAbsences,
                    x.dictated,
                    x.assisted,
                    x.absences,
                    absencePercentage = x.total == 0M ? 0 : Math.Round((x.absences * 100.0) / (x.total * 1.0), 2, MidpointRounding.AwayFromZero)
                }).ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<object> GetReportCardHeader(Guid studentId, Guid termid)
        {
            var student = await _context.Students.Where(x => x.Id == studentId)
                .Select(x => new
                {
                    x.CurrentAcademicYear,
                    x.User.FullName
                })
                .FirstOrDefaultAsync();

            var query = _context.StudentSections
              .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termid).AsTracking();

            var term = await _context.Terms.Where(x => x.Id == termid).FirstOrDefaultAsync();

            var cycle = 0;
            if (term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
            {
                cycle = student.CurrentAcademicYear;
            }
            else
            {
                cycle = await _context.AcademicSummaries.Where(x => x.StudentId == studentId && x.TermId == termid).OrderBy(x => x.StudentAcademicYear).Select(x => x.StudentAcademicYear).FirstOrDefaultAsync();
            }

            var data = await query.Select(x => new
            {
                credits = x.Section.CourseTerm.Course.Credits
            })
                .ToListAsync();

            return new
            {
                credits = data.Sum(x => x.credits).ToString("0.0"),
                courses = data.Count(),
                name = student.FullName,
                cycle
            };
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesReportCardDatatable(DataTablesStructs.SentParameters parameters, Guid studentId, Guid termid)
        {
            Expression<Func<StudentSection, dynamic>> orderByPredicate = null;


            switch (parameters.OrderColumn)
            {
                default:
                    orderByPredicate = (x) => x.Section.CourseTerm.Course.FullName;
                    break;
            }


            var query = _context.StudentSections
                .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termid).AsTracking();

            var recordsFiltered = await query.CountAsync();

            var queryclient = await query
                //.OrderBy(orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    curriculums = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Code),
                    years = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear),
                    code = x.Section.CourseTerm.Course.Code,
                    course = x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code,
                    credits = x.Section.CourseTerm.Course.Credits.ToString("0.0"),
                    theoreticalHours = x.Section.CourseTerm.Course.TheoreticalHours,
                    practicalHours = x.Section.CourseTerm.Course.PracticalHours,
                    grade = x.FinalGrade,
                    status = x.Status,
                    teachers = x.Section.TeacherSections.Select(t => t.Teacher.User.FullName)
                })
                .ToListAsync();

            var data = queryclient
                .Select(x => new
                {
                    x.Id,
                    curriculum = x.curriculums.FirstOrDefault(),
                    year = x.years.FirstOrDefault(),
                    x.code,
                    x.course,
                    x.section,
                    x.credits,
                    x.theoreticalHours,
                    x.practicalHours,
                    x.grade,
                    x.status,
                    teacher = x.teachers.FirstOrDefault()
                })
                .OrderBy(x => x.code)
                .ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<StudentSection> GetByStudentAndCourseTerm(Guid studentId, Guid courseTermId)
        {
            return await _context.StudentSections
                           .Where(x => x.StudentId == studentId && x.Section.CourseTermId == courseTermId)
                           .FirstOrDefaultAsync();
        }

        public async Task<List<Tuple<string, string>>> GetAllStudentsBySection(Guid sectionId)
        {
            var students = await _context.StudentSections
                .Where(x => x.SectionId == sectionId)
                .Select(x => new Tuple<string, string>(x.Student.User.UserName, x.Student.User.FullName))
                .ToListAsync();

            return students;
        }

        public async Task<List<StudentSection>> GetAllBySectionIdWithIncludes(Guid sectionId)
        {
            var query = _context.StudentSections
                          .Include(x => x.Student).ThenInclude(x => x.User)
                          .Include(x => x.Section)
                          .Where(x => x.SectionId == sectionId);

            return await query.ToListAsync();
        }

        public async Task<List<StudentSection>> GetAllByCourse(Guid courseId)
        {
            var query = _context.StudentSections
              .Include(x => x.Student)
                .ThenInclude(x => x.User)
              .Where(x => x.Section.CourseTerm.CourseId == courseId);

            return await query.ToListAsync();
        }

        public async Task<decimal> GetTotalCreditsByStudentAndTerm(Guid studentId, Guid? termId = null)
        {
            var query = _context.StudentSections
                .Where(x => x.StudentId == studentId)
                .AsNoTracking();

            if (termId.HasValue)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId.Value);

            return await query.SumAsync(x => x.Section.CourseTerm.Course.Credits);
        }

        public async Task<List<StudentSection>> GetAllSectionStudentsWithUserBySectionId(Guid sectionId)
        {
            var sectionStudents = await _context.StudentSections
                .OrderBy(x => x.Student.User.FullName)
                .Where(x => x.SectionId == sectionId)
                .Include(x => x.Section)
                .Include(x => x.Student.User)
                .AsNoTracking()
                .ToListAsync();

            return sectionStudents;
        }

        public async Task<object> GetStudentSectionDatatableClientSide(Guid studentId, Guid termId)
        {
            var query = _context.StudentSections
                .Where(ss => ss.StudentId == studentId && ss.Section.CourseTerm.TermId == termId/* && !ss.Section.IsDirectedCourse*/)
                .AsNoTracking();

            var dbData = await query
                .Select(ss => new
                {
                    id = ss.Id,
                    year = ss.Section.CourseTerm.Course.AcademicYearCourses.FirstOrDefault().AcademicYear,
                    isElective = ss.Section.CourseTerm.Course.AcademicYearCourses.FirstOrDefault().IsElective,
                    code = ss.Section.CourseTerm.Course.Code,
                    course = ss.Section.CourseTerm.Course.Name,
                    section = ss.Section.Code,
                    vacancies = ss.Section.Vacancies,
                    studentSections = ss.Section.StudentSections.Count,
                    credits = ss.Section.CourseTerm.Course.Credits,
                    status = ss.Status
                })
                .OrderBy(x => x.year)
                .ThenBy(x => x.code)
                .ToListAsync();

            var data = dbData
                .Select(x => new
                {
                    x.id,
                    x.year,
                    x.code,
                    course = x.status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN
                        ? x.isElective ? $"{x.course} (Elec.) - RETIRADO" : $"{x.course} (Oblig.) - RETIRADO"
                        : x.isElective ? $"{x.course} (Elec.)" : $"{x.course} (Oblig.)",
                    x.section,
                    x.isElective,
                    vacancies = x.vacancies - x.studentSections < 0 ? 0 : x.vacancies - x.studentSections,
                    x.credits
                })
                .OrderBy(x => x.year)
                .ThenBy(x => x.code)
                .ToList();
            return data;
        }

        public async Task<Tuple<bool, string>> ConfirmEnrollment(Guid studentId, Guid termId, string userId)
        {
            var studentInfo = await _context.Students
                .Where(x => x.Id == studentId)
                .Select(x => new
                {
                    x.CareerNumber,
                    x.CurrentAcademicYear,
                    x.User.Document,
                    x.UserId,
                    x.Status,
                    x.CurriculumId,
                    x.CareerId,
                    x.Career.FacultyId,
                    CurriculumCode = x.Curriculum.Code,
                    CareerCode = x.Career.Code,
                    x.AdmissionType.IsExoneratedEnrollment,
                    AcademicHistories = x.AcademicHistories
                        .Where(a => !a.Withdraw)
                        .Select(a => new
                        {
                            a.CourseId,
                            a.Approved,
                            TermNumber = a.Term.Number,
                            a.Term.IsSummer,
                            a.Type
                        })
                        .ToList()
                }).FirstOrDefaultAsync();

            var directedCourses = await _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == termId && x.Section.IsDirectedCourse
                && x.StudentId == studentId)
                .Select(x => new
                {
                    x.Id,
                    x.Section.CourseTerm.CourseId
                })
                .ToListAsync();

            var enrollmentPaymentMethod = byte.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.ENROLLMENT_PAYMENT_METHOD));
            if (enrollmentPaymentMethod == 1)
            {
                var onlyCurrentPayments = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.VALIDATE_ONLY_CURRENT_PAYMENTS_ENROLLMENT));
                var paymentsPaid = true;

                if (onlyCurrentPayments)
                    paymentsPaid = !await _context.Payments.AnyAsync(x => x.UserId == studentInfo.UserId
                                    && (x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)
                                    && x.Status == ConstantHelpers.PAYMENT.STATUS.PENDING
                                    && x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE);
                else
                    paymentsPaid = !await _context.Payments.AnyAsync(x => x.UserId == studentInfo.UserId
                    && (x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)
                    && x.Status == ConstantHelpers.PAYMENT.STATUS.PENDING);

                if (!paymentsPaid) return new Tuple<bool, string>(false, $"Debe realizar el pago previo de matrícula");
            }

            var academicHistoriesTries = new List<Guid>();
            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD)
                academicHistoriesTries = studentInfo.AcademicHistories.Where(x => x.TermNumber != "A" && !x.IsSummer).Select(x => x.CourseId).ToList();
            else if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSM)
                academicHistoriesTries = studentInfo.AcademicHistories
                    .Where(x => !x.IsSummer
                    && x.Type != ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.REEVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.DEFERRED)
                    .Select(x => x.CourseId).ToList();
            else if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNTUMBES)
                academicHistoriesTries = studentInfo.AcademicHistories
                    .Where(x => x.Type != ConstantHelpers.AcademicHistory.Types.REEVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.DEFERRED)
                    .Select(x => x.CourseId).ToList();
            else
                academicHistoriesTries = studentInfo.AcademicHistories
                    .Where(x => x.Type != ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.REEVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.DEFERRED)
                    .Select(x => x.CourseId).ToList();

            var turn = await _context.EnrollmentTurns.FirstOrDefaultAsync(x => x.StudentId == studentId && x.TermId == termId);
            var anyEnrolledSection = await _context.StudentSections.AnyAsync(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termId && !x.Section.IsDirectedCourse);
            if ((turn != null && turn.IsConfirmed) || anyEnrolledSection) return new Tuple<bool, string>(false, $"La matrícula del alumno ya fue confirmada");

            var tmpEnrollments = await _context.TmpEnrollments
                .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termId)
                .Include(x => x.Section.CourseTerm.Course)
                .ToListAsync();

            if (turn != null && tmpEnrollments.Sum(x => x.Section.CourseTerm.Course.Credits) > turn.CreditsLimit) return new Tuple<bool, string>(false, $"El nro. de créditos a matricular exceden el límite del alumno");

            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Id == termId);
            var currentCourses = new List<Guid>();
            if (term.Status == ConstantHelpers.TERM_STATES.INACTIVE)
            {
                currentCourses = await _context.StudentSections
                    .Where(x => x.Section.CourseTerm.TermId == term.Id)
                    .Select(x => x.Section.CourseTerm.CourseId)
                    .ToListAsync();
            }

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAJMA)
            {
                var academicYearCourses = await _context.AcademicYearCourses
                   .Where(x => x.CurriculumId == studentInfo.CurriculumId && x.AcademicYear <= studentInfo.CurrentAcademicYear
                   && x.Course.CourseTerms.Any(y => y.TermId == term.Id))
                   .Select(x => new
                   {
                       x.CourseId,
                       x.Course.Code,
                       x.Course.Name,
                       x.AcademicYear,
                       x.Course.Credits,
                       PreRequisites = x.PreRequisites.ToList()
                   })
                   .ToListAsync();

                academicYearCourses = academicYearCourses
                    .Where(x => !studentInfo.AcademicHistories.Any(y => y.CourseId == x.CourseId && y.Approved))
                    .ToList();

                var requiredCourses = academicYearCourses
                    .Where(ayc => ayc.PreRequisites.All(p => studentInfo.AcademicHistories.Any(h => h.CourseId == p.CourseId && h.Approved)))
                    .Select(a => new
                    {
                        Id = a.CourseId,
                        Code = a.Code,
                        Name = a.Name,
                        Credits = a.Credits,
                        AcademicYear = a.AcademicYear,
                        Time = studentInfo.AcademicHistories.Count(x => x.CourseId == a.CourseId) + 1
                    }).ToList();

                var tmpStudentSections = tmpEnrollments
                                  .Select(te => new
                                  {
                                      te.SectionId,
                                      te.Section.Code,
                                      te.StudentId,
                                      te.Section.CourseTerm.CourseId,
                                      CourseName = te.Section.CourseTerm.Course.Name,
                                      CourseCode = te.Section.CourseTerm.Course.Code,
                                      Time = studentInfo.AcademicHistories.Count(x => x.CourseId == te.Section.CourseTerm.CourseId) + 1,
                                      te.Section.CourseTerm.Course.Credits,
                                      te.IsParallelCourse,
                                      te.SectionGroupId
                                  }).ToList();

                var requiredDisapprovedCourses = requiredCourses.Where(x => x.Time > 1).ToList();
                if (!tmpStudentSections.All(x => x.Time > 1) && !requiredDisapprovedCourses.All(rc => tmpStudentSections.Any(tmp => tmp.CourseId == rc.Id)))
                    return new Tuple<bool, string>(false, $"Debe matricularse en los cursos desaprobados");
            }

            var newStudentSections = new List<StudentSection>();

            foreach (var item in tmpEnrollments)
            {
                var section = await _context.Sections.Where(s => s.Id == item.SectionId).Select(s => new { s.Vacancies, s.StudentSections.Count }).FirstAsync();
                var available = section.Vacancies - section.Count > 0;

                if (item.IsParallelCourse)
                {
                    var prerequisites = await _context.AcademicYearCoursePreRequisites
                        .Where(x => x.AcademicYearCourse.CourseId == item.Section.CourseTerm.CourseId && x.AcademicYearCourse.CurriculumId == studentInfo.CurriculumId)
                        .Select(x => new
                        {
                            x.CourseId,
                            x.Course.Code,
                            x.IsOptional
                        }).ToListAsync();

                    var pendingPrerequisites = prerequisites
                        .Where(x => !x.IsOptional
                        && !studentInfo.AcademicHistories.Any(y => y.CourseId == x.CourseId && y.Approved)
                        && !currentCourses.Any(y => y == x.CourseId)
                        && !tmpEnrollments.Any(y => y.Section.CourseTerm.CourseId == x.CourseId)
                        && !directedCourses.Any(y => y.CourseId == x.CourseId)
                        ).ToList();

                    var optionalPendingPrerequisites = prerequisites.Where(x => x.IsOptional).Any() ? !prerequisites
                        .Where(x => x.IsOptional)
                        .Any(x =>
                        studentInfo.AcademicHistories.Any(y => y.CourseId == x.CourseId && y.Approved)
                        || currentCourses.Any(y => y == x.CourseId)
                        || tmpEnrollments.Any(y => y.Section.CourseTerm.CourseId == x.CourseId)
                        || directedCourses.Any(y => y.CourseId == x.CourseId)
                        ) : false;

                    if (studentInfo.CurriculumCode == "05" && studentInfo.CareerCode == "27" && item.Section.CourseTerm.Course.Code == "037")
                    {
                        var pendingRequiredCourses = pendingPrerequisites.Where(x => x.Code != "028" && x.Code != "029").ToList();
                        var electiveCourses = prerequisites.Where(x => x.Code == "028" || x.Code == "029").ToList();

                        if (pendingRequiredCourses.Count > 0 && !pendingPrerequisites.All(x => tmpEnrollments.Any(y => y.Section.CourseTerm.CourseId == x.CourseId)))
                            return new Tuple<bool, string>(false, $"Debe matricularse en los cursos requeridos para llevar en paralelo el curso {item.Section.CourseTerm.Course.Code}");
                        if (electiveCourses.Count > 0 && !electiveCourses.Any(x => studentInfo.AcademicHistories.Any(y => y.Approved && x.CourseId == y.CourseId) || currentCourses.Any(y => y == x.CourseId))
                            && !electiveCourses.Any(x => tmpEnrollments.Any(y => y.Section.CourseTerm.CourseId == x.CourseId)))
                            return new Tuple<bool, string>(false, $"Debe matricularse en los cursos requeridos para llevar en paralelo el curso {item.Section.CourseTerm.Course.Code}");
                    }
                    else if (studentInfo.CurriculumCode == "06" && studentInfo.CareerCode == "22" && item.Section.CourseTerm.Course.Code == "054")
                    {
                        var pendingRequiredCourses = pendingPrerequisites.Where(x => x.Code != "046" && x.Code != "047").ToList();
                        var electiveCourses = prerequisites.Where(x => x.Code == "046" || x.Code == "047").ToList();

                        if (pendingRequiredCourses.Count > 0 && !pendingPrerequisites.All(x => tmpEnrollments.Any(y => y.Section.CourseTerm.CourseId == x.CourseId)))
                            return new Tuple<bool, string>(false, $"Debe matricularse en los cursos requeridos para llevar en paralelo el curso {item.Section.CourseTerm.Course.Code}");
                        if (electiveCourses.Count > 0 && !electiveCourses.Any(x => studentInfo.AcademicHistories.Any(y => y.Approved && x.CourseId == y.CourseId) || currentCourses.Any(y => y == x.CourseId))
                            && !electiveCourses.Any(x => tmpEnrollments.Any(y => y.Section.CourseTerm.CourseId == x.CourseId)))
                            return new Tuple<bool, string>(false, $"Debe matricularse en los cursos requeridos para llevar en paralelo el curso {item.Section.CourseTerm.Course.Code}");
                    }
                    else if (studentInfo.CurriculumCode == "05" && studentInfo.CareerCode == "03"
                        && (item.Section.CourseTerm.Course.Code == "078" || item.Section.CourseTerm.Course.Code == "079"
                    || item.Section.CourseTerm.Course.Code == "080" || item.Section.CourseTerm.Course.Code == "081"))
                    {
                        var pendingRequiredCourses = pendingPrerequisites.Where(x => x.Code != "066" && x.Code != "067" && x.Code != "065" && x.Code != "064").ToList();
                        var electiveCourses = prerequisites.Where(x => x.Code == "066" || x.Code == "067" || x.Code == "065" || x.Code == "064").ToList();

                        if (pendingRequiredCourses.Count > 0 && !pendingPrerequisites.All(x => tmpEnrollments.Any(y => y.Section.CourseTerm.CourseId == x.CourseId)))
                            return new Tuple<bool, string>(false, $"Debe matricularse en los cursos requeridos para llevar en paralelo el curso {item.Section.CourseTerm.Course.Code}");
                        if (electiveCourses.Count > 0 && !electiveCourses.Any(x => studentInfo.AcademicHistories.Any(y => y.Approved && x.CourseId == y.CourseId) || currentCourses.Any(y => y == x.CourseId))
                            && !electiveCourses.Any(x => tmpEnrollments.Any(y => y.Section.CourseTerm.CourseId == x.CourseId)))
                            return new Tuple<bool, string>(false, $"Debe matricularse en los cursos requeridos para llevar en paralelo el curso {item.Section.CourseTerm.Course.Code}");
                    }
                    else if (studentInfo.CurriculumCode == "06" && studentInfo.CareerCode == "18" && item.Section.CourseTerm.Course.Code == "076")
                    {
                        var pendingRequiredCourses = pendingPrerequisites.Where(x => x.Code != "061" && x.Code != "062" && x.Code != "063" && x.Code != "078").ToList();
                        var electiveCourses = prerequisites.Where(x => x.Code == "061" || x.Code == "062" || x.Code == "063" || x.Code == "078").ToList();

                        if (pendingRequiredCourses.Count > 0 && !pendingPrerequisites.All(x => tmpEnrollments.Any(y => y.Section.CourseTerm.CourseId == x.CourseId)))
                            return new Tuple<bool, string>(false, $"Debe matricularse en los cursos requeridos para llevar en paralelo el curso {item.Section.CourseTerm.Course.Code}");
                        if (electiveCourses.Count > 0 && !electiveCourses.Any(x => studentInfo.AcademicHistories.Any(y => y.Approved && x.CourseId == y.CourseId) || currentCourses.Any(y => y == x.CourseId))
                            && !electiveCourses.Any(x => tmpEnrollments.Any(y => y.Section.CourseTerm.CourseId == x.CourseId)))
                            return new Tuple<bool, string>(false, $"Debe matricularse en los cursos requeridos para llevar en paralelo el curso {item.Section.CourseTerm.Course.Code}");
                    }
                    else if (studentInfo.CurriculumCode == "05" && studentInfo.CareerCode == "09" && item.Section.CourseTerm.Course.Code == "040")
                    {
                        var pendingRequiredCourses = pendingPrerequisites.Where(x => x.Code != "034" && x.Code != "033").ToList();
                        var electiveCourses = prerequisites.Where(x => x.Code == "034" || x.Code == "033").ToList();

                        if (pendingRequiredCourses.Count > 0 && !pendingPrerequisites.All(x => tmpEnrollments.Any(y => y.Section.CourseTerm.CourseId == x.CourseId)))
                            return new Tuple<bool, string>(false, $"Debe matricularse en los cursos requeridos para llevar en paralelo el curso {item.Section.CourseTerm.Course.Code}");
                        if (electiveCourses.Count > 0 && !electiveCourses.Any(x => studentInfo.AcademicHistories.Any(y => y.Approved && x.CourseId == y.CourseId) || currentCourses.Any(y => y == x.CourseId))
                            && !electiveCourses.Any(x => tmpEnrollments.Any(y => y.Section.CourseTerm.CourseId == x.CourseId)))
                            return new Tuple<bool, string>(false, $"Debe matricularse en los cursos requeridos para llevar en paralelo el curso {item.Section.CourseTerm.Course.Code}");
                    }
                    else if (pendingPrerequisites.Count > 0 || optionalPendingPrerequisites)
                        return new Tuple<bool, string>(false, $"Debe matricularse en los cursos requeridos para llevar en paralelo el curso {item.Section.CourseTerm.Course.Code}");
                }

                if (!available) return new Tuple<bool, string>(false, $"No hay vacantes dispónibles en el curso {item.Section.CourseTerm.Course.Code} sección {item.Section.Code}");

                newStudentSections.Add(new StudentSection
                {
                    SectionId = item.SectionId,
                    StudentId = item.StudentId,
                    Try = academicHistoriesTries.Count(x => x == item.Section.CourseTerm.CourseId) + 1,
                    SectionGroupId = item.SectionGroupId
                });

                item.WasApplied = true;
            }

            await _context.StudentSections.AddRangeAsync(newStudentSections);

            var adminEnrollment = new AdminEnrollment
            {
                StudentId = studentId,
                TermId = termId,
                UserId = userId,
                WasApplied = true
            };

            if (turn != null)
            {
                turn.IsConfirmed = true;
                turn.ConfirmationDate = DateTime.UtcNow;
            }
            else
            {
                var credits = await CalculateStudentCredits(studentId);
                turn = new EnrollmentTurn
                {
                    TermId = term.Id,
                    StudentId = studentId,
                    ConfirmationDate = DateTime.UtcNow,
                    IsConfirmed = true,
                    Time = DateTime.UtcNow,
                    CreditsLimit = credits,
                    IsOnline = false
                };
            }

            await _context.AdminEnrollments.AddAsync(adminEnrollment);

            var statusHash = new HashSet<int>()
                {
					//CORE.Helpers.ConstantHelpers.Student.States.ENTRANT,
					//CORE.Helpers.ConstantHelpers.Student.States.REGULAR,
					//CORE.Helpers.ConstantHelpers.Student.States.IRREGULAR,
					//CORE.Helpers.ConstantHelpers.Student.States.REPEATER,
					//CORE.Helpers.ConstantHelpers.Student.States.TRANSFER,
					//CORE.Helpers.ConstantHelpers.Student.States.UNBEATEN,
					//CORE.Helpers.ConstantHelpers.Student.States.NOENROLLMENT,
					//CORE.Helpers.ConstantHelpers.Student.States.OBSERVED,
					ConstantHelpers.Student.States.RESERVED,
                    ConstantHelpers.Student.States.SANCTIONED,
                    ConstantHelpers.Student.States.DESERTION,
                    ConstantHelpers.Student.States.NONPRESENTED,
                    ConstantHelpers.Student.States.NOENROLLMENT,
                };

            var student = await _context.Students.FindAsync(studentId);

            if (statusHash.Contains(studentInfo.Status))
                student.Status = ConstantHelpers.Student.States.IRREGULAR;

            if (!student.FirstEnrollmentTermId.HasValue)
            {
                student.FirstEnrollmentTermId = term.Id;
                student.FirstEnrollmentDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            #region Carga Procedimientos
            if (enrollmentPaymentMethod == 2)
            {
                await CreateStudentEnrollmentPayments(studentId);
            }
            #endregion

            //if moodle enabled
            foreach (var item in newStudentSections)
            {
                var result = await MoodleCreateEnrollment(item.Id);
                if (!result.Item1)
                    return result;
            }

            return new Tuple<bool, string>(true, "");
        }

        public async Task<Tuple<bool, string>> ConfirmEnrollmentRectification(Guid studentId, Guid termId, string userId, string fileName = null, string fileUrl = null, string observations = null)
        {
            var studentInfo = await _context.Students
                .Where(x => x.Id == studentId)
                .Select(x => new
                {
                    x.User.Document,
                    x.CareerId,
                    x.Career.FacultyId,
                    x.CareerNumber,
                    x.AdmissionTypeId,
                    x.AdmissionType.IsExoneratedEnrollment,
                    AdmissionTypeAbbrev = x.AdmissionType.Abbreviation,
                    x.UserId,
                    x.Status,
                    AcademicHistories = x.AcademicHistories
                    .Where(a => !a.Withdraw)
                    .Select(a => new
                    {
                        a.CourseId,
                        a.Approved,
                        TermNumber = a.Term.Number,
                        a.Term.IsSummer,
                        a.Type
                    }).ToList(),

                    TmpStudentSections = x.TmpEnrollments
                    .Where(t => t.Section.CourseTerm.TermId == termId)
                    .Select(te => new
                    {
                        te.Id,
                        te.SectionId,
                        te.Section.Code,
                        te.StudentId,
                        te.Section.CourseTerm.CourseId,
                        te.Section.CourseTerm.Course.Credits,
                        te.IsParallelCourse,
                        CourseCode = te.Section.CourseTerm.Course.Code,
                        te.SectionGroupId
                    })
                    .ToList(),

                    StudentSections = x.StudentSections
                    .Where(t => t.Section.CourseTerm.TermId == termId && !t.Section.IsDirectedCourse)
                    .ToList(),

                    x.CurriculumId,
                    CurriculumCode = x.Curriculum.Code,
                    x.CurrentAcademicYear,
                    CareerCode = x.Career.Code
                }).FirstOrDefaultAsync();

            var directedCourses = await _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == termId && x.Section.IsDirectedCourse
                && x.StudentId == studentId)
                .Select(x => new
                {
                    x.Id,
                    x.Section.CourseTerm.CourseId
                })
                .ToListAsync();

            var academicHistoriesTries = new List<Guid>();

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD)
                academicHistoriesTries = studentInfo.AcademicHistories.Where(x => x.TermNumber != "A" && !x.IsSummer).Select(x => x.CourseId).ToList();
            else if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSM)
                academicHistoriesTries = studentInfo.AcademicHistories
                    .Where(x => !x.IsSummer
                    && x.Type != ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.REEVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.DEFERRED)
                    .Select(x => x.CourseId).ToList();
            else if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNTUMBES)
                academicHistoriesTries = studentInfo.AcademicHistories
                    .Where(x => x.Type != ConstantHelpers.AcademicHistory.Types.REEVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.DEFERRED)
                    .Select(x => x.CourseId).ToList();
            else
                academicHistoriesTries = studentInfo.AcademicHistories
                    .Where(x => x.Type != ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.REEVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.DEFERRED)
                    .Select(x => x.CourseId).ToList();

            var enrollmentTurn = await _context.EnrollmentTurns.Where(x => x.StudentId == studentId && x.TermId == termId).FirstOrDefaultAsync();
            var totalCredits = studentInfo.TmpStudentSections.Sum(x => x.Credits);

            if (enrollmentTurn != null)
            {
                if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAMAD)
                {
                    if (totalCredits > enrollmentTurn.CreditsLimit)
                    {
                        return new Tuple<bool, string>(false, "Excede el máximo de créditos habilitados para el alumno");
                    }
                }
            }
            else
            {
                var enrollmentTurnRepository = new EnrollmentTurnRepository(_context);
                var creditsLimit = await enrollmentTurnRepository.GetStudentCreditsWithoutTurn(studentId);

                //var creditsModality = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.TERM_CREDITS_MODALITY);
                //var regularCredits = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.REGULAR_MAXIMUM_CREDITS));
                //var unbeatenStudentCredits = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.UNBEATEN_STUDENT_CREDITS));

                //var yearCredits = await _context.AcademicYearCourses
                //    .Where(x => x.CurriculumId == studentInfo.CurriculumId && x.AcademicYear == studentInfo.CurrentAcademicYear)
                //    .SumAsync(x => x.Course.Credits);

                //var creditsLimit = creditsModality == ConstantHelpers.Term.CreditsModality.MAXIMUM_CREDITS_BASED_ON_CURRICULUM ? yearCredits : regularCredits;

                //if (creditsModality == ConstantHelpers.Term.CreditsModality.ACADEMIC_YEAR_CREDITS_CONFIGURATION)
                //{
                //    var academicYearCredit = await _context.AcademicYearCredits
                //                      .Where(x => x.CurriculumId == studentInfo.CurriculumId && x.AcademicYear == studentInfo.CurrentAcademicYear)
                //                      .FirstOrDefaultAsync();
                //    creditsLimit = academicYearCredit.Credits;
                //}

                //if (studentInfo.Status == ConstantHelpers.Student.States.UNBEATEN)
                //    creditsLimit = creditsModality == ConstantHelpers.Term.CreditsModality.REGULAR_MAXIMUM_CREDITS ? unbeatenStudentCredits : creditsLimit + unbeatenStudentCredits;


                if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAMAD)
                {
                    if (totalCredits > creditsLimit)
                    {
                        return new Tuple<bool, string>(false, "Excede el máximo de créditos habilitados para el alumno");
                    }
                }
            }

            var toAdd = studentInfo.TmpStudentSections.Where(ts => studentInfo.StudentSections.All(ss => ss.SectionId != ts.SectionId || (ss.SectionId == ts.SectionId && ss.SectionGroupId != ts.SectionGroupId))).ToList();
            var toRemove = studentInfo.StudentSections.Where(ss => studentInfo.TmpStudentSections.All(ts => ts.SectionId != ss.SectionId || (ts.SectionId == ss.SectionId && ts.SectionGroupId != ss.SectionGroupId))).ToList();

            foreach (var item in toRemove)
            {
                if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAMAD)
                {
                    //var hasGrades = await _context.Grades
                    //    .Where(x => x.StudentSection.StudentId == studentId && x.StudentSection.Section.CourseTerm.TermId == termId)
                    //    .AnyAsync();
                    var hasGrades = await _context.Grades.Where(x => x.StudentSectionId == item.Id).AnyAsync() || await _context.TemporalGrades.Where(x => x.StudentSectionId == item.Id).AnyAsync();
                    if (hasGrades) return new Tuple<bool, string>(false, "Ya posee notas registradas en el periodo académico");

                    var hasQualifications = await _context.OtherQualificationStudents.Where(x => x.StudentSectionId == item.Id).AnyAsync();
                    if (hasQualifications) return new Tuple<bool, string>(false, "Ya posee calificaciones registradas en Aula Virtual");
                }
                else
                {
                    var grades = await _context.Grades.Where(x => x.StudentSectionId == item.Id).ToListAsync();
                    if (grades.Count > 0) _context.Grades.RemoveRange(grades);
                }
            }

            _context.StudentSections.RemoveRange(toRemove);

            var newStudentSections = new List<StudentSection>();

            foreach (var item in toAdd)
            {
                if (item.IsParallelCourse)
                {
                    var prerequisites = await _context.AcademicYearCoursePreRequisites
                        .Where(x => x.AcademicYearCourse.CourseId == item.CourseId && x.AcademicYearCourse.CurriculumId == studentInfo.CurriculumId)
                        .Select(x => new
                        {
                            x.CourseId,
                            x.Course.Code,
                            x.IsOptional
                        }).ToListAsync();

                    //var pendingPrerequisites = prerequisites.Where(x => !studentInfo.AcademicHistories.Any(y => y.CourseId == x.CourseId && y.Approved)).ToList();

                    var pendingPrerequisites = prerequisites
                      .Where(x => !x.IsOptional
                      && !studentInfo.AcademicHistories.Any(y => y.CourseId == x.CourseId && y.Approved)
                      && !studentInfo.TmpStudentSections.Any(y => y.CourseId == x.CourseId)
                      && !directedCourses.Any(y => y.CourseId == x.CourseId)
                      ).ToList();

                    var optionalPendingPrerequisites = prerequisites.Where(x => x.IsOptional).Any() ? !prerequisites
                        .Where(x => x.IsOptional)
                        .Any(x =>
                        studentInfo.AcademicHistories.Any(y => y.CourseId == x.CourseId && y.Approved)
                        || studentInfo.TmpStudentSections.Any(y => y.CourseId == x.CourseId)
                        || directedCourses.Any(y => y.CourseId == x.CourseId)
                        ) : false;

                    if (studentInfo.CurriculumCode == "05" && studentInfo.CareerCode == "27" && item.CourseCode == "037")
                    {
                        var pendingRequiredCourses = pendingPrerequisites.Where(x => x.Code != "028" && x.Code != "029").ToList();
                        var electiveCourses = prerequisites.Where(x => x.Code == "028" || x.Code == "029").ToList();

                        if (pendingRequiredCourses.Count > 0 && !pendingPrerequisites.All(x => studentInfo.TmpStudentSections.Any(y => y.CourseId == x.CourseId)))
                            return new Tuple<bool, string>(false, $"Debe matricularse en los cursos requeridos para llevar en paralelo el curso {item.CourseCode}");
                        if (electiveCourses.Count > 0 && !electiveCourses.Any(x => studentInfo.AcademicHistories.Any(y => y.Approved && x.CourseId == y.CourseId))
                            && !electiveCourses.Any(x => studentInfo.TmpStudentSections.Any(y => y.CourseId == x.CourseId)))
                            return new Tuple<bool, string>(false, $"Debe matricularse en los cursos requeridos para llevar en paralelo el curso {item.CourseCode}");
                    }
                    else if (pendingPrerequisites.Count > 0 || optionalPendingPrerequisites)
                        return new Tuple<bool, string>(false, $"Debe matricularse en los cursos requeridos para llevar en paralelo el curso {item.CourseCode}");
                }

                var available = await _context.Sections.Where(s => s.Id == item.SectionId).Select(s => (s.Vacancies - s.StudentSections.Count) > 0).FirstAsync();
                if (!available) return new Tuple<bool, string>(false, $"No hay vacantes dispónibles en la sección {item.Code}");

                newStudentSections.Add(new StudentSection
                {
                    SectionId = item.SectionId,
                    StudentId = item.StudentId,
                    Try = academicHistoriesTries.Count(x => x == item.CourseId) + 1,
                    SectionGroupId = item.SectionGroupId
                });
            }

            await _context.StudentSections.AddRangeAsync(newStudentSections);

            if (enrollmentTurn != null)
            {
                if (enrollmentTurn.IsRectificationActive) enrollmentTurn.IsRectificationActive = false;

                if (studentInfo.TmpStudentSections.Count == 0)
                {
                    enrollmentTurn.IsConfirmed = false;
                    enrollmentTurn.ConfirmationDate = null;
                }
            }

            if (studentInfo.TmpStudentSections.Count == 0)
            {
                var lastSummary = await _context.AcademicSummaries
                    .Where(x => x.StudentId == studentId)
                    .OrderByDescending(x => x.Term.Year)
                    .ThenByDescending(x => x.Term.Number)
                    .FirstOrDefaultAsync();

                if (lastSummary != null)
                {
                    var student = await _context.Students.FindAsync(studentId);
                    student.FirstEnrollmentTermId = lastSummary.TermId;
                    student.FirstEnrollmentDate = null;
                }
            }

            var adminEnrollment = await _context.AdminEnrollments
            .Where(x => x.IsRectification && !x.WasApplied && x.StudentId == studentId && x.TermId == termId && x.UserId == userId)
            .FirstOrDefaultAsync();

            if (adminEnrollment == null)
            {
                adminEnrollment = new AdminEnrollment
                {
                    StudentId = studentId,
                    TermId = termId,
                    UserId = userId,
                    Observations = observations,
                    IsRectification = true,
                    WasApplied = true
                };

                if (!string.IsNullOrEmpty(fileName))
                {
                    adminEnrollment.Filename = fileName;
                    adminEnrollment.FileUrl = fileUrl;
                }

                await _context.AdminEnrollments.AddAsync(adminEnrollment);
            }
            else
            {
                adminEnrollment.Observations = observations;
                adminEnrollment.WasApplied = true;

                if (!string.IsNullOrEmpty(fileName))
                {
                    adminEnrollment.Filename = fileName;
                    adminEnrollment.FileUrl = fileUrl;
                }
            }

            var tmpEnrollments = await _context.TmpEnrollments.IgnoreQueryFilters()
                .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termId
                && x.IsAdminRectification).ToListAsync();

            foreach (var item in tmpEnrollments) item.WasApplied = true;

            await _context.SaveChangesAsync();

            #region Carga Procedimientos
            var enrollmentPaymentMethod = byte.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.ENROLLMENT_PAYMENT_METHOD));
            if (enrollmentPaymentMethod == 2)
            {
                await CreateStudentEnrollmentPayments(studentId, true);
            }
            #endregion

            foreach (var item in newStudentSections)
            {
                var result = await MoodleCreateEnrollment(item.Id);
                if (!result.Item1)
                    return result;
            }

            return new Tuple<bool, string>(true, ""); ;
        }

        public async Task<object> GetCoursesSelect2ClientSide(Guid studentId, Guid termId, bool filterWithdraw = false)
        {
            var qry = _context.StudentSections
                .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termId && !x.Section.IsDirectedCourse)
                .AsNoTracking();

            if (filterWithdraw) qry = qry.Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN);

            var result = await qry
                 .Select(x => new
                 {
                     id = x.Id,
                     text = x.Section.CourseTerm.Course.FullName
                 })
                 .ToListAsync();

            result = result
                .OrderBy(x => x.text)
                .ToList();

            return result;
        }

        public async Task CourseWithdrawal(Guid studentId, Guid termId, Guid courseId)
        {
            var studentSection = await _context.StudentSections
                .Where(x => x.StudentId == studentId &&
                x.Section.CourseTerm.TermId == termId &&
                x.Section.CourseTerm.CourseId == courseId && !x.Section.IsDirectedCourse)
                .FirstOrDefaultAsync();

            studentSection.Status = ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN;


        }

        public async Task<bool> IsEnabledForWithdrawal(Guid studentSectionId)
        {
            var data = await _context.StudentSections.Where(x => x.Id == studentSectionId)
                .Include(x => x.Section.CourseTerm).FirstOrDefaultAsync();

            var histories = await _context.AcademicHistories
                .Where(x => x.StudentId == data.StudentId
                && x.CourseId == data.Section.CourseTerm.CourseId
                && x.Withdraw)
                .CountAsync();

            return histories < 2;
        }
        public async Task<object> GetStudnetSectionDetail(Guid id, Guid? termId = null)
        {
            var result = await _context.StudentSections
                .Where(ss => ss.StudentId == id && ss.Section.CourseTerm.TermId == termId && !ss.Section.IsDirectedCourse)
                .Select(ss => new
                {
                    code = ss.Section.CourseTerm.Course.Code,
                    section = ss.Section.Code,
                    course = ss.Section.CourseTerm.Course.FullName,
                    students = ss.Section.StudentSections.Count,
                    vacancies = ss.Section.Vacancies
                })
                .ToListAsync();

            return result;
        }

        public async Task<decimal> GetUsedCredits(Guid studentId, int status, Guid courseTermId)
        {
            var usedCredits = await _context.StudentSections
            .Where(ss => ss.StudentId == studentId &&
                  ss.Section.CourseTerm.Term.Status == status &&
                  ss.Section.CourseTermId != courseTermId && !ss.Section.IsDirectedCourse)
            .SumAsync(tmp => tmp.Section.CourseTerm.Course.Credits);

            return usedCredits;
        }

        public async Task<bool> GetIntersection(Guid studentId, int status, Guid courseTermId, ICollection<ClassSchedule> classSchedules)
        {
            var intersection = await _context.StudentSections
                .Where(ss => ss.StudentId == studentId && ss.Section.CourseTerm.Term.Status == status && ss.Section.CourseTermId != courseTermId && !ss.Section.IsDirectedCourse)
                .AnyAsync(ss => ss.Section.ClassSchedules
                    .Any(cs => classSchedules
                        .Any(scs => scs.WeekDay == cs.WeekDay && ((scs.StartTime <= cs.StartTime && cs.StartTime < scs.EndTime) || (scs.StartTime < cs.EndTime && cs.EndTime <= scs.EndTime) || (cs.StartTime <= scs.EndTime && scs.EndTime < cs.EndTime) || (cs.StartTime < scs.EndTime && scs.EndTime <= cs.EndTime)))
                    )
                );

            return intersection;
        }

        public async Task<List<StudentSection>> GetActiveSections(Guid courseId, Guid studentId)
        {
            var activeSections = await _context.StudentSections.Where(ss => ss.Section.CourseTermId == courseId && ss.StudentId == studentId && !ss.Section.IsDirectedCourse).ToListAsync();

            return activeSections;
        }

        public async Task<IEnumerable<StudentSection>> GetAllWithData()
        {
            var result = await _context.StudentSections
                .Include(x => x.Student).ThenInclude(x => x.User)
                .Include(x => x.Section).ThenInclude(x => x.CourseTerm).ThenInclude(x => x.Course)
                .Include(x => x.Section).ThenInclude(x => x.CourseTerm).ThenInclude(x => x.Term).ToListAsync();

            return result;

        }

        public async Task<List<StudentSection>> GetAllWithDataByStudentIdAndTermId(Guid termId, Guid studentId, bool? isDirectedCourse = false)
        {
            var model = await _context.StudentSections
                .Include(x => x.Section).ThenInclude(x => x.CourseTerm).ThenInclude(x => x.Course).ThenInclude(x => x.AcademicYearCourses)
                .Include(x => x.Section).ThenInclude(x => x.CourseTerm).ThenInclude(x => x.Term)
                .Include(x => x.Section).ThenInclude(x => x.CourseTerm).ThenInclude(x => x.Course)
                .Include(x => x.SectionGroup)
                .Where(ss => ss.Section.CourseTerm.TermId == termId && ss.StudentId == studentId && ss.Section.IsDirectedCourse == isDirectedCourse)
                .ToListAsync();

            return model;
        }

        public async Task<List<StudentSection>> GetAllWithDataByCareerAndStatus(Guid careerId, int status)
        {
            var result = await _context.StudentSections
              .Where(x => x.Student.CareerId == careerId && x.Section.CourseTerm.Term.Status == status)
                .Include(x => x.Student)
                .ThenInclude(x => x.User)
                .Include(x => x.Section)
                .ThenInclude(x => x.CourseTerm)
                .ThenInclude(x => x.Course)
                .Include(x => x.Section)
                .ThenInclude(x => x.CourseTerm)
                .ThenInclude(x => x.Term)
             .ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentEnrolledReportDatatable(DataTablesStructs.SentParameters sentParameters, int sex, Guid? careerId = null, Guid? termId = null, ClaimsPrincipal user = null)
        {
            var studentSections = _context.StudentSections.Where(ss => !ss.Section.IsDirectedCourse).AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    studentSections = studentSections.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                }
            }

            if (careerId != null)
            {
                studentSections = studentSections.Where(x => x.Student.CareerId == careerId);
            }

            if (termId != null)
            {
                studentSections = studentSections.Where(x => x.Section.CourseTerm.TermId == termId);
            }
            if (sex != 0)
            {
                studentSections = studentSections.Where(x => x.Student.User.Sex == sex);
            }

            var recordsFiltered = await studentSections
                .Select(x => new { x.Student.CareerId, x.Student.User.Sex, x.Section.CourseTerm.TermId })
                .Distinct()
                .CountAsync();

            var data = await studentSections
                .Select(x => new
                {
                    x.StudentId,
                    x.Student.CareerId,
                    CareerName = x.Student.Career.Name,
                    x.Student.User.Sex,
                    x.Section.CourseTerm.TermId,
                    TermName = x.Section.CourseTerm.Term.Name
                })
                .Distinct()
                .GroupBy(x => new { x.CareerName, x.CareerId, x.Sex, x.TermId, x.TermName })
                .OrderBy(x => x.Key.CareerName)
                .Select(x => new
                {
                    Career = x.Key.CareerName,
                    Sex = ConstantHelpers.SEX.VALUES.ContainsKey(x.Key.Sex) ? ConstantHelpers.SEX.VALUES[x.Key.Sex] : "-",
                    Term = x.Key.TermName,
                    Accepted = x.Count()
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

        public async Task<object> GetStudentEnrolledReportChart(int sex, Guid? careerId = null, Guid? termId = null, ClaimsPrincipal user = null)
        {
            var studentSections = _context.StudentSections
                .Where(x => !x.Section.IsDirectedCourse)
                .AsQueryable();

            var careerQuery = _context.Careers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    studentSections = studentSections.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                    careerQuery = careerQuery.Where(x => x.QualityCoordinatorId == userId);
                }
            }

            //Filtramos
            if (careerId != null)
            {
                studentSections = studentSections.Where(x => x.Student.CareerId == careerId);
            }

            if (termId != null)
            {
                studentSections = studentSections.Where(x => x.Section.CourseTerm.TermId == termId);
            }
            if (sex != 0)
            {
                studentSections = studentSections.Where(x => x.Student.User.Sex == sex);
            }



            var data = await careerQuery
                .Select(x => new
                {
                    Career = x.Name,
                    Count = studentSections.Where(y => y.Student.CareerId == x.Id).Select(x => new { x.StudentId, x.Section.CourseTerm.TermId }).Distinct().Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Career)
                .ToListAsync();


            var result = new
            {
                categories = data.Select(x => x.Career).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;

        }

        //Reporte 26
        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentEnrolledByInstitutionReportDatatable(DataTablesStructs.SentParameters sentParameters, int sex, string originSchool = null, Guid? careerId = null, Guid? termId = null, ClaimsPrincipal user = null)
        {
            var studentSections = _context.StudentSections
                .Where(x => !x.Section.IsDirectedCourse).AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    studentSections = studentSections.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                }
            }

            if (careerId != null)
            {
                studentSections = studentSections.Where(x => x.Student.CareerId == careerId);
            }
            if (termId != null)
            {
                studentSections = studentSections.Where(x => x.Section.CourseTerm.TermId == termId);
            }
            if (sex != 0)
            {
                studentSections = studentSections.Where(x => x.Student.User.Sex == sex);
            }
            if (originSchool != null)
            {
                studentSections = studentSections.Where(x => x.Student.StudentInformation.OriginSchool.ToUpper().Trim() == originSchool.ToUpper().Trim());
            }

            var recordsFiltered = await studentSections
                .Select(x => new { x.Student.CareerId, x.Student.User.Sex, OriginSchool = x.Student.StudentInformation.OriginSchool.ToUpper().Trim(), x.Section.CourseTerm.TermId })
                .Distinct()
                .CountAsync();

            var data = await studentSections
                .Select(x => new
                {
                    x.StudentId,
                    x.Student.CareerId,
                    OriginSchool = x.Student.StudentInformation.OriginSchool.ToUpper().Trim(),
                    CareerName = x.Student.Career.Name,
                    x.Student.User.Sex,
                    x.Section.CourseTerm.TermId,
                    TermName = x.Section.CourseTerm.Term.Name
                })
                .Distinct()
                .GroupBy(x => new { x.CareerName, x.CareerId, x.Sex, x.TermId, x.TermName, x.OriginSchool })
                .OrderByDescending(x => x.Count())
                .ThenBy(x => x.Key.CareerName)
                .Select(x => new
                {
                    Career = x.Key.CareerName,
                    Sex = ConstantHelpers.SEX.VALUES.ContainsKey(x.Key.Sex) ? ConstantHelpers.SEX.VALUES[x.Key.Sex] : "-",
                    Term = x.Key.TermName,
                    x.Key.OriginSchool,
                    Accepted = x.Count()
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

        public async Task<object> GetStudentEnrolledByInstitutionReportChart(int sex, string originSchool = null, Guid? careerId = null, Guid? termId = null, ClaimsPrincipal user = null)
        {
            var studentSections = _context.StudentSections.Where(x => !x.Section.IsDirectedCourse).AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    studentSections = studentSections.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                }
            }

            //Filtramos
            if (careerId != null)
            {
                studentSections = studentSections.Where(x => x.Student.CareerId == careerId);
            }

            if (termId != null)
            {
                studentSections = studentSections.Where(x => x.Section.CourseTerm.TermId == termId);
            }
            if (sex != 0)
            {
                studentSections = studentSections.Where(x => x.Student.User.Sex == sex);
            }
            if (originSchool != null)
            {
                studentSections = studentSections.Where(x => x.Student.StudentInformation.OriginSchool.ToUpper() == originSchool.ToUpper());
            }

            var data = await studentSections
                .Select(x => new
                {
                    x.StudentId,
                    x.Student.CareerId,
                    CareerName = x.Student.Career.Name,
                    x.Section.CourseTerm.TermId
                })
                .Distinct()
                .GroupBy(x => new { x.CareerName, x.CareerId })
                .OrderByDescending(x => x.Count())
                .ThenBy(x => x.Key.CareerName)
                .Select(x => new
                {
                    Career = x.Key.CareerName,
                    Accepted = x.Count()
                })
                .ToListAsync();

            var result = new
            {
                categories = data.Select(x => x.Career).ToList(),
                data = data.Select(x => x.Accepted).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentsByCurriculumDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> curriculums, Guid careerId, Guid termId)
        {
            var term = await _context.Terms.FindAsync(termId);

            var students = await _context.Students
                .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId))
                .Select(x => new
                {
                    CurriculumId = term.Status == ConstantHelpers.TERM_STATES.ACTIVE ? x.CurriculumId
                        : x.AcademicSummaries.Where(y => y.TermId == termId).Select(y => y.CurriculumId).FirstOrDefault(),
                    AcademicYear = term.Status == ConstantHelpers.TERM_STATES.ACTIVE ? x.CurrentAcademicYear
                        : x.AcademicSummaries.Where(y => y.TermId == termId).Select(y => y.StudentAcademicYear).FirstOrDefault()
                }).ToListAsync();

            var curriculumsList = await _context.Curriculums
                .Where(x => curriculums.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Year,
                    Career = x.Career.Name
                }).ToListAsync();

            var curriculumList = await _context.Curriculums.Where(x => curriculums.Any(y => y == x.Id)).ToListAsync();

            var result = curriculumsList
               .Select(x => new
               {
                   curriculum = $"{x.Year}-{x.Code}",
                   one = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 1),
                   two = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 2),
                   three = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 3),
                   four = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 4),
                   five = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 5),
                   six = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 6),
                   seven = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 7),
                   eight = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 8),
                   nine = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 9),
                   ten = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 10),
                   eleven = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 11),
                   twelve = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 12),
                   thirteen = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 13),
                   fourteen = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 14),
                   fifteen = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 15),
               }).ToList();

            var recordsFiltered = result.Count;

            var recordsTotal = result.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = result,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<List<CurriculumEnrolledStudentsTemplate>> GetEnrolledStudentsByCurriculum(List<Guid> curriculums, Guid termId)
        {
            var term = await _context.Terms.FindAsync(termId);

            var students = await _context.Students
                .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId))
                .Select(x => new
                {
                    CurriculumId = term.Status == ConstantHelpers.TERM_STATES.ACTIVE ? x.CurriculumId
                        : x.AcademicSummaries.Where(y => y.TermId == termId).Select(y => y.CurriculumId).FirstOrDefault(),
                    AcademicYear = term.Status == ConstantHelpers.TERM_STATES.ACTIVE ? x.CurrentAcademicYear
                        : x.AcademicSummaries.Where(y => y.TermId == termId).Select(y => y.StudentAcademicYear).FirstOrDefault()
                }).ToListAsync();

            var curriculumsList = await _context.Curriculums
                .Where(x => curriculums.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Year,
                    Career = x.Career.Name
                }).ToListAsync();

            var result = curriculumsList
                .Select(x => new CurriculumEnrolledStudentsTemplate
                {
                    Curriculum = $"{x.Year}-{x.Code}",
                    Semester1 = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 1),
                    Semester2 = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 2),
                    Semester3 = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 3),
                    Semester4 = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 4),
                    Semester5 = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 5),
                    Semester6 = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 6),
                    Semester7 = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 7),
                    Semester8 = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 8),
                    Semester9 = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 9),
                    Semester10 = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 10),
                    Semester11 = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 11),
                    Semester12 = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 12),
                    Semester13 = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 13),
                    Semester14 = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 14),
                    Semester15 = students.Count(y => y.CurriculumId == x.Id && y.AcademicYear == 15),
                }).ToList();

            return result;
        }


        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentByEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid evaluationReportId, string searchValue = null)
        {
            var evaluationReport = await _context.EvaluationReports.FirstOrDefaultAsync(x => x.Id == evaluationReportId);

            if (evaluationReport.Type == CORE.Helpers.ConstantHelpers.Intranet.EvaluationReportType.DIRECTED_COURSE)
            {
                Expression<Func<DirectedCourse, dynamic>> orderByPredicate = null;

                switch (sentParameters.OrderColumn)
                {
                    case "0":
                        orderByPredicate = (x) => x.CourseId;
                        break;
                    //case "1":
                    //    orderByPredicate = (x) => x.StudentId;
                    //    break;
                    default:
                        orderByPredicate = (x) => x.TeacherId;
                        break;
                }

                var query = _context.DirectedCourses
                    .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                    .AsNoTracking();

                query = query.Where(x => x.CourseId == evaluationReport.CourseId && x.TeacherId == evaluationReport.TeacherId)
                    .AsNoTracking();

                var recordsFiltered = await query.CountAsync();

                var data = await query
                    .Select(x => new
                    {
                        //student = x.Student.User.FullName,
                        //finalGrade = x.Grade
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
            else
            {
                Expression<Func<StudentSection, dynamic>> orderByPredicate = null;

                switch (sentParameters.OrderColumn)
                {
                    case "0":
                        orderByPredicate = (x) => x.SectionId;
                        break;
                    case "1":
                        orderByPredicate = (x) => x.StudentId;
                        break;
                    default:
                        orderByPredicate = (x) => x.SectionId;
                        break;
                }


                var query = _context.StudentSections
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .AsNoTracking();

                query = query.Where(x => x.SectionId == evaluationReport.SectionId && !x.Section.IsDirectedCourse);

                if (!string.IsNullOrEmpty(searchValue))
                    query = query.Where(x => x.Student.User.FullName.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

                var recordsFiltered = await query.CountAsync();

                var data = await query
                    .Select(x => new
                    {
                        code = x.Student.User.UserName,
                        student = x.Student.User.FullName,
                        finalGrade = $"{x.FinalGrade:00}"
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
        }

        public async Task<EnrolledByGroupsDataTemplate> GetEnrolledStudentsByGroupData(Guid termId, Guid careerId, Guid curriculums)
        {
            var academicyears = await _context.AcademicYearCourses.Where(x => (x.Course.CareerId.HasValue) ? x.Course.CareerId.Value == careerId : false).ToListAsync();
            var studentsections = await _context.StudentSections
                .Where(m => curriculums == m.Student.CurriculumId && m.Section.CourseTerm.TermId == termId && !m.Section.IsDirectedCourse)
                .Select(x => new
                {
                    x.SectionId
                })
                .ToListAsync();
            var sqlData = await _context.CourseTerms
                                            .Where(y => y.TermId == termId && y.Course.CareerId.Value == careerId)
                                            .Select(x => new
                                            {
                                                x.CourseId,
                                                //AcademicYear = academicyears.FirstOrDefault(a => a.CourseId == x.CourseId) != null ? academicyears.FirstOrDefault(a => a.CourseId == x.CourseId).AcademicYear : Convert.ToByte(0),
                                                CourseName = x.Course.Name,
                                                CourseCode = x.Course.Code,
                                                Esp = x.Course.AcademicProgram.Code,
                                                AcademicProgram = x.Course.AcademicProgram.Name,
                                                Sections = x.Sections.ToList()
                                                //Sections = x.Sections
                                                //            .Select(j => new SectionDataTemplate
                                                //            {
                                                //                Code = j.Code,
                                                //                //students = studentsections.Where(m => m.SectionId == j.Id).Count()
                                                //            }).ToList()
                                            })
                                            .ToListAsync();

            var courses = sqlData
                .Select(x => new CourseTermsTemplate
                {
                    AcademicYear = academicyears.FirstOrDefault(a => a.CourseId == x.CourseId) != null ? academicyears.FirstOrDefault(a => a.CourseId == x.CourseId).AcademicYear : Convert.ToByte(0),
                    CourseName = x.CourseName,
                    CourseCode = x.CourseCode,
                    Esp = x.Esp,
                    AcademicProgram = x.AcademicProgram,
                    Sections = x.Sections
                    .Select(j => new SectionDataTemplate
                    {
                        Code = j.Code,
                        students = studentsections.Where(m => m.SectionId == j.Id).Count()
                    }).ToList()
                }).ToList();

            var codes = await _context.SectionCodes.OrderBy(x => x.Description).ToListAsync();

            var curriculum = await _context.Curriculums.Where(x => x.Id == curriculums).FirstOrDefaultAsync();

            var curriculumT = new CurriculumTemplate()
            {
                Year = curriculum.Year,
                Code = curriculum.Code,
                TextCurriculum = $"{curriculum.Year}-{curriculum.Code}"
            };

            return new EnrolledByGroupsDataTemplate { StudentSectionByGroup = courses, Codes = codes, Curriculum = curriculumT };
        }

        public async Task<DataTablesStructs.ReturnedData<CourseModalityTemplate>> GetEnrolledStudentsByModalityDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid careerId)
        {

            var result = await GetEnrolledStudentsByModalityData(termId, careerId);

            var recordsFiltered = result.Count;

            var data = result
                   //.Skip(sentParameters.PagingFirstRecord)
                   //.Take(sentParameters.RecordsPerDraw)
                   .ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<CourseModalityTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<CourseModalityTemplate>> GetEnrolledStudentsByModalityData(Guid termId, Guid careerId)
        {
            //var academicyears = await _context.AcademicYearCourses.Where(x => (x.Course.CareerId.HasValue) ? x.Course.CareerId.Value == careerId : false).ToListAsync();
            var academicyears = await _context.AcademicYearCourses.Where(x => x.Course.CareerId == careerId).ToListAsync();

            var studentsections = await _context.StudentSections
                .Where(m => careerId == m.Student.CareerId && m.Section.CourseTerm.TermId == termId && !m.Section.IsDirectedCourse)
                .Select(x => new
                {
                    x.Student.Status,
                    x.Try,
                    x.Section.CourseTerm.CourseId
                })
                .ToListAsync();

            var directedCourseStudents = await _context.StudentSections
                .Where(x => x.Student.CareerId == careerId && x.Section.IsDirectedCourse && x.Section.CourseTerm.TermId == termId)
                .Select(x => new
                {
                    x.Student.Status,
                    x.Try,
                    x.Section.CourseTerm.CourseId
                }).ToListAsync();

            var courses = await _context.CourseTerms
                                    .Where(y => y.TermId == termId && y.Course.CareerId.Value == careerId)
                                    .Select(x => new CourseModalityTemplate
                                    {
                                        //intAcademicYear = academicyears.FirstOrDefault(a => a.CourseId == x.CourseId) != null ? academicyears.FirstOrDefault(a => a.CourseId == x.CourseId).AcademicYear : 0,
                                        CourseId = x.CourseId,
                                        CourseName = x.Course.Name,
                                        CourseCode = x.Course.Code,
                                        //GroupName = x.Sections.Select(j => j.Code).FirstOrDefault(),                                  
                                        Dir = 0,
                                        Esp = x.Course.AcademicProgram.Code,
                                    })
                                    //.Where(x => x.Total > 0)
                                    //.OrderBy(x => x.intAcademicYear)
                                    //.ThenBy(x => x.GroupName)
                                    //.ThenBy(x => x.CourseCode)
                                    .ToListAsync();

            foreach (var item in courses)
            {
                item.intAcademicYear = academicyears.FirstOrDefault(a => a.CourseId == item.CourseId) != null ? academicyears.FirstOrDefault(a => a.CourseId == item.CourseId).AcademicYear : 0;
                item.Regular = studentsections.Where(c => c.CourseId == item.CourseId && c.Status != ConstantHelpers.Student.States.RESERVED && c.Status != ConstantHelpers.Student.States.OBSERVED && c.Try < 3).Count();
                item.Observed = studentsections.Where(c => c.CourseId == item.CourseId && c.Status == ConstantHelpers.Student.States.OBSERVED && c.Try < 3).Count();
                item.Reserved = studentsections.Where(c => c.CourseId == item.CourseId && c.Status == ConstantHelpers.Student.States.RESERVED && c.Try < 3).Count();
                item.Third = studentsections.Where(y => y.CourseId == item.CourseId && y.Try == 3).Count();
                item.Fourth = studentsections.Where(y => y.CourseId == item.CourseId && y.Try == 4).Count();
                item.Fifth = studentsections.Where(y => y.CourseId == item.CourseId && y.Try == 5).Count();
                item.Sixth = studentsections.Where(y => y.CourseId == item.CourseId && y.Try == 6).Count();
                item.Seventh = studentsections.Where(y => y.CourseId == item.CourseId && y.Try == 7).Count();
                item.Eighth = studentsections.Where(y => y.CourseId == item.CourseId && y.Try == 8).Count();
                item.Ninth = studentsections.Where(y => y.CourseId == item.CourseId && y.Try == 9).Count();
                item.Tenth = studentsections.Where(y => y.CourseId == item.CourseId && y.Try == 10).Count();
                item.Eleventh = studentsections.Where(y => y.CourseId == item.CourseId && y.Try == 11).Count();
                item.Twelfth = studentsections.Where(y => y.CourseId == item.CourseId && y.Try == 12).Count();
                item.Thirteenth = studentsections.Where(y => y.CourseId == item.CourseId && y.Try == 13).Count();
                item.Fourteenth = studentsections.Where(y => y.CourseId == item.CourseId && y.Try == 14).Count();
                item.Fifteenth = studentsections.Where(y => y.CourseId == item.CourseId && y.Try == 15).Count();
                item.Dir = directedCourseStudents.Where(y => y.CourseId == item.CourseId).Count();
            }

            courses = courses.Where(x => x.Total > 0)
                .OrderBy(x => x.intAcademicYear)
                .ThenBy(x => x.GroupName)
                .ThenBy(x => x.CourseCode).ToList();
            return courses;
        }

        public async Task<object> GetEnrolledTrends()
        {
            return await _context.StudentSections
                .Where(x => !x.Section.IsDirectedCourse)
                 .GroupBy(s => new { s.StudentId, s.Section.CourseTerm.TermId })
                 .GroupBy(s => s.Key.TermId)
                 .Select(s => new
                 {
                     term = _context.Terms.Find(s.Key).Name,
                     enrolledCount = s.Count()
                 }).ToListAsync();
        }
        public async Task UpdateStudentSectionFinalGradeJob()
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            var studentSections = await _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == term.Id && !x.Section.IsDirectedCourse)
                .Include(x => x.Section)
                .ToListAsync();

            var evaluations = await _context.Evaluations.Where(x => x.CourseTerm.TermId == term.Id).ToListAsync();

            foreach (var item in studentSections)
            {
                var courseEvaluations = evaluations.Where(x => x.CourseTermId == item.Section.CourseTermId).ToList();
                var totalPercetage = 1.0M * courseEvaluations.Sum(x => x.Percentage);

                if (totalPercetage <= 0) continue;

                var grades = await _context.Grades.Where(x => x.StudentSectionId == item.Id && x.EvaluationId.HasValue).SumAsync(x => x.Value * x.Evaluation.Percentage);

                var finalGrade = (int)Math.Round(grades / totalPercetage, 0, MidpointRounding.AwayFromZero);

                item.FinalGrade = finalGrade;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyBySectionAndStudentId(Guid sectionId, Guid studentId)
        {
            return await _context.StudentSections.AnyAsync(x => x.SectionId == sectionId && x.StudentId == studentId);
        }
        #endregion

        public async Task<bool> IsStudentEnabledForCourse(Guid sectionId, Guid studentId)
        {
            var courseId = await _context.Sections.Where(x => x.Id == sectionId).Select(x => x.CourseTerm.CourseId).FirstOrDefaultAsync();
            var approvedAcademicHistories = await _context.AcademicHistories.Where(x => x.StudentId == studentId && x.CourseId == courseId && x.Approved).AnyAsync();
            return !approvedAcademicHistories;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledSectionsByTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? curriculumId = null, string searchValue = null, ClaimsPrincipal user = null, byte? academicYear = null, bool? withStudentsInProcess = null)
        {
            Expression<Func<Section, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.Code); break;
                case "2":
                    orderByPredicate = ((x) => x.CourseTerm.Course.Code); break;
                case "3":
                    orderByPredicate = ((x) => x.CourseTerm.Course.Name); break;
                case "4":
                    orderByPredicate = ((x) => x.StudentSections.Count); break;
                case "0":
                default:
                    orderByPredicate = (x) => x.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault();
                    break;
            }

            var query = _context.Sections.AsNoTracking();

            var studentSections = _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == termId && !x.Section.IsDirectedCourse)
                .AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    //var faculties = await _context.Faculties.Where(x => x.Careers.Any(y => y.AcademicCoordinatorId == userId || y.CareerDirectorId == userId)).Select(x => x.Id).ToListAsync();
                    var careers = await _context.Careers.Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId).Select(x => x.Id).ToListAsync();
                    //var academicPrograms = await _context.AcademicPrograms.Where(x => x.Career.AcademicCoordinatorId == userId || x.Career.CareerDirectorId == userId).Select(x => x.Id).ToListAsync();

                    //studentSections = studentSections.Where(x => faculties.Contains(x.Student.Career.FacultyId));
                    studentSections = studentSections.Where(x => careers.Contains(x.Student.CareerId));
                    //studentSections = studentSections.Where(x => academicPrograms.Contains(x.Student.AcademicProgramId.Value));

                }
            }

            if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    studentSections = studentSections.Where(x => x.Student.Career.Faculty.DeanId == userId || x.Student.Career.Faculty.SecretaryId == userId || x.Student.Career.Faculty.AdministrativeAssistantId == userId);
                }
            }

            if (facultyId != null)
                studentSections = studentSections.Where(x => x.Student.Career.FacultyId == facultyId);

            if (careerId != null)
                studentSections = studentSections.Where(x => x.Student.CareerId == careerId);

            if (academicProgramId != null)
                studentSections = studentSections.Where(x => x.Student.AcademicProgramId == academicProgramId);

            if (curriculumId != null)
                studentSections = studentSections.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (academicYear.HasValue)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.AcademicYear == academicYear));

            if (!string.IsNullOrEmpty(searchValue))
            {
                studentSections = studentSections.Where(x => x.Section.Code.ToUpper().Contains(searchValue.ToUpper()) ||
                                x.Section.Code.ToUpper().Contains(searchValue.ToUpper()) ||
                                x.Section.CourseTerm.Course.Code.ToUpper().Contains(searchValue.ToUpper()) ||
                                x.Section.CourseTerm.Course.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            if (withStudentsInProcess.HasValue && withStudentsInProcess.Value)
                query = query.Where(x => x.StudentSections.Any(y => y.Status == ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS));

            var sectionsFiltered = await studentSections.Select(x => x.SectionId).Distinct().ToListAsync();

            query = query.Where(x => sectionsFiltered.Any(y => y == x.Id));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    section = x.Code,
                    course = $"{x.CourseTerm.Course.Code} - {x.CourseTerm.Course.Name}",
                    academicProgram = x.CourseTerm.Course.AcademicProgram.Name,
                    academicYear = x.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault(),
                    students = x.StudentSections.Count
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
        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledCoursesByTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? curriculumId = null, string searchValue = null, ClaimsPrincipal user = null, byte? academicYear = null)
        {
            Expression<Func<CourseTerm, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.Course.Code); break;
                case "2":
                    orderByPredicate = ((x) => x.Course.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.Course.AcademicProgram.Name); break;
                case "0":
                default:
                    //orderByPredicate = (x) => x.Course.Code;
                    orderByPredicate = (x) => x.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault();
                    break;
            }

            var courses = _context.CourseTerms
                .Where(x => x.TermId == termId)
                .AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    courses = courses.Where(x => x.Course.Career.AcademicCoordinatorId == userId || x.Course.Career.CareerDirectorId == userId || x.Course.Career.AcademicSecretaryId == userId);
                }
            }
            if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    courses = courses.Where(x => x.Course.Career.Faculty.DeanId == userId || x.Course.Career.Faculty.SecretaryId == userId || x.Course.Career.Faculty.AdministrativeAssistantId == userId);
                }
            }

            if (facultyId != null) courses = courses.Where(x => x.Course.Career.FacultyId == facultyId);
            if (careerId != null) courses = courses.Where(x => x.Course.CareerId == careerId);
            if (academicProgramId != null) courses = courses.Where(x => x.Course.AcademicProgramId == academicProgramId);
            if (curriculumId != null) courses = courses.Where(x => x.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (!string.IsNullOrEmpty(searchValue))
            {
                courses = courses.Where(x => x.Course.Code.ToUpper().Contains(searchValue.ToUpper()) ||
                                x.Course.Code.ToUpper().Contains(searchValue.ToUpper()) ||
                                x.Course.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            if (academicYear.HasValue)
                courses = courses.Where(x => x.Course.AcademicYearCourses.Any(y => y.AcademicYear == academicYear));

            courses = courses.Where(x => x.Sections.Any(y => y.StudentSections.Count > 0));

            var recordsFiltered = await courses.CountAsync();

            var dbdata = await courses
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    academicYear = x.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault(),
                    code = x.Course.Code,
                    course = $"{x.Course.Name}",
                    academicProgram = x.Course.AcademicProgram.Name,
                    students = x.Sections.Select(y => y.StudentSections.Count).ToList()
                    //students = filtered.Where(y => y.coursetermid == x.Id).Count()
                })
                .ToListAsync();

            var data = dbdata
                .Select(x => new
                {
                    x.Id,
                    academicYear = x.academicYear,
                    code = x.code,
                    course = x.course,
                    academicProgram = x.academicProgram,
                    students = x.students.Sum(y => y)
                }).ToList();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<EnrolledCourseTermTemplate>> GetEnrolledCoursesByTermData(Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? curriculumId = null, string searchValue = null, ClaimsPrincipal user = null, byte? academicYear = null)
        {
            var courses = _context.CourseTerms
                .Where(x => x.TermId == termId)
                .AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    courses = courses.Where(X => X.Course.Career.AcademicCoordinatorId == userId || X.Course.Career.CareerDirectorId == userId || X.Course.Career.AcademicSecretaryId == userId);
                }
            }
            if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    courses = courses.Where(x => x.Course.Career.Faculty.DeanId == userId || x.Course.Career.Faculty.SecretaryId == userId);
                }
            }

            if (facultyId != null) courses = courses.Where(x => x.Course.Career.FacultyId == facultyId);
            if (careerId != null) courses = courses.Where(x => x.Course.CareerId == careerId);
            if (academicProgramId != null) courses = courses.Where(x => x.Course.AcademicProgramId == academicProgramId);
            if (curriculumId != null) courses = courses.Where(x => x.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (!string.IsNullOrEmpty(searchValue))
            {
                courses = courses.Where(x => x.Course.Code.ToUpper().Contains(searchValue.ToUpper()) ||
                                x.Course.Code.ToUpper().Contains(searchValue.ToUpper()) ||
                                x.Course.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            if (academicYear.HasValue)
                courses = courses.Where(x => x.Course.AcademicYearCourses.Any(y => y.AcademicYear == academicYear));

            var recordsFiltered = await courses.CountAsync();

            courses = courses.Where(x => x.Sections.Any(y => y.StudentSections.Count > 0));

            var dbdata = await courses
                .Select(x => new
                {
                    academicYear = x.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault(),
                    code = x.Course.Code,
                    course = $"{x.Course.Name}",
                    academicProgram = x.Course.AcademicProgram.Name,
                    students = x.Sections.Select(y => y.StudentSections.Count).ToList(),
                    area = x.Course.AreaId.HasValue ? x.Course.Area.Name : "",
                    career = x.Course.AcademicYearCourses.Select(y => y.Curriculum.Career.Name).FirstOrDefault() ?? "-",
                    faculty = x.Course.AcademicYearCourses.Select(y => y.Curriculum.Career.Faculty.Name).FirstOrDefault() ?? "-",
                })
                .ToListAsync();

            var data = dbdata
                .Select(x => new EnrolledCourseTermTemplate
                {
                    AcademicYear = x.academicYear,
                    Code = x.code,
                    Course = x.course,
                    AcademicProgram = x.academicProgram,
                    Students = x.students.Sum(y => y),
                    Area = x.area,
                    Faculty = x.faculty,
                    Career = x.career
                }).ToList();

            return data;
        }


        public async Task<List<Student>> GetEnrolledStudentsBySection(Guid sectionId)
        {
            var query = _context.Students.AsNoTracking();

            var studentsInSection = await _context.StudentSections
                .Where(x => x.SectionId == sectionId)
                .Select(x => x.StudentId)
                .ToListAsync();

            query = query.Where(x => studentsInSection.Any(y => y == x.Id));

            return await query.Include(x => x.User).OrderBy(x => x.User.FullName).ToListAsync();
        }

        public async Task<List<StudentSection>> GetEnrolledStudentsByCourse(Guid courseTermId)
        {
            var studentSections = await _context.StudentSections
                .Where(x => x.Section.CourseTermId == courseTermId)
                .Include(x => x.Student).ThenInclude(x => x.User)
                .Include(x => x.Section)
                .AsNoTracking()
                .ToListAsync();

            return studentSections.OrderBy(x => x.Student.User.FullName).ToList();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentsBySectionDatatable(DataTablesStructs.SentParameters sentParameters, Guid sectionId, string searchValue = null, int? studentSectionStatus = null)
        {
            var query = _context.StudentSections
                .Where(x => x.SectionId == sectionId)
                .AsNoTracking();

            if (studentSectionStatus.HasValue)
                query = query.Where(x => x.Status == studentSectionStatus);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()) || x.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var dataDB = await query
                .OrderBy(x => x.Student.User.FullName)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    studentId = x.StudentId,
                    username = x.Student.User.UserName,
                    fullname = x.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN ? $"{x.Student.User.FullName} (RET.)" : x.Student.User.FullName,
                    @try = x.Try,
                    x.Section.IsDirectedCourse
                }).ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.studentId,
                    x.username,
                    x.fullname,
                    modality = x.IsDirectedCourse ? "DIRIGIDO"
                    : ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES.ContainsKey(x.@try) ? ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES[x.@try] : "REGULAR",
                    x.@try
                })
                .ToList();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentsByCourseDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseId, string searchValue = null)
        {

            var query = _context.StudentSections
                .Where(x => x.Section.CourseTermId == courseId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()) || x.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper()));


            int recordsFiltered = await query.CountAsync();

            var dbData = await query
                    .OrderBy(x => x.Student.User.FullName)
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .Select(x => new
                    {
                        id = x.Id,
                        username = x.Student.User.UserName,
                        fullname = x.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN ? $"{x.Student.User.FullName} (RET.)" : x.Student.User.FullName,
                        //modality = x.AdmissionType.Name,
                        cycle = x.Student.CurrentAcademicYear,
                        section = x.Section.Code,
                        @try = x.Try,
                        x.Section.IsDirectedCourse
                    })
                    .ToListAsync();

            var data = dbData
                .Select(x => new
                {
                    x.username,
                    x.fullname,
                    modality = x.IsDirectedCourse ? "DIRIGIDO" :
                        ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES.ContainsKey(x.@try) ? ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES[x.@try] : "REGULAR",
                    x.cycle,
                    x.section
                })
                .ToList();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetSummerEnrolledBySection(Guid sectionId)
        {
            var result = await _context.StudentSections
                .Where(x => x.SectionId == sectionId)
                .Select(x => new
                {
                    code = x.Student.User.UserName,
                    name = x.Student.User.FullName,
                    @try = x.Student.AcademicHistories.Where(y => y.CourseId == x.Section.CourseTerm.CourseId).Count() + 1,
                    id = x.Id,
                    program = x.Student.AcademicProgramId.HasValue ? x.Student.AcademicProgram.Name : "---",
                    academicYear = x.Student.CurrentAcademicYear
                })
                //.OrderBy(x => x.name)
                .ToListAsync(); ;

            return result;
        }

        public async Task DeleteIfEnrolledInAnotherSection(Guid sectionId, Guid studentId)
        {
            var courseTerm = await _context.Sections.Where(x => x.Id == sectionId).Select(x => x.CourseTerm).FirstOrDefaultAsync();

            var studentSectionsToDelete = await _context.StudentSections
                .Where(x => x.SectionId != sectionId && x.StudentId == studentId
                && x.Section.CourseTerm.CourseId == courseTerm.CourseId && x.Section.CourseTerm.TermId == courseTerm.TermId && !x.Section.IsDirectedCourse)
                .ToListAsync();

            _context.StudentSections.RemoveRange(studentSectionsToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<Tuple<bool, string>> InsertWithValidations(StudentSection studentSection)
        {
            var exist = await _context.StudentSections
                .AnyAsync(x => x.SectionId == studentSection.SectionId && x.StudentId == studentSection.StudentId);
            if (exist) return new Tuple<bool, string>(false, "El alumno ya está matriculado en la sección");

            var academicHistories = await _context.AcademicHistories.Where(x => x.StudentId == studentSection.StudentId && x.Approved).ToListAsync();

            var courseTerm = await _context.Sections.Where(x => x.Id == studentSection.SectionId).Select(x => x.CourseTerm).FirstOrDefaultAsync();
            var approvedAcademicHistories = academicHistories.Any(x => x.CourseId == courseTerm.CourseId);

            if (approvedAcademicHistories) return new Tuple<bool, string>(false, "El alumno seleccionado ya aprobó el curso");


            //var prerequisites = await _context.AcademicYearCoursePreRequisites
            //    .Where(x => x.AcademicYearCourse.CourseId == courseTerm.CourseId)
            //    .ToListAsync();

            //if (!prerequisites.All(x => academicHistories.Any(y => y.CourseId == x.CourseId))) return new Tuple<bool, string>(false, "El alumno no cumple con los pre-requisitos del curso");

            var studentSectionsToDelete = await _context.StudentSections
                .Where(x => x.SectionId != studentSection.SectionId && x.StudentId == studentSection.StudentId
                && x.Section.CourseTerm.CourseId == courseTerm.CourseId && x.Section.CourseTerm.TermId == courseTerm.TermId && !x.Section.IsDirectedCourse)
                .ToListAsync();
            _context.StudentSections.RemoveRange(studentSectionsToDelete);

            await _context.StudentSections.AddAsync(studentSection);
            await _context.SaveChangesAsync();

            return new Tuple<bool, string>(true, string.Empty);
        }

        public async Task UpdateStudentSectionFinalGrade(List<Guid> sectionStudents, Guid termId)
        {
            var students = _context.StudentSections.Include(x => x.Section.CourseTerm.Course).Where(x => sectionStudents.Any(y => y == x.Id) && !x.Section.IsDirectedCourse).AsQueryable();

            foreach (var item in students)
            {
                var evaluations = await _context.Evaluations
               .Where(x => x.CourseTerm.CourseId == item.Section.CourseTerm.CourseId && x.CourseTerm.TermId == termId)
               .Select(x => new ENTITIES.Models.Enrollment.Evaluation
               {
                   Id = x.Id,
                   Name = x.Name,
                   Percentage = x.Percentage,
                   Retrievable = x.Retrievable,
                   Taken = x.Grades.Any(g => g.StudentSection.SectionId == item.SectionId),
                   Week = x.Week
               }).OrderBy(x => x.Week).ThenBy(x => x.Name).ToListAsync();

                var grades = await _context.Grades.Include(x => x.Evaluation).Where(x => x.StudentSectionId == item.Id && x.Evaluation != null).ToArrayAsync();
                var gradesSum = grades.Sum(x => x.Value * x.Evaluation.Percentage);
                var totalPercentage = evaluations.Sum(x => x.Percentage);

                var studentSection = item;
                var calc = 0.0M;
                if (gradesSum != 0 && totalPercentage != 0)
                {
                    calc = gradesSum / totalPercentage;
                }
                var finalGrade = (int)Math.Round(calc, 0, MidpointRounding.AwayFromZero);
                studentSection.FinalGrade = finalGrade;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentSectionsWithSectionGroupDatatable(DataTablesStructs.SentParameters sentParameters, Guid sectionId, Guid? sectionGroupId = null, string searchValue = null)
        {
            var query = _context.StudentSections
                .Where(x => x.SectionId == sectionId).AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Student.User.FullName.ToLower().Contains(searchValue.Trim().ToLower()));

            query = query.Where(x => x.SectionGroupId == sectionGroupId);

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderBy(x => x.Student.User.FullName)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    username = x.Student.User.UserName,
                    fullname = x.Student.User.FullName,
                    dni = x.Student.User.Dni,
                    sectionGroup = x.SectionGroupId.HasValue ? x.SectionGroup.Code : "No Asignado",
                    x.SectionGroupId
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

        public async Task GroupAlphabeticallyInSectionGroups(Guid sectionId)
        {
            var studentSections = await _context
                .StudentSections.Where(x => x.SectionId == sectionId)
                .OrderBy(x => x.Student.User.FullName)
                .ToListAsync();

            var sectionGroups = await _context.ClassSchedules.Where(x => x.SectionId == sectionId && x.SectionGroupId.HasValue)
                .OrderBy(x => x.SectionGroup.Code)
                .GroupBy(x => x.SectionGroupId)
                .Select(x => x.Key)
                .ToArrayAsync();

            var sectionGroupsTest = await _context.ClassSchedules.Where(x => x.SectionId == sectionId && x.SectionGroupId.HasValue)
                .Select(x => new
                {
                    x.SectionGroupId,
                    x.SectionGroup.Code
                })
                .ToListAsync();

            var tpmEnrollments = await _context.TmpEnrollments.Where(x => x.SectionId == sectionId).ToListAsync();

            var count = (int)Math.Round((decimal)studentSections.Count() / sectionGroups.Count(), MidpointRounding.AwayFromZero);
            var skip = 0;
            for (int i = 0; i < sectionGroups.Count(); i++)
            {
                if (i == sectionGroups.Count() - 1)
                {
                    studentSections.Skip(skip).ToList().ForEach(x => { x.SectionGroupId = sectionGroups[i]; });
                }
                else
                {
                    studentSections.Skip(skip).Take(count).ToList().ForEach(x => { x.SectionGroupId = sectionGroups[i]; });
                    skip += count;
                }
            }

            foreach (var studentSection in studentSections)
            {
                var tpmEnrollment = tpmEnrollments.Where(x => x.StudentId == studentSection.StudentId).FirstOrDefault();
                if (tpmEnrollment != null)
                    tpmEnrollment.SectionGroupId = studentSection.SectionGroupId;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<StudentAssistanceReportTemplate>> GetStudentAssistanceReportDataTable(Guid sectionId)
        {
            var sectioonsclasses = await _context.ClassStudents
                .Where(x => x.Class.SectionId == sectionId)
                .Select(x => new { x.Class, classStudents = x })
                .ToListAsync();

            var sectionTerm = await _context.Sections.Where(x => x.Id == sectionId).Select(x => x.CourseTerm.Term).FirstOrDefaultAsync();

            var totalClasses = await _context.Classes.Where(x => x.ClassSchedule.SectionId == sectionId).CountAsync();

            var dataDB = await _context.StudentSections
                .Where(x => x.SectionId == sectionId)
                .Select(x => new
                {
                    student = x.Student.User.FullName,
                    studentUsername = x.Student.User.UserName,
                    studentId = x.StudentId

                }).ToListAsync();

            var data = dataDB
                .OrderBy(x => x.student)
                .Select(x => new StudentAssistanceReportTemplate
                {
                    student = x.student,
                    StudentUsername = x.studentUsername,
                    absences = sectioonsclasses.Where(cs => cs.classStudents.StudentId == x.studentId).Count(cs => cs.classStudents.IsAbsent && cs.Class.StartTime.ToLocalTime() < DateTime.Now),
                    assisted = sectioonsclasses.Where(cs => cs.classStudents.StudentId == x.studentId).Count(cs => !cs.classStudents.IsAbsent && cs.Class.StartTime.ToLocalTime() < DateTime.Now),
                    dictated = sectioonsclasses.Where(cs => cs.classStudents.StudentId == x.studentId).Count(cs => cs.Class.StartTime.ToLocalTime() < DateTime.Now),
                    maxAbsences = (int)Math.Ceiling(totalClasses * (sectionTerm.AbsencePercentage / 100.0))
                })
                .ToList();

            data = data.OrderBy(x => x.student).ToList();

            return new DataTablesStructs.ReturnedData<StudentAssistanceReportTemplate>
            {
                Data = data,
                DrawCounter = 0,
                RecordsFiltered = data.Count,
                RecordsTotal = data.Count
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentGradesReportDataTable(Guid sectionId)
        {
            var section = await _context.Sections.FindAsync(sectionId);

            var students = await _context.StudentSections
                .Where(x => x.SectionId == sectionId)
                .Include(x => x.Student.User)
                .ToListAsync();

            var evaluations = await _context.Evaluations
                .Where(x => x.CourseTermId == section.CourseTermId)
                .ToListAsync();

            var data = await _context.Grades
                .Where(x => x.StudentSection.SectionId == sectionId)
                .ToListAsync();

            //var data = students
            //    .Select(x => new
            //    {
            //        username = x.Student.User.UserName,
            //        student = x.Student.User.FullName,

            //    }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = 0,
                RecordsFiltered = data.Count,
                RecordsTotal = data.Count
            };
        }

        public async Task<StudentGradeReportTemplate> GetGradesByCourses(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, Guid? academicProgramId, Guid? courseId, byte? academicYear = null)
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Id == termId);

            if (term != null && term.Status == ConstantHelpers.TERM_STATES.FINISHED)
            {
                var query = _context.AcademicHistories
                    .Where(x => x.Type != ConstantHelpers.AcademicHistory.Types.CONVALIDATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.REEVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION)
                    .AsNoTracking();

                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Course.Career.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Course.CareerId == careerId);
                if (academicProgramId.HasValue && academicProgramId != Guid.Empty) query = query.Where(x => x.Course.AcademicProgramId == academicProgramId);

                if (curriculumId.HasValue && curriculumId != Guid.Empty)
                {
                    var coursesQry = _context.AcademicYearCourses
                        .Where(x => x.CurriculumId == curriculumId)
                        .AsNoTracking();
                    if (academicYear.HasValue) coursesQry = coursesQry.Where(x => x.AcademicYear == academicYear);
                    var courses = coursesQry.Select(x => x.CourseId).ToHashSet();

                    query = query.Where(x => courses.Contains(x.CourseId));
                }

                if (courseId.HasValue && courseId != Guid.Empty) query = query.Where(x => x.CourseId == courseId);
                if (termId.HasValue && termId != Guid.Empty) query = query.Where(x => x.TermId == termId);

                var data = await query
                    .Select(x => new
                    {
                        Name = x.Student.User.FullName,
                        Grade = x.Grade,
                        SectionCode = x.Section.Code,
                        Course = x.Section.CourseTerm.Course.Name
                    }).ToListAsync();

                var fulldata = data.Count > 0 ?
                    data.GroupBy(x => x.Grade)
                    .Select(x => new FullDataTemplate
                    {
                        Key = x.Key,
                        AbsoluteFrecuency = x.Count()
                    })
                    .OrderBy(x => x.Key)
                    .ToList() : null;

                var cat = fulldata?.Select(x => x.Key).ToList();
                var grades = data.Select(x => x.Grade).ToList();
                var avr = data.Count > 0 ? data.Average(x => x.Grade) : 0;

                var deviation = grades.Count > 0 ? GetStandardDeviation(grades) : 0;
                var median = grades != null && grades.Count > 0 ? Median(grades) : 0;
                var overmedian = grades.Count > 0 ? grades.Where(x => x >= median).Count() * 100 / grades.Count : 0;
                var undermedian = grades.Count > 0 ? grades.Where(x => x < median).Count() * 100 / data.Count : 0;

                var counts = fulldata?.Select(x => x.AbsoluteFrecuency).ToList();

                var datatable = data
                    .OrderByDescending(x => x.Grade)
                    .ThenBy(x => x.Name)
                    .Select(x => new StudentGradeTableTemplate
                    {
                        Name = x.Name,
                        Grade = x.Grade,
                        SectionCode = x.SectionCode,
                        Course = x.Course
                    }).ToList();

                var report = new StudentGradeReportTemplate
                {
                    //series,
                    Datatable = datatable,
                    categories = cat,
                    data = counts,
                    average = avr.ToString("0.00"),//media
                    standarDeviation = deviation.ToString("0.00"),//desviacion estandar
                    median = median.ToString("0.00"),//mediana
                    overmedian = overmedian,
                    undermedian = undermedian,
                    totalstudents = data.Count
                };

                return report;
            }
            else
            {
                var query = _context.StudentSections.Where(x => !x.Section.IsDirectedCourse).AsNoTracking();

                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);
                if (academicProgramId.HasValue && academicProgramId != Guid.Empty) query = query.Where(x => x.Section.CourseTerm.Course.AcademicProgramId == academicProgramId);

                if (curriculumId.HasValue && curriculumId != Guid.Empty)
                {
                    var coursesQry = _context.AcademicYearCourses
                        .Where(x => x.CurriculumId == curriculumId)
                        .AsNoTracking();
                    if (academicYear.HasValue) coursesQry = coursesQry.Where(x => x.AcademicYear == academicYear);
                    var courses = coursesQry.Select(x => x.CourseId).ToHashSet();
                    query = query.Where(x => courses.Contains(x.Section.CourseTerm.CourseId));
                }

                if (courseId.HasValue && courseId != Guid.Empty) query = query.Where(x => x.Section.CourseTerm.CourseId == courseId);
                if (termId.HasValue && termId != Guid.Empty) query = query.Where(x => x.Section.CourseTerm.TermId == termId);

                var data = await query
                    .Select(x => new
                    {
                        Name = x.Student.User.FullName,
                        Grade = x.FinalGrade,
                        SectionCode = x.Section.Code,
                        Course = x.Section.CourseTerm.Course.Name
                    }).ToListAsync();

                var fulldata = data.Count > 0 ?
                    data.GroupBy(x => x.Grade)
                    .Select(x => new FullDataTemplate
                    {
                        Key = x.Key,
                        AbsoluteFrecuency = x.Count()
                    })
                    .OrderBy(x => x.Key)
                    .ToList() : null;

                var cat = fulldata?.Select(x => x.Key).ToList();
                var grades = data.Select(x => x.Grade).ToList();
                var avr = data.Count > 0 ? data.Average(x => x.Grade) : 0;

                var deviation = grades.Count > 0 ? GetStandardDeviation(grades) : 0;
                var median = grades != null && grades.Count > 0 ? Median(grades) : 0;
                var overmedian = grades.Count > 0 ? grades.Where(x => x >= median).Count() * 100 / grades.Count : 0;
                var undermedian = grades.Count > 0 ? grades.Where(x => x < median).Count() * 100 / data.Count : 0;

                var counts = fulldata?.Select(x => x.AbsoluteFrecuency).ToList();

                var datatable = data
                    .OrderByDescending(x => x.Grade)
                    .ThenBy(x => x.Name)
                    .Select(x => new StudentGradeTableTemplate
                    {
                        Name = x.Name,
                        Grade = x.Grade,
                        SectionCode = x.SectionCode,
                        Course = x.Course
                    }).ToList();

                var report = new StudentGradeReportTemplate
                {
                    //series,
                    Datatable = datatable,
                    categories = cat,
                    data = counts,
                    average = avr.ToString("0.00"),//media
                    standarDeviation = deviation.ToString("0.00"),//desviacion estandar
                    median = median.ToString("0.00"),//mediana
                    overmedian = overmedian,
                    undermedian = undermedian,
                    totalstudents = data.Count
                };

                return report;
            }
        }
        private decimal Median(List<int> doubleList)
        {
            var ys = doubleList.OrderBy(x => x).ToList();
            double mid = (ys.Count - 1) / 2.0;
            return (ys[(int)(mid)] + ys[(int)(mid + 0.5)]) / 2;
        }
        private double GetStandardDeviation(List<int> doubleList)
        {
            var average = doubleList.Average();
            var sumOfDerivation = 0;
            foreach (var value in doubleList)
            {
                sumOfDerivation += (value) * (value);
            }
            double sumOfDerivationAverage = 0;
            if (doubleList.Count - 1 > 0 && sumOfDerivation > 0)
            {
                sumOfDerivationAverage = sumOfDerivation / (doubleList.Count - 1);
            }
            return Math.Sqrt(sumOfDerivationAverage - (average * average));
        }
        public async Task<object> GetGradesByCycle(Guid? termId, Guid? facultyId, Guid? careerId, Guid? academicProgramId)
        {
            //agrupar por semestre ??? preguntar a alemaru
            var query = _context.Grades.AsQueryable();

            if (termId.HasValue && termId != Guid.Empty)
                query = query.Where(x => x.Evaluation.CourseTerm.TermId == termId);

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Evaluation.CourseTerm.Course.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.Evaluation.CourseTerm.Course.CareerId == careerId);

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
                query = query.Where(x => x.Evaluation.CourseTerm.Course.AcademicProgramId == academicProgramId);

            var data = await query.Select(x => new
            {
                x.StudentSection.Student.User.FullName,
                x.StudentSection.FinalGrade
            }).ToListAsync();
            return data;
        }
        public async Task<object> GetGradesByYear(Guid? termId, Guid? facultyId, Guid? careerId, Guid? academicProgramId, Guid? courseId)
        {
            //agrupar por año ??? preguntar a alemaru
            var query = _context.StudentSections.Where(x => !x.Section.IsDirectedCourse).AsQueryable();

            if (courseId.HasValue && courseId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.CourseId == courseId);

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.Course.AcademicProgramId == academicProgramId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);

            var term = new Term();
            if (termId.HasValue && termId != Guid.Empty)
            {
                term = await _context.Terms.FirstOrDefaultAsync(x => x.Id == termId);
                query = query.Where(x => x.Section.CourseTerm.Term.Year == term.Year);
            }

            var data = await query.Select(x => new GradesReportTemplate
            {
                Name = x.Student.User.FullName ?? "",
                Grade = x.FinalGrade,
            })
                       .ToListAsync();

            var cat = data?.Select(x => x.Name).ToList();
            var dat = data?.Select(x => x.Grade).ToList();
            var avr = data.Count > 0 ? data.Average(x => x.Grade) : 0;
            var deviation = dat != null && dat.Count > 0 ? GetStandardDeviation(dat) : 0;
            var median = dat != null && dat.Count > 0 ? Median(dat) : 0;
            var report = new
            {
                categories = cat,
                data = dat,
                average = avr.ToString("0.00"),//media
                standarDeviation = deviation.ToString("0.00"),//desviacion estandar
                median = median.ToString("0.00"),
                year = term.Year
            };

            return data;
        }


        public async Task<object> GetStudentsWithParallelCoursesDatatable(Guid termId, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            //Expression<Func<Student, dynamic>> orderByPredicate = null;
            //switch (sentParameters.OrderColumn)
            //{
            //    case "0":
            //        orderByPredicate = ((x) => x.User.UserName); break;
            //    case "1":
            //        orderByPredicate = ((x) => x.User.FullName); break;
            //    case "2":
            //        orderByPredicate = ((x) => x.Career.Name); break;
            //    case "3":
            //        orderByPredicate = ((x) => x.CurrentAcademicYear); break;
            //    default:
            //        break;
            //}

            var qryCourses = _context.AcademicYearCourses.Where(x => x.PreRequisites.Any()).AsNoTracking();
            var qryStudents = _context.Students
                .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId && !y.Section.IsDirectedCourse))
                .AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    qryStudents = qryStudents.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId || x.Career.Faculty.AdministrativeAssistantId == userId);
                }

            }

            if (facultyId.HasValue && facultyId.Value != Guid.Empty)
            {
                qryCourses = qryCourses.Where(x => x.Curriculum.Career.FacultyId == facultyId);
                qryStudents = qryStudents.Where(x => x.Career.FacultyId == facultyId);
            }

            if (careerId.HasValue && careerId.Value != Guid.Empty)
            {
                qryCourses = qryCourses.Where(x => x.Curriculum.CareerId == careerId);
                qryStudents = qryStudents.Where(x => x.CareerId == careerId);
            }

            var academicYearCourses = await qryCourses
                .Select(x => new
                {
                    x.CourseId,
                    PreRequisites = x.PreRequisites
                    .Select(x => x.CourseId)
                    .ToList()
                }).ToListAsync();

            var students = await qryStudents
                .Select(x => new
                {
                    x.Id,
                    x.User.UserName,
                    x.User.FullName,
                    x.CurrentAcademicYear,
                    Career = x.Career.Name,
                    Sections = x.StudentSections
                    .Where(y => y.Section.CourseTerm.TermId == termId && !y.Section.IsDirectedCourse)
                    .Select(y => y.Section.CourseTerm.CourseId)
                    .ToList()
                }).ToListAsync();

            var data = students
                .Where(x => academicYearCourses.Any(y => x.Sections.Contains(y.CourseId) && y.PreRequisites.Any(z => x.Sections.Contains(z))))
                .Select(x => new
                {
                    id = x.Id,
                    code = x.UserName,
                    name = x.FullName,
                    year = x.CurrentAcademicYear,
                    career = x.Career
                })
                .ToList();

            //var recordsFiltered = await query.CountAsync();

            //var data = students
            //    .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
            //    .Skip(sentParameters.PagingFirstRecord)
            //    .Take(sentParameters.RecordsPerDraw)

            //    .Select(x => new { 
            //        x.Id,
            //        x.UserName,
            //        x.FullName,
            //        x.Career,
            //        x.CurrentAcademicYear
            //    })
            //    .ToList();

            //int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = 0,
                RecordsFiltered = data.Count,
                RecordsTotal = data.Count
            };
        }

        public async Task<object> GetStudentParallelCoursesDatatable(Guid termId, Guid studentId)
        {
            var student = await _context.Students.FindAsync(studentId);

            var academicYearCourses = await _context.AcademicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId)
                .Select(x => new
                {
                    x.CourseId,
                    x.AcademicYear,
                    PreRequisites = x.PreRequisites
                   .Select(x => x.CourseId)
                   .ToList()
                }).ToListAsync();

            var studentSections = await _context.StudentSections
                .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termId && !x.Section.IsDirectedCourse)
                .Select(x => new
                {
                    Section = x.Section.Code,
                    CourseCode = x.Section.CourseTerm.Course.Code,
                    Course = x.Section.CourseTerm.Course.Name,
                    x.Section.CourseTerm.CourseId,
                }).ToListAsync();

            var data = studentSections
                .Where(x => academicYearCourses.Any(y => (y.CourseId == x.CourseId && studentSections.Any(z => y.PreRequisites.Contains(z.CourseId)))
                || (y.PreRequisites.Contains(x.CourseId) && studentSections.Any(z => z.CourseId == y.CourseId))))
                .Select(x => new
                {
                    section = x.Section,
                    course = x.Course,
                    code = x.CourseCode,
                    year = academicYearCourses.FirstOrDefault(y => y.CourseId == x.CourseId).AcademicYear
                })
                .OrderBy(x => x.year)
                .ThenBy(x => x.code)
                .ToList();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = 0,
                RecordsFiltered = data.Count,
                RecordsTotal = data.Count
            };
        }
        public async Task<List<StudentSection>> GetByCareerIdAndTermId(Guid careerId, Guid termId)
            => await _context.StudentSections
                .Where(x => x.Student.CareerId == careerId && x.Section.CourseTerm.TermId == termId && !x.Section.IsDirectedCourse)
            .Include(x => x.Student)
            .ThenInclude(x => x.User)
            .Include(x => x.Section)
            .ThenInclude(x => x.CourseTerm)
            .ThenInclude(x => x.Course)
            .ToListAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetIrregularStudents(DataTablesStructs.SentParameters sentParameters, int? time = null, string search = null, Guid? career = null, ClaimsPrincipal user = null, int? academicyear = null)
        {
            Expression<Func<StudentSection, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Student.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.Student.User.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.Try); break;
                case "3":
                    orderByPredicate = ((x) => x.Student.Curriculum.Code); break;
                case "4":
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.Name); break;
                case "5":
                    orderByPredicate = ((x) => x.Section.Code); break;
                case "6":
                    orderByPredicate = ((x) => x.Student.User.PhoneNumber); break;
                case "7":
                    orderByPredicate = ((x) => x.Student.User.Email); break;
                case "8":
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.Career.Name); break;
                default:
                    orderByPredicate = ((x) => x.Student.User.FullName); break;
            }

            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            var query = _context.StudentSections.Where(x => x.Section.CourseTerm.TermId == term.Id && x.Try >= 2).AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.TUTORING_COORDINATOR))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    //var teacher = _context.Teachers.Where(x => x.UserId == userId).FirstOrDefault();

                    //query = query = query.Where(x => x.Section.CourseTerm.Course.CareerId == teacher.CareerId);


                    var coordinatorCareerId = await _context.TutoringCoordinators
                        .Where(x => x.UserId == userId)
                        .Select(x => x.CareerId)
                        .ToListAsync();

                    query = query = query.Where(x => coordinatorCareerId.Contains(x.Section.CourseTerm.Course.CareerId.Value));
                }
            }

            if (time.HasValue && time != 0)
            {
                if (time == 5)
                    query = query.Where(x => x.Try >= time);
                else
                    query = query.Where(x => x.Try == time);
            }

            if (career.HasValue)
                query = query.Where(x => x.Section.CourseTerm.Course.CareerId == career);

            if (academicyear.HasValue && academicyear != 0)
                query = query.Where(x => x.Student.CurrentAcademicYear == academicyear);


            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.FullName.ToLower().Contains(search.Trim().ToLower())
                    || x.Student.User.UserName.ToUpper().Contains(search.Trim()));

            int recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Student.User.UserName,
                    name = x.Student.User.FullName,
                    time = ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES[x.Try],
                    plan = x.Student.Curriculum.Code,
                    course = x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code,
                    phone = x.Student.User.PhoneNumber,
                    email = x.Student.User.Email,
                    career = x.Section.CourseTerm.Course.Career.Name
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data.OrderBy(x => x.career).ThenBy(x => x.time),
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<StudentSectionIrregularTutoringReportTemplate>> GetIrregularStudentsExport(int? time = null, string search = null, Guid? career = null, ClaimsPrincipal user = null, int? academicyear = null)
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            var query = _context.StudentSections.Where(x => x.Section.CourseTerm.TermId == term.Id && x.Try >= 2).AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.TUTORING_COORDINATOR))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    //var teacher = _context.Teachers.Where(x => x.UserId == userId).FirstOrDefault();

                    //query = query = query.Where(x => x.Section.CourseTerm.Course.CareerId == teacher.CareerId);

                    var coordinatorCareerId = await _context.TutoringCoordinators
                        .Where(x => x.UserId == userId)
                        .Select(x => x.CareerId)
                        .ToListAsync();

                    query = query = query.Where(x => coordinatorCareerId.Contains(x.Section.CourseTerm.Course.CareerId.Value));
                }
            }

            if (time.HasValue && time != 0)
            {
                if (time == 5)
                    query = query.Where(x => x.Try >= time);
                else
                    query = query.Where(x => x.Try == time);
            }


            if (career.HasValue)
                query = query.Where(x => x.Section.CourseTerm.Course.CareerId == career);

            if (academicyear.HasValue && academicyear != 0)
                query = query.Where(x => x.Student.CurrentAcademicYear == academicyear);


            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.FullName.ToLower().Contains(search.Trim().ToLower()));

            var data = await query
                .Select(x => new StudentSectionIrregularTutoringReportTemplate
                {
                    StudentSectionId = x.Id,
                    UserName = x.Student.User.UserName,
                    PaternalSurname = x.Student.User.PaternalSurname,
                    MaternalSurname = x.Student.User.MaternalSurname,
                    Name = x.Student.User.Name,
                    FullName = x.Student.User.FullName,
                    PhoneNumber = x.Student.User.PhoneNumber,
                    Email = x.Student.User.Email,
                    Sex = x.Student.User.Sex,
                    Dni = x.Student.User.Dni,
                    StudentCurriculumCode = x.Student.Curriculum.Code,
                    StudentCareerCode = x.Student.Career.Code,
                    StudentCareerName = x.Student.Career.Name,
                    StudentFacultyName = x.Student.Career.Faculty.Name,
                    SectionCode = x.Section.Code,
                    CourseCode = x.Section.CourseTerm.Course.Code,
                    CourseName = x.Section.CourseTerm.Course.Name,
                    CourseCareerName = x.Section.CourseTerm.Course.Career.Name,
                    AcademicProgram = x.Section.CourseTerm.Course.AcademicProgram.Name,
                    AcademicProgramCareerName = x.Section.CourseTerm.Course.AcademicProgram.Career.Name,
                    CurrentAcademicYear = x.Student.CurrentAcademicYear,
                    Try = x.Try,
                })
                .ToListAsync();

            return data;
        }

        public async Task<int> CountDirectedCourseAttempts(Guid studentId, Guid courseId)
        {
            var count = await _context.StudentSections
                .Where(x => x.StudentId == studentId && x.Section.CourseTerm.CourseId == courseId && x.Section.IsDirectedCourse)
                .CountAsync();

            return count;
        }

        public async Task<object> GetDirectedCourseStudentsDatatableClientSide(Guid courseId, Guid termId)
        {
            var result = await _context.StudentSections
                 .Where(x => x.Section.CourseTerm.CourseId == courseId && x.Section.CourseTerm.TermId == termId && x.Section.IsDirectedCourse)
                 .Select(x => new
                 {
                     id = x.Id,
                     code = x.Student.User.UserName,
                     name = x.Student.User.FullName,
                     program = x.Student.AcademicProgramId.HasValue ? x.Student.AcademicProgram.Name : "---",
                     academicYear = x.Student.CurrentAcademicYear
                 })
                    .OrderBy(x => x.name)
                    .ToListAsync();


            return result;
        }

        public async Task<EnrollmentDirectedCourseDataTemplate> GetEnrollmentDirectedCourseData(Guid termId, Guid careerId, Guid curriculumId)
        {
            var academicyears = await _context.AcademicYearCourses.Where(x => (x.Course.CareerId.HasValue) ? x.Course.CareerId.Value == careerId : false).ToListAsync();

            var sqlData = _context.StudentSections
                .Where(y => y.Section.IsDirectedCourse && y.Student.CurriculumId == curriculumId && y.Section.CourseTerm.TermId == termId && y.Section.CourseTerm.Course.CareerId.Value == careerId)
                .Include(y => y.Section.CourseTerm.Course)
                .ToList();

            var courses = sqlData
                .GroupBy(y => new { y.Section.CourseTerm.Course })
                .Select(x => new DirectedCourseDataTemplate
                {
                    AcademicYear = academicyears.FirstOrDefault(a => a.CourseId == x.Key.Course.Id) != null ? academicyears.FirstOrDefault(a => a.CourseId == x.Key.Course.Id).AcademicYear : Convert.ToByte(0),
                    CourseName = x.Key.Course.Name,
                    CourseCode = x.Key.Course.Code,
                    StudentDirectedCount = x.Count()
                })
                .ToList();

            var curriculum = await _context.Curriculums.Where(x => x.Id == curriculumId).FirstOrDefaultAsync();

            return new EnrollmentDirectedCourseDataTemplate { StudentEnrollmentDirectedCourse = courses, CurriculumCode = curriculum.Code };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDirectedCourseStudentsDataDatatable(DataTablesStructs.SentParameters sentParameters,
            Guid? termId = null, Guid? careerId = null, Guid? facultyId = null, Guid? courseId = null, string searchValue = null, ClaimsPrincipal user = null)
        {
            Expression<Func<StudentSection, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Section.CourseTerm.Term.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Section.CourseTerm.Course.Code;
                    break;
                default:
                    //orderByPredicate = (x) => x.CreatedAt;
                    break;
            }

            var query = _context.StudentSections
                .Where(x => x.Section.IsDirectedCourse)
                .AsNoTracking();

            if (user != null)
            {
                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (!string.IsNullOrEmpty(userId))
                    {
                        query = query.Where(x => x.Student.Career.Faculty.DeanId == userId || x.Student.Career.Faculty.SecretaryId == userId);
                    }
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (!string.IsNullOrEmpty(userId))
                    {
                        var qryCareers = _context.Careers
                            .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                            .AsNoTracking();

                        var careers = qryCareers.Select(x => x.Id).ToHashSet();

                        query = query.Where(x => careers.Contains(x.Student.CareerId));
                    }
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.TEACHERS))
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (!string.IsNullOrEmpty(userId))
                        query = query.Where(x => x.Section.TeacherSections.Any(x => x.TeacherId == userId));
                }
            }

            if (termId.HasValue && termId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId);
            else
            {
                var term = await _context.Terms.Where(x => x.Status != ConstantHelpers.TERM_STATES.INACTIVE).OrderBy(x => x.Status).FirstOrDefaultAsync();
                if (term != null) query = query.Where(x => x.Section.CourseTerm.TermId == term.Id);
            }

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Student.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.Student.CareerId == careerId);

            if (courseId.HasValue && courseId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.CourseId == courseId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x =>
                x.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()) ||
                x.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper()));
            }

            //return query.OrderBy(x => x.Section.CourseTerm.Course.Career.Name).ThenBy(x => x.Student.User.PaternalSurname);

            var recordsFiltered = await query.CountAsync();

            var dbdata = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    Id = x.Id,
                    Teacher = x.Section.TeacherSections.Select(x => x.Teacher.User.FullName).ToList(),
                    CareerId = x.Student.CareerId,
                    FacultyId = x.Student.Career.FacultyId,
                    Career = x.Student.Career.Name,
                    Faculty = x.Student.Career.Faculty.Name,
                    Term = x.Section.CourseTerm.Term.Name,
                    Course = x.Section.CourseTerm.Course.FullName,
                    Code = x.Student.User.UserName,
                    Name = x.Student.User.FullName,
                    Status = x.Status,
                    Grade = x.FinalGrade,
                    Courseid = x.Section.CourseTerm.CourseId,
                    Date = x.CreatedAt.ToLocalDateFormat()
                })
                .ToListAsync();

            var data = dbdata
                .Select(x => new
                {
                    x.Id,
                    Teacher = string.Join(", ", x.Teacher),
                    x.CareerId,
                    x.FacultyId,
                    x.Career,
                    x.Faculty,
                    x.Term,
                    x.Course,
                    x.Code,
                    x.Name,
                    x.Status,
                    x.Grade,
                    x.Courseid,
                    x.Date
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

        public async Task<IEnumerable<Student>> GetStudentsByCourseTermId(Guid courseTermId)
            => await _context.StudentSections.Where(x => x.Section.CourseTermId == courseTermId).Select(x => x.Student).Include(x => x.User).ToListAsync();

        public async Task DeleteRangeWithData(List<StudentSection> studentSections)
        {
            var studentSectionIds = studentSections.Select(x => x.Id).ToHashSet();

            var otherQualifications = await _context.OtherQualificationStudents
                .Where(x => studentSectionIds.Contains(x.StudentSectionId))
                .ToListAsync();
            _context.OtherQualificationStudents.RemoveRange(otherQualifications);

            var temporalGrades = await _context.TemporalGrades
            .Where(x => studentSectionIds.Contains(x.StudentSectionId))
            .ToListAsync();
            _context.TemporalGrades.RemoveRange(temporalGrades);

            var grades = await _context.Grades
                .Where(x => studentSectionIds.Contains(x.StudentSectionId))
                .ToListAsync();
            _context.Grades.RemoveRange(grades);


            _context.StudentSections.RemoveRange(studentSections);
            await _context.SaveChangesAsync();
        }

        public async Task RecalculateFinalGrade(Guid studentSectionId)
        {
            var evaluationsByUnit = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT));
            var evaluationsBySection = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_SECTION));
            var studentSection = await _context.StudentSections.Where(x => x.Id == studentSectionId).FirstOrDefaultAsync();

            if (studentSection.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN) return;

            var section = await _context.Sections
                  .Where(x => x.Id == studentSection.SectionId)
                  .Select(x => new
                  {
                      x.Id,
                      x.CourseTerm.TermId,
                      x.CourseTerm.CourseId,
                      x.CourseTermId
                  }).FirstOrDefaultAsync();

            var finalGrade = 0M;
            var grades = await _context.Grades
                .Where(x => x.StudentSectionId == studentSection.Id)
                .Select(x => new
                {
                    x.EvaluationId,
                    x.Value,
                    Percentage = evaluationsBySection ? _context.SectionEvaluations.Where(z => z.EvaluationId == x.EvaluationId && z.SectionId == section.Id).Select(z => z.Percentage).FirstOrDefault() : x.Evaluation.Percentage
                })
                .ToListAsync();

            if (evaluationsByUnit)
            {
                var courseUnits = await _context.CourseUnits
                    .Where(x => x.CourseSyllabus.TermId == section.TermId && x.CourseSyllabus.CourseId == section.CourseId)
                    .OrderBy(x => x.WeekNumberStart)
                    .Select(x => new
                    {
                        x.AcademicProgressPercentage,
                        Evaluations =
                            x.Evaluations
                            .Select(y => new
                            {
                                y.Id,
                                Percentage = evaluationsBySection ? _context.SectionEvaluations.Where(z => z.EvaluationId == y.Id && z.SectionId == section.Id).Select(z => z.Percentage).FirstOrDefault() : y.Percentage
                            }).ToList()
                    }).ToListAsync();

                var areEquivalent = courseUnits.All(x => x.AcademicProgressPercentage == 0);
                var unitsPorcentage = courseUnits.Sum(x => x.AcademicProgressPercentage) * 1.0M;

                foreach (var courseUnit in courseUnits)
                {
                    var evaluationCalc = 0M;
                    var evaluationTotalPercentage = courseUnit.Evaluations.Sum(x => x.Percentage) * 1.0M;

                    if (evaluationTotalPercentage == 0) evaluationTotalPercentage = 1.0M;

                    foreach (var evaluation in courseUnit.Evaluations)
                    {
                        var gradeByEvaluation = grades.Where(x => x.EvaluationId == evaluation.Id).Select(x => x.Value).FirstOrDefault();
                        evaluationCalc += gradeByEvaluation * evaluation.Percentage / evaluationTotalPercentage;
                    }

                    if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAJMA)
                        evaluationCalc = Math.Round(evaluationCalc, 0, MidpointRounding.AwayFromZero);

                    if (areEquivalent) finalGrade += evaluationCalc / courseUnits.Count();
                    else finalGrade += evaluationCalc * courseUnit.AcademicProgressPercentage / unitsPorcentage;
                }
            }
            else
            {
                var totalPercentage = 0M;

                if (evaluationsBySection)
                {
                    totalPercentage = await _context.SectionEvaluations
                        .Where(x => x.SectionId == section.Id)
                        .SumAsync(x => x.Percentage);
                }
                else
                {
                    totalPercentage = await _context.Evaluations
                        .Where(x => x.CourseTerm.TermId == section.TermId && x.CourseTerm.CourseId == section.CourseId)
                        .SumAsync(x => x.Percentage);
                }

                if (totalPercentage == 0)
                    totalPercentage = 1;

                var gradesSum = grades.Sum(x => x.Value * x.Percentage);
                finalGrade = gradesSum / totalPercentage;
            }

            studentSection.FinalGrade = (int)Math.Round(finalGrade, 0, MidpointRounding.AwayFromZero);

            if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UIGV)
            {
                var substituteExam = await _context.SubstituteExams
                    .Where(x => x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED
                    && x.StudentId == studentSection.StudentId && x.CourseTermId == section.CourseTermId)
                    .FirstOrDefaultAsync();

                if (substituteExam != null)
                    studentSection.FinalGrade = studentSection.FinalGrade > substituteExam.ExamScore ? studentSection.FinalGrade : substituteExam.ExamScore.Value;
            }

            if (studentSection.Status != ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS)
            {
                var term = await _context.Terms.FindAsync(section.TermId);

                if (studentSection.Status != ConstantHelpers.STUDENT_SECTION_STATES.DPI)
                {
                    studentSection.Status = studentSection.FinalGrade >= term.MinGrade ? ConstantHelpers.STUDENT_SECTION_STATES.APPROVED
                      : ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED;
                }

                if (term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                {
                    var academicHistory = await _context.AcademicHistories.FirstOrDefaultAsync(x => x.StudentId == studentSection.StudentId && x.SectionId == studentSection.SectionId && x.TermId == term.Id);
                    if (academicHistory != null)
                    {
                        academicHistory.Grade = studentSection.FinalGrade;
                        academicHistory.Approved = studentSection.Status != ConstantHelpers.STUDENT_SECTION_STATES.DPI && studentSection.FinalGrade >= term.MinGrade;
                    }
                }
            }



            await _context.SaveChangesAsync();
        }

        public async Task RecalculateFinalGradeByCourseTerm(Guid courseTermId)
        {
            var courseTerm = await _context.CourseTerms.Where(x => x.Id == courseTermId).FirstOrDefaultAsync();
            var studentSections = await _context.StudentSections.Where(x => x.Section.CourseTermId == courseTermId).ToListAsync();

            foreach (var studentSection in studentSections)
            {
                await RecalculateFinalGrade(studentSection.Id);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RecalculateFinalGradeBySectionId(Guid sectionId)
        {
            var studentSections = await _context.StudentSections.Where(x => x.SectionId == sectionId).ToListAsync();

            foreach (var studentSection in studentSections)
            {
                await RecalculateFinalGrade(studentSection.Id);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RecalculateFinalGradeByCourseSyllabusId(Guid courseSyllabusId)
        {
            var courseSyllabyus = await _context.CourseSyllabus.Where(x => x.Id == courseSyllabusId).FirstOrDefaultAsync();
            var courseTerm = await _context.CourseTerms.Where(x => x.CourseId == courseSyllabyus.CourseId && x.TermId == courseSyllabyus.TermId).FirstOrDefaultAsync();
            if (courseTerm != null)
            {
                var studentSections = await _context.StudentSections.Where(x => x.Section.CourseTermId == courseTerm.Id).ToListAsync();
                var courseUnits = await _context.CourseUnits
                    .Include(x => x.Evaluations)
                    .OrderBy(x => x.WeekNumberStart)
                    .Where(x => x.CourseSyllabusId == courseSyllabusId).ToListAsync();

                var configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT).FirstOrDefaultAsync();
                var evaluationByUnits = Convert.ToBoolean(configuration?.Value ?? "true");

                foreach (var studentSection in studentSections)
                {
                    await RecalculateFinalGrade(studentSection.Id);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<object> GetSectionStudentsSelect2(Guid sectionId)
        {
            var result = await _context.StudentSections
                .Where(x => x.SectionId == sectionId)
                .OrderBy(x => x.Student.User.FullName)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Student.User.FullName
                })
                .ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetFinalGradeReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid sectionId)
        {
            var query = _context.StudentSections
               .Where(x => x.SectionId == sectionId)
               .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    student = x.Student.User.FullName,
                    finalGrade = x.Status == ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS ? "En proceso" : x.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN ? "Retirado" : $"{x.FinalGrade}"
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

        public async Task<bool> HasStudentSections(Guid studentId, Guid termId)
        {
            return await _context.StudentSections.AnyAsync(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termId);
        }

        public async Task<object> GetGradesByCompetences(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, Guid? competenceId)
        {

            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();

            if (term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
            {
                var query = _context.Competencies.AsQueryable();

                if (competenceId.HasValue && competenceId != Guid.Empty)
                {
                    query = query.Where(x => x.Id == competenceId);
                }

                var competences = await query.ToListAsync();
                var result = new List<DataReport>();

                foreach (var item in competences)
                {
                    var queryStudentSections = _context.StudentSections.Where(x => x.Section.CourseTerm.TermId == term.Id && x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CompetencieId == item.Id)).AsQueryable();

                    if (facultyId.HasValue && facultyId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);
                    }

                    if (careerId.HasValue && careerId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);
                    }
                    if (curriculumId.HasValue && curriculumId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CurriculumId == curriculumId));
                    }

                    var objData = await queryStudentSections
                                   .GroupBy(x => new { x.Section.CourseTerm.Course.Id, x.Section.CourseTerm.Course.Name, x.Section.CourseTerm.Course.Credits, x.SectionId, x.Section.Code }).Select(x => new RowChild
                                   {
                                       CourseSection = x.Key.Name + " " + x.Key.Code,
                                       CourseName = x.Key.Name,
                                       Average = x.Average(x => (decimal)x.FinalGrade),
                                       CourseId = x.Key.Id,
                                       SectionId = x.Key.SectionId,
                                       Credits = x.Key.Credits
                                   })
                                   .ToListAsync();

                    if (objData != null && objData.Count() > 0)
                    {
                        var objReview = objData.GroupBy(x => new { x.CourseId, x.CourseName, x.Credits }).Select(x => new RowChild
                        {
                            CourseId = x.Key.CourseId,
                            Average = Math.Round(x.Average(x => x.Average), 2),
                            Credits = x.Key.Credits,
                            CourseName = x.Key.CourseName

                        }).ToList();

                        var sumCredits = objReview.Sum(x => x.Credits);
                        decimal finalResult = 0m;
                        foreach (var i in objReview)
                        {
                            finalResult += (i.Credits * i.Average);
                        }
                        result.Add(new DataReport
                        {
                            CompetenceName = item.Name,
                            FinalResult = Math.Round((finalResult / sumCredits), 2),
                            RowChilds = objReview
                        });
                    }
                    else
                    {
                        result.Add(new DataReport
                        {
                            CompetenceName = item.Name,
                            FinalResult = 0,
                            RowChilds = objData
                        });
                    }


                }
                return result;
            }
            else
            {
                var query = _context.Competencies.AsQueryable();

                if (competenceId.HasValue && competenceId != Guid.Empty)
                {
                    query = query.Where(x => x.Id == competenceId);
                }

                var competences = await query.ToListAsync();
                var result = new List<DataReport>();

                foreach (var item in competences)
                {
                    var queryStudentSections = _context.AcademicHistories.Where(x => x.TermId == term.Id && x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CompetencieId == item.Id)).AsQueryable();

                    if (facultyId.HasValue && facultyId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);
                    }

                    if (careerId.HasValue && careerId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);
                    }
                    if (curriculumId.HasValue && curriculumId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CurriculumId == curriculumId));
                    }

                    var objData = await queryStudentSections
                                    .GroupBy(x => new { x.Section.CourseTerm.Course.Id, x.Section.CourseTerm.Course.Name, x.Section.CourseTerm.Course.Credits, x.SectionId, x.Section.Code }).Select(x => new RowChild
                                    {
                                        CourseSection = x.Key.Name + " " + x.Key.Code,
                                        CourseName = x.Key.Name,
                                        Average = x.Average(x => (decimal)x.Grade),
                                        CourseId = x.Key.Id,
                                        SectionId = x.Key.SectionId.Value,
                                        Credits = x.Key.Credits
                                    })
                                    .ToListAsync();
                    if (objData != null && objData.Count() > 0)
                    {
                        var objReview = objData.GroupBy(x => new { x.CourseId, x.CourseName, x.Credits }).Select(x => new RowChild
                        {
                            CourseId = x.Key.CourseId,
                            Average = Math.Round(x.Average(x => x.Average), 2),
                            Credits = x.Key.Credits,
                            CourseName = x.Key.CourseName

                        }).ToList();

                        var sumCredits = objReview.Sum(x => x.Credits);
                        decimal finalResult = 0m;
                        foreach (var i in objReview)
                        {
                            finalResult += (i.Credits * i.Average);
                        }
                        result.Add(new DataReport
                        {
                            CompetenceName = item.Name,
                            FinalResult = Math.Round((finalResult / sumCredits), 2),
                            RowChilds = objReview
                        });
                    }
                    else
                    {
                        result.Add(new DataReport
                        {
                            CompetenceName = item.Name,
                            FinalResult = 0,
                            RowChilds = objData
                        });
                    }

                }
                return result;

            }

        }

        public async Task<List<DataReport2>> AchievementLevelCompetences(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();

            if (term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
            {
                var query = _context.Competencies.AsQueryable();

                var competences = await query.ToListAsync();
                var result = new List<DataReport>();
                var result2 = new List<DataReport2>();

                foreach (var item in competences)
                {
                    var RangeLevelList = new List<RangeLevel>();
                    RangeLevelList.Add(new RangeLevel
                    {
                        Name = "Deficiente",
                        Min = 0,
                        Max = 10,
                        Total = 0,
                        Type = 1
                    });
                    RangeLevelList.Add(new RangeLevel
                    {
                        Name = "Regular",
                        Min = 11,
                        Max = 13,
                        Total = 0,
                        Type = 2
                    });
                    RangeLevelList.Add(new RangeLevel
                    {
                        Name = "Bueno",
                        Min = 14,
                        Max = 16,
                        Total = 0,
                        Type = 3
                    });
                    RangeLevelList.Add(new RangeLevel
                    {
                        Name = "Excelente",
                        Min = 17,
                        Max = 20,
                        Total = 0,
                        Type = 4
                    });

                    var queryStudentSections = _context.StudentSections.Where(x => x.Section.CourseTerm.TermId == term.Id && x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CompetencieId == item.Id)).AsQueryable();

                    if (facultyId.HasValue && facultyId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);
                    }

                    if (careerId.HasValue && careerId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);
                    }
                    if (curriculumId.HasValue && curriculumId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CurriculumId == curriculumId));
                    }

                    var objData = await queryStudentSections
                            .GroupBy(x => new { x.Section.CourseTerm.Course.Id, x.Section.CourseTerm.Course.Name, x.Section.CourseTerm.Course.Credits, x.StudentId, x.Student.User.FullName }).Select(x => new RowChild2
                            {
                                //CompetenceName = item.Name,
                                //StudentFullName = x.Key.FullName,
                                //CourseName = x.Key.Name,
                                StudentId = x.Key.StudentId,
                                Average = x.Sum(z => (decimal)z.FinalGrade) * x.Key.Credits,
                                Credits = x.Key.Credits

                            }).ToListAsync();

                    if (objData != null && objData.Count > 0)
                    {
                        var objDataFinal = objData.GroupBy(x => new { x.StudentId }).Select(x => new RowChild2
                        {
                            Average = Math.Round(x.Sum(s => s.Average) / x.Sum(s => s.Credits), 0)

                        }).ToList();
                        foreach (var i in RangeLevelList)
                        {

                            foreach (var j in objDataFinal)
                            {
                                if (i.Min <= j.Average && j.Average <= i.Max)
                                {
                                    i.Total++;
                                }
                            }
                        }
                    }


                    result2.Add(new DataReport2
                    {
                        CompetenceName = item.Name,
                        CompetenceId = item.Id,
                        RangeLevels = RangeLevelList
                    });



                }
                return result2;
            }
            else
            {
                var query = _context.Competencies.AsQueryable();

                var competences = await query.ToListAsync();
                var result = new List<DataReport>();
                var result2 = new List<DataReport2>();


                foreach (var item in competences)
                {

                    var RangeLevelList = new List<RangeLevel>();
                    RangeLevelList.Add(new RangeLevel
                    {
                        Name = "Deficiente",
                        Min = 0,
                        Max = 10,
                        Total = 0,
                        Type = 1
                    });
                    RangeLevelList.Add(new RangeLevel
                    {
                        Name = "Regular",
                        Min = 11,
                        Max = 13,
                        Total = 0,
                        Type = 2
                    });
                    RangeLevelList.Add(new RangeLevel
                    {
                        Name = "Bueno",
                        Min = 14,
                        Max = 16,
                        Total = 0,
                        Type = 3
                    });
                    RangeLevelList.Add(new RangeLevel
                    {
                        Name = "Excelente",
                        Min = 17,
                        Max = 20,
                        Total = 0,
                        Type = 4
                    });
                    var queryStudentSections = _context.AcademicHistories.Where(x => x.TermId == term.Id && x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CompetencieId == item.Id)).AsQueryable();

                    if (facultyId.HasValue && facultyId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);
                    }

                    if (careerId.HasValue && careerId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);
                    }
                    if (curriculumId.HasValue && curriculumId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CurriculumId == curriculumId));
                    }

                    var objData = await queryStudentSections
                                     .GroupBy(x => new { x.Section.CourseTerm.Course.Id, x.Section.CourseTerm.Course.Name, x.Section.CourseTerm.Course.Credits, x.StudentId, x.Student.User.FullName, x.Student.User.UserName }).Select(x => new RowChild2
                                     {
                                         //CompetenceName = item.Name,
                                         //StudentFullName = x.Key.FullName,
                                         //CourseName = x.Key.Name,
                                         StudentId = x.Key.StudentId,
                                         Average = x.Sum(z => (decimal)z.Grade) * x.Key.Credits,
                                         Credits = x.Key.Credits

                                     }).ToListAsync();



                    if (objData != null && objData.Count > 0)
                    {
                        var objDataFinal = objData.GroupBy(x => new { x.StudentId }).Select(x => new RowChild2
                        {
                            Average = Math.Round(x.Sum(s => s.Average) / x.Sum(s => s.Credits), 0)

                        }).ToList();


                        foreach (var i in RangeLevelList)
                        {
                            foreach (var j in objDataFinal)
                            {
                                if (i.Min <= j.Average && j.Average <= i.Max)
                                {
                                    i.Total++;
                                }
                            }
                        }
                    }



                    result2.Add(new DataReport2
                    {
                        CompetenceName = item.Name,
                        CompetenceId = item.Id,
                        RangeLevels = RangeLevelList
                    });

                }
                return result2;

            }
        }

        public async Task<object> AchievementLevelCompetenceDetail(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, Guid? competenceId, List<RangeLevel> rangeLevelList)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();
            Competencie competence = null;
            if (competenceId.HasValue && competenceId != Guid.Empty)
            {
                competence = await _context.Competencies.Where(x => x.Id == competenceId).FirstOrDefaultAsync();
            }
            var typeRange = rangeLevelList.FirstOrDefault().Name;
            var minAvg = rangeLevelList.FirstOrDefault().Min;
            var maxAvg = rangeLevelList.FirstOrDefault().Max;
            var typeRangeList = rangeLevelList.FirstOrDefault().Type;

            if (term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
            {

                var queryStudentSections = _context.StudentSections.Where(x => x.Section.CourseTerm.TermId == term.Id && x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CompetencieId == competenceId)).AsQueryable();

                if (facultyId.HasValue && facultyId != Guid.Empty)
                {
                    queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);
                }

                if (careerId.HasValue && careerId != Guid.Empty)
                {
                    queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);
                }
                if (curriculumId.HasValue && curriculumId != Guid.Empty)
                {
                    queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CurriculumId == curriculumId));
                }

                var objData = await queryStudentSections
                        .GroupBy(x => new { x.Section.CourseTerm.Course.Id, x.Section.CourseTerm.Course.Name, x.Section.CourseTerm.Course.Credits, x.StudentId, x.Student.User.FullName, x.Student.User.UserName }).Select(x => new RowChild3
                        {
                            CompetenceName = $"{competence.Name} - {typeRange}",
                            StudentFullName = x.Key.FullName,
                            UserName = x.Key.UserName,
                            StudentId = x.Key.StudentId,
                            Average = x.Sum(z => (decimal)z.FinalGrade) * x.Key.Credits,
                            CompetenceId = competence.Id,
                            Credits = x.Key.Credits,
                            Type = typeRangeList

                        }).ToListAsync();

                if (objData != null && objData.Count() > 0)
                {
                    var objDataFinal = objData.GroupBy(x => new { x.StudentId, x.UserName, x.StudentFullName }).Select(x => new RowChild3
                    {
                        AverageInt = Math.Round(x.Sum(s => s.Average) / x.Sum(s => s.Credits), 0),
                        Average = Math.Round(x.Sum(s => s.Average) / x.Sum(s => s.Credits), 2),
                        CompetenceId = competence.Id,
                        StudentFullName = x.Key.StudentFullName,
                        UserName = x.Key.UserName,
                        StudentId = x.Key.StudentId,
                        Type = typeRangeList

                    }).ToList();

                    var total = 0;
                    total = objDataFinal.Count();

                    objDataFinal = objDataFinal.Where(x => minAvg <= x.AverageInt && x.AverageInt <= maxAvg).ToList();

                    var percentaje = Math.Round(((decimal)objDataFinal.Count() / (decimal)total * 100), 2);

                    var finalResult = new DataReport3
                    {
                        CompetenceName = $"{competence.Name} - {typeRange} ({percentaje}%)",
                        RowChilds = objDataFinal
                    };

                    return finalResult;

                }
                else
                {
                    var finalResult = new DataReport3
                    {
                        CompetenceName = $"{competence.Name} - {typeRange}",
                        RowChilds = objData
                    };

                    return finalResult;
                }


            }
            else
            {
                var queryStudentSections = _context.AcademicHistories.Where(x => x.TermId == term.Id && x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CompetencieId == competenceId)).AsQueryable();

                if (facultyId.HasValue && facultyId != Guid.Empty)
                {
                    queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);
                }

                if (careerId.HasValue && careerId != Guid.Empty)
                {
                    queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);
                }
                if (curriculumId.HasValue && curriculumId != Guid.Empty)
                {
                    queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CurriculumId == curriculumId));
                }

                var objData = await queryStudentSections
                                 .GroupBy(x => new { x.Section.CourseTerm.Course.Id, x.Section.CourseTerm.Course.Name, x.Section.CourseTerm.Course.Credits, x.StudentId, x.Student.User.FullName, x.Student.User.UserName }).Select(x => new RowChild3
                                 {
                                     CompetenceName = $"{competence.Name} - {typeRange}",
                                     StudentFullName = x.Key.FullName,
                                     UserName = x.Key.UserName,
                                     StudentId = x.Key.StudentId,
                                     Average = x.Sum(z => (decimal)z.Grade) * x.Key.Credits,
                                     CompetenceId = competence.Id,
                                     Credits = x.Key.Credits,
                                     Type = typeRangeList
                                 }).ToListAsync();



                if (objData != null && objData.Count > 0)
                {
                    var objDataFinal = objData.GroupBy(x => new { x.StudentId, x.UserName, x.StudentFullName }).Select(x => new RowChild3
                    {
                        AverageInt = Math.Round(x.Sum(s => s.Average) / x.Sum(s => s.Credits), 0),
                        Average = Math.Round(x.Sum(s => s.Average) / x.Sum(s => s.Credits), 2),
                        StudentFullName = x.Key.StudentFullName,
                        StudentId = x.Key.StudentId,
                        UserName = x.Key.UserName,
                        CompetenceId = competence.Id,
                        Type = typeRangeList

                    }).ToList();

                    var total = 0;
                    total = objDataFinal.Count();

                    objDataFinal = objDataFinal.Where(x => minAvg <= x.AverageInt && x.AverageInt <= maxAvg).ToList();

                    var percentaje = Math.Round(((decimal)objDataFinal.Count() / (decimal)total * 100), 2);

                    var finalResult = new DataReport3
                    {
                        CompetenceName = $"{competence.Name} - {typeRange} ({percentaje}%)",
                        RowChilds = objDataFinal
                    };

                    return finalResult;

                }
                else
                {
                    var finalResult = new DataReport3
                    {
                        CompetenceName = $"{competence.Name} - {typeRange}",
                        RowChilds = objData
                    };

                    return finalResult;
                }

            }

        }

        public async Task<object> AchievementLevelCompetenceStudentDetail(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, Guid? competenceId, Guid? studentId, List<RangeLevel> rangeLevelList)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();

            var studentFullName = await _context.Students.Where(x => x.Id == studentId).Select(x => x.User.FullName).FirstOrDefaultAsync();

            Competencie competence = null;
            if (competenceId.HasValue && competenceId != Guid.Empty)
            {
                competence = await _context.Competencies.Where(x => x.Id == competenceId).FirstOrDefaultAsync();
            }
            var typeRange = rangeLevelList.FirstOrDefault().Name;
            var minAvg = rangeLevelList.FirstOrDefault().Min;
            var maxAvg = rangeLevelList.FirstOrDefault().Max;
            var typeRangeList = rangeLevelList.FirstOrDefault().Type;

            if (term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
            {

                var queryStudentSections = _context.StudentSections.Where(x => x.Section.CourseTerm.TermId == term.Id && x.StudentId == studentId && x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CompetencieId == competenceId)).AsQueryable();

                if (facultyId.HasValue && facultyId != Guid.Empty)
                {
                    queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);
                }

                if (careerId.HasValue && careerId != Guid.Empty)
                {
                    queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);
                }
                if (curriculumId.HasValue && curriculumId != Guid.Empty)
                {
                    queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CurriculumId == curriculumId));
                }

                var objData = await queryStudentSections
                        .GroupBy(x => new { x.Section.CourseTerm.Course.Id, x.Section.CourseTerm.Course.Name, x.Section.CourseTerm.Course.Credits, x.StudentId, x.Student.User.FullName, x.Student.User.UserName }).Select(x => new RowChild3
                        {
                            CourseName = x.Key.Name,
                            Credits = x.Key.Credits,
                            Average = x.Sum(z => (decimal)z.FinalGrade)

                        }).ToListAsync();

                //objData = objData.Where(x => minAvg <= x.Average && x.Average <= maxAvg).ToList();

                var finalResult = new DataReport3
                {
                    StudentFullName = studentFullName,
                    RowChilds = objData
                };

                return finalResult;
            }
            else
            {
                var queryStudentSections = _context.AcademicHistories.Where(x => x.TermId == term.Id && x.StudentId == studentId && x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CompetencieId == competenceId)).AsQueryable();

                if (facultyId.HasValue && facultyId != Guid.Empty)
                {
                    queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);
                }

                if (careerId.HasValue && careerId != Guid.Empty)
                {
                    queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);
                }
                if (curriculumId.HasValue && curriculumId != Guid.Empty)
                {
                    queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CurriculumId == curriculumId));
                }

                var objData = await queryStudentSections
                                 .GroupBy(x => new { x.Section.CourseTerm.Course.Id, x.Section.CourseTerm.Course.Name, x.Section.CourseTerm.Course.Credits, x.StudentId, x.Student.User.FullName, x.Student.User.UserName }).Select(x => new RowChild3
                                 {
                                     CourseName = x.Key.Name,
                                     Credits = x.Key.Credits,
                                     Average = x.Sum(z => (decimal)z.Grade)
                                 }).ToListAsync();

                //objData = objData.Where(x => minAvg <= x.Average && x.Average <= maxAvg).ToList();
                var finalResult = new DataReport3
                {
                    StudentFullName = studentFullName,
                    RowChilds = objData
                };

                return finalResult;

            }
        }

        public async Task<object> GetReportAchievementLevel(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();

            var RangeLevelList = new List<RangeLevel>();
            RangeLevelList.Add(new RangeLevel
            {
                Name = "Deficiente",
                Min = 0,
                Max = 10,
                Total = 0,
                Type = 1,
                Array = new List<int>()
            });
            RangeLevelList.Add(new RangeLevel
            {
                Name = "Regular",
                Min = 11,
                Max = 13,
                Total = 0,
                Type = 2,
                Array = new List<int>()
            });
            RangeLevelList.Add(new RangeLevel
            {
                Name = "Bueno",
                Min = 14,
                Max = 16,
                Total = 0,
                Type = 3,
                Array = new List<int>()
            });
            RangeLevelList.Add(new RangeLevel
            {
                Name = "Excelente",
                Min = 17,
                Max = 20,
                Total = 0,
                Type = 4,
                Array = new List<int>()
            });

            if (term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
            {
                var query = _context.Competencies.AsQueryable();

                var competences = await query.ToListAsync();


                var lstCompetencesStrings = new List<string>();

                foreach (var item in competences)
                {


                    var queryStudentSections = _context.StudentSections.Where(x => x.Section.CourseTerm.TermId == term.Id && x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CompetencieId == item.Id)).AsQueryable();

                    if (facultyId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);
                    }

                    if (careerId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);
                    }
                    if (curriculumId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CurriculumId == curriculumId));
                    }

                    var objData = await queryStudentSections
                            .GroupBy(x => new { x.Section.CourseTerm.Course.Id, x.Section.CourseTerm.Course.Name, x.Section.CourseTerm.Course.Credits, x.StudentId, x.Student.User.FullName }).Select(x => new RowChild2
                            {
                                //CompetenceName = item.Name,
                                //StudentFullName = x.Key.FullName,
                                //CourseName = x.Key.Name,
                                StudentId = x.Key.StudentId,
                                Average = x.Sum(z => (decimal)z.FinalGrade) * x.Key.Credits,
                                Credits = x.Key.Credits

                            }).ToListAsync();

                    if (objData != null && objData.Count > 0)
                    {
                        var objDataFinal = objData.GroupBy(x => new { x.StudentId }).Select(x => new RowChild2
                        {
                            Average = Math.Round(x.Sum(s => s.Average) / x.Sum(s => s.Credits), 0)

                        }).ToList();

                        lstCompetencesStrings.Add(item.Name);
                        foreach (var i in RangeLevelList)
                        {
                            var totalDisp = 0;
                            foreach (var j in objDataFinal)
                            {
                                if (i.Min <= j.Average && j.Average <= i.Max)
                                {
                                    totalDisp++;
                                }
                            }
                            i.Array.Add(totalDisp);
                        }
                    }

                }
                lstCompetencesStrings.Sort();

                var result = new DataReport2
                {
                    RangeLevels = RangeLevelList,
                    CompetencesNames = lstCompetencesStrings
                };
                return result;

            }
            else
            {
                var query = _context.Competencies.AsQueryable();

                var competences = await query.ToListAsync();

                var lstCompetencesStrings = new List<string>();

                foreach (var item in competences)
                {

                    var queryStudentSections = _context.AcademicHistories.Where(x => x.TermId == term.Id && x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CompetencieId == item.Id)).AsQueryable();

                    if (facultyId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);
                    }

                    if (careerId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);
                    }
                    if (curriculumId != Guid.Empty)
                    {
                        queryStudentSections = queryStudentSections.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(z => z.CurriculumId == curriculumId));
                    }

                    var objData = await queryStudentSections
                                     .GroupBy(x => new { x.Section.CourseTerm.Course.Id, x.Section.CourseTerm.Course.Name, x.Section.CourseTerm.Course.Credits, x.StudentId, x.Student.User.FullName, x.Student.User.UserName }).Select(x => new RowChild2
                                     {
                                         //CompetenceName = item.Name,
                                         //StudentFullName = x.Key.FullName,
                                         //CourseName = x.Key.Name,
                                         StudentId = x.Key.StudentId,
                                         Average = x.Sum(z => (decimal)z.Grade) * x.Key.Credits,
                                         Credits = x.Key.Credits

                                     }).ToListAsync();



                    if (objData != null && objData.Count > 0)
                    {
                        var objDataFinal = objData.GroupBy(x => new { x.StudentId }).Select(x => new RowChild2
                        {
                            Average = Math.Round(x.Sum(s => s.Average) / x.Sum(s => s.Credits), 0)

                        }).ToList();

                        lstCompetencesStrings.Add(item.Name);
                        foreach (var i in RangeLevelList)
                        {
                            var totalDisp = 0;
                            foreach (var j in objDataFinal)
                            {
                                if (i.Min <= j.Average && j.Average <= i.Max)
                                {
                                    totalDisp++;
                                }
                            }
                            i.Array.Add(totalDisp);
                        }
                    }





                }
                lstCompetencesStrings.Sort();

                var result = new DataReport2
                {
                    RangeLevels = RangeLevelList,
                    CompetencesNames = lstCompetencesStrings
                };
                return result;

            }
        }

        public async Task<object> GetStudentsEnrolledByTermChart(Guid? termId = null, Guid? careerId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null)
        {
            var query = _context.StudentSections.AsQueryable();

            if (termId != null)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId);


            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                }
            }

            if (careerId != null)
                query = query.Where(x => x.Student.CareerId == careerId);

            if (academicProgramId != null)
                query = query.Where(x => x.Student.AcademicProgramId == academicProgramId);

            var enrolledStudents = await query
                .Select(x => new
                {
                    x.Section.CourseTerm.TermId,
                    x.StudentId
                }).Distinct()
                .GroupBy(x => x.TermId)
                .Select(x => new
                {
                    TermId = x.Key,
                    Count = x.Count()
                })
                .ToListAsync();

            var terms = await _context.Terms
                .Select(x => new
                {
                    x.Id,
                    x.Name
                }).ToListAsync();

            var data = terms
                .Select(x => new
                {
                    x.Name,
                    Count = enrolledStudents.Where(y => y.TermId == x.Id).Select(x => x.Count).FirstOrDefault()
                })
                .ToList();

            var result = new
            {
                categories = data.Select(x => x.Name).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<SectionPartialGradesReportTemplate> GetSectionPartialGradesTemplate(Guid sectionId, Guid? studentId = null)
        {
            var model = new SectionPartialGradesReportTemplate();

            var section = await _context.Sections.Where(x => x.Id == sectionId).FirstOrDefaultAsync();
            var courseTerm = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).FirstOrDefaultAsync();

            model.Section = section.Code;
            model.SectionId = section.Id;

            model.Evaluations = await _context.Evaluations.Where(x => x.CourseTermId == courseTerm.Id)
                .OrderBy(x => x.CourseUnit.Number).ThenBy(x => x.Week)
                .Select(x => new PartialGradeEvaluationTemplate
                {
                    EvaluationId = x.Id,
                    Evaluation = x.Name,
                    Percentage = x.Percentage,
                    GradeRegistrationDate = x.GradeRegistrations.Where(y => y.SectionId == sectionId).Select(y => y.Date).FirstOrDefault(),
                    HasGradeRegistration = x.GradeRegistrations.Any(y => y.SectionId == sectionId),
                    User = x.GradeRegistrations.Where(y => y.SectionId == sectionId).Select(x => x.Teacher.User.UserName).FirstOrDefault(),
                    GradeRegistationPublished = x.GradeRegistrations.Where(y => y.SectionId == sectionId).Select(y => y.WasPublished).FirstOrDefault(),
                })
                .ToListAsync();

            var queryStudents = _context.StudentSections
                .Where(x => x.SectionId == sectionId)
                .AsNoTracking();

            if (studentId.HasValue)
                queryStudents = queryStudents.Where(x => x.StudentId == studentId);

            var gradeCorrections = await _context.GradeCorrections.Where(x => x.Grade.StudentSection.SectionId == sectionId).ToListAsync();

            model.Students = await queryStudents
                .OrderBy(x => x.Student.User.FullName)
                .Select(x => new StudentPartialGradesReportTemplate
                {
                    StudentId = x.StudentId,
                    Status = x.Status,
                    StudentSectionId = x.Id,
                    FinalGrade = x.FinalGrade,
                    Username = x.Student.User.UserName,
                    FullName = x.Student.User.FullName,
                    Grades = x.Grades.Where(y => y.EvaluationId.HasValue).OrderBy(y => y.Evaluation.CourseUnit.Number).ThenBy(y => y.Evaluation.Week).
                    Select(y => new PartialGradesReportTemplate
                    {
                        Id = y.Id,
                        EvaluationId = y.EvaluationId.Value,
                        Value = y.Value,
                        //HasGradeCorrection = y.GradeCorrections.Any(z => z.State == ConstantHelpers.GRADECORRECTION_STATES.APPROBED),
                        //LastGrade = y.GradeCorrections.Where(z => z.State == ConstantHelpers.GRADECORRECTION_STATES.APPROBED).OrderByDescending(z => z.CreatedAt).Select(z => z.OldGrade).FirstOrDefault()
                    })
                    .ToList()
                })
                .ToListAsync();

            foreach (var student in model.Students)
            {
                student.Grades = student.Grades
                    .Select(x => new PartialGradesReportTemplate
                    {
                        Id = x.Id,
                        EvaluationId = x.EvaluationId,
                        Value = x.Value,
                        HasGradeCorrection = gradeCorrections.Any(y => y.GradeId == x.Id && y.State == ConstantHelpers.GRADECORRECTION_STATES.APPROBED),
                        LastGrade = gradeCorrections.Where(y => y.GradeId == x.Id && y.State == ConstantHelpers.GRADECORRECTION_STATES.APPROBED).OrderByDescending(z => z.CreatedAt).Select(z => z.OldGrade).FirstOrDefault()
                    })
                    .ToList();
            }

            return model;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsThirdEnrollmentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null)
        {
            var term = await _context.Terms.FindAsync(termId);
            Expression<Func<StudentSection, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Student.Career.Name;
                    break;
                case "3":
                    if (term.Status == ConstantHelpers.TERM_STATES.FINISHED) orderByPredicate = (x) => !x.Student.AcademicSummaries.Where(y => y.TermId == termId).Any() ? x.Student.CurrentAcademicYear : x.Student.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear);
                    else orderByPredicate = (x) => x.Student.CurrentAcademicYear;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Section.CourseTerm.Course.Name;
                    break;
                default:
                    break;
            }

            var query = _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == term.Id
                && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN && x.Try == 3)
                .AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (facultyId.HasValue && facultyId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                    if (careerId.HasValue && careerId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.Student.CareerId));
                }
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Student.CareerId == careerId);
            }

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(search.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    career = x.Student.Career.Name,
                    faculty = x.Student.Career.Faculty.Name,
                    code = x.Student.User.UserName,
                    name = x.Student.User.FullName,
                    course = x.Section.CourseTerm.Course.Name,
                    academicYear = term.Status == ConstantHelpers.TERM_STATES.FINISHED ?
                        x.Student.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear)
                        : x.Student.CurrentAcademicYear,
                    phone = x.Student.User.PhoneNumber,
                    email = x.Student.User.Email,
                    studentId = x.StudentId,
                    courseId = x.Section.CourseTerm.CourseId
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<ThridExcelTemplate>> GetStudentsThirdEnrollmentData(Guid termId, Guid? facultyId = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null)
        {
            var term = await _context.Terms.FindAsync(termId);
            var summerCountsTry = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.SUMMER_COURSE_COUNTS_TRY));

            var query = _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == term.Id
                && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN && x.Try == 3)
                .AsNoTracking();

            var historyQry = _context.AcademicHistories
                .Where(x => x.Term.StartDate < term.StartDate)
                .AsNoTracking();

            if (!summerCountsTry) historyQry = historyQry.Where(x => !x.Term.IsSummer);

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (facultyId.HasValue && facultyId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                    if (careerId.HasValue && careerId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.Student.CareerId));
                    historyQry = historyQry.Where(x => careers.Contains(x.Student.CareerId));
                }
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty)
                {
                    query = query.Where(x => x.Student.Career.FacultyId == facultyId);
                    historyQry = historyQry.Where(x => x.Student.Career.FacultyId == facultyId);
                }
                if (careerId.HasValue && careerId != Guid.Empty)
                {
                    query = query.Where(x => x.Student.CareerId == careerId);
                    historyQry = historyQry.Where(x => x.Student.CareerId == careerId);
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(search.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(search.ToUpper()));
                historyQry = historyQry.Where(x => x.Student.User.FullName.ToUpper().Contains(search.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(search.ToUpper()));
            }

            var academicHistories = await historyQry
                .Select(x => new
                {
                    x.StudentId,
                    x.CourseId,
                    x.Grade,
                    Term = x.Term.Name,
                    x.Term.StartDate,
                    x.Try
                })
                .ToListAsync();

            var data = await query
                .Select(x => new ThridExcelTemplate
                {
                    AcademicYear = term.Status == ConstantHelpers.TERM_STATES.FINISHED ?
                        x.Student.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear)
                        : x.Student.CurrentAcademicYear,
                    Career = x.Student.Career.Name,
                    Faculty = x.Student.Career.Faculty.Name,
                    Code = x.Student.User.UserName,
                    Name = x.Student.User.FullName,
                    Course = x.Section.CourseTerm.Course.Name,
                    Email = x.Student.User.Email,
                    PhoneNumber = x.Student.User.PhoneNumber,

                    CourseId = x.Section.CourseTerm.CourseId,
                    StudentId = x.StudentId
                }).ToListAsync();

            foreach (var item in data)
            {
                var histories = academicHistories
                    .Where(x => x.StudentId == item.StudentId && x.CourseId == item.CourseId)
                    .OrderBy(x => x.StartDate).ToList();

                item.FirstTryTerm = histories.FirstOrDefault().Term;
                item.FirstTryGrade = histories.FirstOrDefault().Grade;

                item.SecondTryTerm = histories.Skip(1).FirstOrDefault().Term;
                item.SecondTryGrade = histories.Skip(1).FirstOrDefault().Grade;
            }

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsFourthEnrollmentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null)
        {
            var term = await _context.Terms.FindAsync(termId);
            Expression<Func<StudentSection, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Student.Career.Name;
                    break;
                case "3":
                    if (term.Status == ConstantHelpers.TERM_STATES.FINISHED) orderByPredicate = (x) => !x.Student.AcademicSummaries.Where(y => y.TermId == termId).Any() ? x.Student.CurrentAcademicYear : x.Student.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear);
                    else orderByPredicate = (x) => x.Student.CurrentAcademicYear;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Section.CourseTerm.Course.Name;
                    break;
                default:
                    break;
            }

            var query = _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == term.Id
                && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN && x.Try > 3)
                .AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (facultyId.HasValue && facultyId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                    if (careerId.HasValue && careerId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.Student.CareerId));
                }
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Student.CareerId == careerId);
            }

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(search.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    @try = x.Try,
                    career = x.Student.Career.Name,
                    faculty = x.Student.Career.Faculty.Name,
                    code = x.Student.User.UserName,
                    name = x.Student.User.FullName,
                    course = x.Section.CourseTerm.Course.Name,
                    academicYear = term.Status == ConstantHelpers.TERM_STATES.FINISHED ?
                        x.Student.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear)
                        : x.Student.CurrentAcademicYear,
                    phone = x.Student.User.PhoneNumber,
                    email = x.Student.User.Email,
                    studentId = x.StudentId,
                    courseId = x.Section.CourseTerm.CourseId
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<FourthExcelTemplate>> GetStudentsFourthEnrollmentData(Guid termId, Guid? facultyId = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null)
        {
            var term = await _context.Terms.FindAsync(termId);
            var summerCountsTry = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.SUMMER_COURSE_COUNTS_TRY));

            var query = _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == term.Id
                && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN && x.Try > 3)
                .AsNoTracking();

            var historyQry = _context.AcademicHistories
                .Where(x => x.Term.StartDate < term.StartDate)
                .AsNoTracking();

            if (!summerCountsTry) historyQry = historyQry.Where(x => !x.Term.IsSummer);

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (facultyId.HasValue && facultyId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                    if (careerId.HasValue && careerId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.Student.CareerId));
                    historyQry = historyQry.Where(x => careers.Contains(x.Student.CareerId));
                }
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty)
                {
                    query = query.Where(x => x.Student.Career.FacultyId == facultyId);
                    historyQry = historyQry.Where(x => x.Student.Career.FacultyId == facultyId);
                }
                if (careerId.HasValue && careerId != Guid.Empty)
                {
                    query = query.Where(x => x.Student.CareerId == careerId);
                    historyQry = historyQry.Where(x => x.Student.CareerId == careerId);
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(search.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(search.ToUpper()));
                historyQry = historyQry.Where(x => x.Student.User.FullName.ToUpper().Contains(search.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(search.ToUpper()));
            }

            var academicHistories = await historyQry
                .Select(x => new
                {
                    x.StudentId,
                    x.CourseId,
                    x.Grade,
                    Term = x.Term.Name,
                    x.Term.StartDate,
                    x.Try
                })
                .ToListAsync();

            var data = await query
                .Select(x => new FourthExcelTemplate
                {
                    Try = x.Try,
                    AcademicYear = term.Status == ConstantHelpers.TERM_STATES.FINISHED ?
                        x.Student.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear)
                        : x.Student.CurrentAcademicYear,
                    Career = x.Student.Career.Name,
                    Faculty = x.Student.Career.Faculty.Name,
                    Code = x.Student.User.UserName,
                    Name = x.Student.User.FullName,
                    Course = x.Section.CourseTerm.Course.Name,
                    Email = x.Student.User.Email,
                    PhoneNumber = x.Student.User.PhoneNumber,

                    CourseId = x.Section.CourseTerm.CourseId,
                    StudentId = x.StudentId
                }).ToListAsync();


            foreach (var item in data)
            {
                var histories = academicHistories
                    .Where(x => x.StudentId == item.StudentId && x.CourseId == item.CourseId)
                    .OrderBy(x => x.StartDate).ToList();

                item.FirstTryTerm = histories.FirstOrDefault().Term;
                item.FirstTryGrade = histories.FirstOrDefault().Grade;

                item.SecondTryTerm = histories.Skip(1).FirstOrDefault().Term;
                item.SecondTryGrade = histories.Skip(1).FirstOrDefault().Grade;

                item.ThirdTryTerm = histories.Skip(2).FirstOrDefault().Term;
                item.ThirdTryGrade = histories.Skip(2).FirstOrDefault().Grade;
            }

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionsByCareerDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            var query = _context.StudentSections.Where(x => !x.Section.IsDirectedCourse).AsNoTracking();

            if (termId != null)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                }
            }


            if (careerId != null)
                query = query.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);

            int recordsFiltered = await query
                .Select(x => new
                {
                    x.Section.CourseTerm.TermId,
                    x.Section.CourseTerm.Course.CareerId
                })
                .Distinct()
                .CountAsync();

            var data = await query
                    .Select(x => new
                    {
                        x.SectionId,
                        x.Section.CourseTerm.TermId,
                        TermName = x.Section.CourseTerm.Term.Name,
                        TermYear = x.Section.CourseTerm.Term.Year,
                        TermNumber = x.Section.CourseTerm.Term.Number,
                        Career = x.Section.CourseTerm.Course.Career.Name,
                        x.Section.CourseTerm.Course.CareerId
                    })
                    .Distinct()
                    .GroupBy(x => new { x.TermId, x.TermName, x.TermNumber, x.TermYear, x.Career })
                    .Select(x => new
                    {
                        x.Key.Career,
                        Term = x.Key.TermName,
                        Sections = x.Count()
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

        public async Task<object> GetSectionsByCareerChart(Guid? termId = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            var studentSections = _context.StudentSections
                            .Where(x => !x.Section.IsDirectedCourse)
                            .AsQueryable();

            var careersQuery = _context.Careers.AsNoTracking();

            if (termId != null)
                studentSections = studentSections.Where(x => x.Section.CourseTerm.TermId == termId);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    studentSections = studentSections.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                    careersQuery = careersQuery.Where(x => x.QualityCoordinatorId == userId);
                }
            }


            if (careerId != null)
                studentSections = studentSections.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);

            var data = await careersQuery
                .Select(x => new
                {
                    Career = x.Name,
                    Count = studentSections.Where(y => y.Section.CourseTerm.Course.CareerId == x.Id).Select(x => x.SectionId).Distinct().Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Career)
                .ToListAsync();


            var result = new
            {
                categories = data.Select(x => x.Career).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionsByFacultyDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? facultyId = null, ClaimsPrincipal user = null)
        {
            var query = _context.StudentSections.Where(x => !x.Section.IsDirectedCourse).AsNoTracking();

            if (termId != null)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Section.CourseTerm.Course.Career.QualityCoordinatorId == userId);
                }
            }

            if (facultyId != null)
                query = query.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);

            int recordsFiltered = await query
                .Select(x => new
                {
                    x.Section.CourseTerm.TermId,
                    x.Section.CourseTerm.Course.Career.FacultyId
                })
                .Distinct()
                .CountAsync();

            var data = await query
                    .Select(x => new
                    {
                        x.SectionId,
                        x.Section.CourseTerm.TermId,
                        TermName = x.Section.CourseTerm.Term.Name,
                        TermYear = x.Section.CourseTerm.Term.Year,
                        TermNumber = x.Section.CourseTerm.Term.Number,
                        Faculty = x.Section.CourseTerm.Course.Career.Faculty.Name,
                        x.Section.CourseTerm.Course.Career.FacultyId
                    })
                    .Distinct()
                    .GroupBy(x => new { x.TermId, x.TermName, x.TermNumber, x.TermYear, x.Faculty })
                    .Select(x => new
                    {
                        x.Key.Faculty,
                        Term = x.Key.TermName,
                        Sections = x.Count()
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

        public async Task<object> GetSectionsByFacultyChart(Guid? termId = null, Guid? facultyId = null, ClaimsPrincipal user = null)
        {
            var studentSections = _context.StudentSections
                                        .Where(x => !x.Section.IsDirectedCourse)
                                        .AsQueryable();

            var facultiesQuery = _context.Faculties.AsNoTracking();

            if (termId != null)
                studentSections = studentSections.Where(x => x.Section.CourseTerm.TermId == termId);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    studentSections = studentSections.Where(x => x.Section.CourseTerm.Course.Career.QualityCoordinatorId == userId);
                    facultiesQuery = facultiesQuery.Where(x => x.Careers.Any(y => y.QualityCoordinatorId == userId));
                }
            }

            if (facultyId != null)
                studentSections = studentSections.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);

            var data = await facultiesQuery
                .Select(x => new
                {
                    Faculty = x.Name,
                    Count = studentSections.Where(y => y.Section.CourseTerm.Course.Career.FacultyId == x.Id).Select(x => x.SectionId).Distinct().Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Faculty)
                .ToListAsync();


            var result = new
            {
                categories = data.Select(x => x.Faculty).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsPerformanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, ClaimsPrincipal user = null)
        {
            var query = _context.StudentSections
                .Where(x => !x.Section.IsDirectedCourse &&
                (x.Status == ConstantHelpers.STUDENT_SECTION_STATES.APPROVED || x.Status == ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED))
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                }
            }

            if (termId != null)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            int recordsFiltered = await query
                .Select(x => x.Section.CourseTerm.TermId)
                .Distinct()
                .CountAsync();

            var preData = await query
                    .Select(x => new
                    {
                        x.Section.CourseTerm.TermId,
                        TermName = x.Section.CourseTerm.Term.Name,
                        TermYear = x.Section.CourseTerm.Term.Year,
                        TermNumber = x.Section.CourseTerm.Term.Number,
                        x.Status,
                        x.Section.CourseTerm.Course.Credits
                    })
                    .ToListAsync();

            var data = preData
                    .OrderByDescending(x => x.TermYear)
                    .ThenByDescending(x => x.TermNumber)
                    .GroupBy(x => new { x.TermId, x.TermName, x.TermNumber, x.TermYear })
                    .Select(x => new
                    {
                        Term = x.Key.TermName,
                        EnrolledCredits = x.Sum(y => y.Credits),
                        ApprovedCredits = x.Where(y => y.Status == ConstantHelpers.STUDENT_SECTION_STATES.APPROVED).Sum(y => y.Credits)
                    })
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .ToList();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetStudentsPerformanceChart(Guid? termId = null, ClaimsPrincipal user = null)
        {
            var query = _context.StudentSections
                .Where(x => !x.Section.IsDirectedCourse &&
                (x.Status == ConstantHelpers.STUDENT_SECTION_STATES.APPROVED || x.Status == ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED))
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                }
            }

            if (termId != null)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            var preData = await query
                    .Select(x => new
                    {
                        x.Section.CourseTerm.TermId,
                        TermName = x.Section.CourseTerm.Term.Name,
                        TermYear = x.Section.CourseTerm.Term.Year,
                        TermNumber = x.Section.CourseTerm.Term.Number,
                        x.Status,
                        x.Section.CourseTerm.Course.Credits
                    })
                    .ToListAsync();

            var data = preData
                    .OrderByDescending(x => x.TermYear)
                    .ThenByDescending(x => x.TermNumber)
                    .GroupBy(x => new { x.TermId, x.TermName, x.TermNumber, x.TermYear })
                    .Select(x => new
                    {
                        Term = x.Key.TermName,
                        EnrolledCredits = x.Sum(y => y.Credits),
                        ApprovedCredits = x.Where(y => y.Status == ConstantHelpers.STUDENT_SECTION_STATES.APPROVED).Sum(y => y.Credits)
                    })
                    .ToList();

            var result = new
            {
                categories = data.Select(x => x.Term).ToList(),
                data = data.Select(x => x.EnrolledCredits == 0 ? 0.0 :
                    Math.Round(((double)x.ApprovedCredits * 100.0) / ((double)x.EnrolledCredits * 1.0), 2, MidpointRounding.AwayFromZero)).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentPartenStudyLevelDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, ClaimsPrincipal user = null)
        {
            var query = _context.StudentSections
                .Where(x => !x.Section.IsDirectedCourse)
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                }
            }

            if (termId != null)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            int recordsFiltered = await query
                .Select(x => x.Section.CourseTerm.TermId)
                .Distinct()
                .CountAsync();

            var preData = await query
                    .Select(x => new
                    {
                        x.Section.CourseTerm.TermId,
                        TermName = x.Section.CourseTerm.Term.Name,
                        TermYear = x.Section.CourseTerm.Term.Year,
                        TermNumber = x.Section.CourseTerm.Term.Number,
                        x.StudentId,
                        ParentNotFinishHighSchool = x.Student
                            .StudentFamilies
                            .Where(y => y.RelationshipInt == ConstantHelpers.STUDENT_FAMILY.RELATIONSHIPS.FATHER ||
                                y.RelationshipInt == ConstantHelpers.STUDENT_FAMILY.RELATIONSHIPS.MOTHER)
                            .Any(y => y.DegreeInstructionInt == ConstantHelpers.STUDENT_FAMILY.CERTIFICATES.NO_STUDIES ||
                                    y.DegreeInstructionInt == ConstantHelpers.STUDENT_FAMILY.CERTIFICATES.COMPLETE_PRIMARY ||
                                    y.DegreeInstructionInt == ConstantHelpers.STUDENT_FAMILY.CERTIFICATES.INCOMPLETE_PRIMARY ||
                                    y.DegreeInstructionInt == ConstantHelpers.STUDENT_FAMILY.CERTIFICATES.INCOMPLETE_SECONDARY)
                    })
                    .Distinct()
                    .ToListAsync();

            var data = preData
                    .OrderByDescending(x => x.TermYear)
                    .ThenByDescending(x => x.TermNumber)
                    .GroupBy(x => new { x.TermId, x.TermName, x.TermNumber, x.TermYear })
                    .Select(x => new
                    {
                        Term = x.Key.TermName,
                        EnrolledStudent = x.Count(),
                        StudentParentNotFinishHighSchool = x.Where(y => y.ParentNotFinishHighSchool).Count()
                    })
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .ToList();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetStudentPartenStudyLevelChart(Guid? termId = null, ClaimsPrincipal user = null)
        {
            var query = _context.StudentSections
                .Where(x => !x.Section.IsDirectedCourse)
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                }
            }

            if (termId != null)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            int recordsFiltered = await query
                .Select(x => x.Section.CourseTerm.TermId)
                .Distinct()
                .CountAsync();

            var preData = await query
                    .Select(x => new
                    {
                        x.Section.CourseTerm.TermId,
                        TermName = x.Section.CourseTerm.Term.Name,
                        TermYear = x.Section.CourseTerm.Term.Year,
                        TermNumber = x.Section.CourseTerm.Term.Number,
                        x.StudentId,
                        ParentNotFinishHighSchool = x.Student
                            .StudentFamilies
                            .Where(y => y.RelationshipInt == ConstantHelpers.STUDENT_FAMILY.RELATIONSHIPS.FATHER ||
                                y.RelationshipInt == ConstantHelpers.STUDENT_FAMILY.RELATIONSHIPS.MOTHER)
                            .Any(y => y.DegreeInstructionInt == ConstantHelpers.STUDENT_FAMILY.CERTIFICATES.NO_STUDIES ||
                                    y.DegreeInstructionInt == ConstantHelpers.STUDENT_FAMILY.CERTIFICATES.COMPLETE_PRIMARY ||
                                    y.DegreeInstructionInt == ConstantHelpers.STUDENT_FAMILY.CERTIFICATES.INCOMPLETE_PRIMARY ||
                                    y.DegreeInstructionInt == ConstantHelpers.STUDENT_FAMILY.CERTIFICATES.INCOMPLETE_SECONDARY)
                    })
                    .Distinct()
                    .ToListAsync();

            var data = preData
                    .OrderByDescending(x => x.TermYear)
                    .ThenByDescending(x => x.TermNumber)
                    .GroupBy(x => new { x.TermId, x.TermName, x.TermNumber, x.TermYear })
                    .Select(x => new
                    {
                        Term = x.Key.TermName,
                        EnrolledStudent = x.Count(),
                        StudentParentNotFinishHighSchool = x.Where(y => y.ParentNotFinishHighSchool).Count()
                    })
                    .ToList();


            var result = new
            {
                categories = data.Select(x => x.Term).ToList(),
                data = data.Select(x => x.EnrolledStudent == 0 ? 0.0 :
                    Math.Round((x.StudentParentNotFinishHighSchool * 100.0) / (x.EnrolledStudent * 1.0), 2, MidpointRounding.AwayFromZero)).ToList()
            };

            return result;
        }

        public async Task<List<EnrolledStudentTemplate>> GetEnrolledStudentWithOutInstitutionalEmailData(Guid termId)
        {
            var query = _context.StudentSections
                    .Where(x => x.Section.CourseTerm.TermId == termId && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                    .AsNoTracking();

            query = query.Where(x => string.IsNullOrEmpty(x.Student.User.Email));

            var data = await query
                .Select(x => new EnrolledStudentTemplate
                {
                    UserName = x.Student.User.UserName,
                    PaternalSurname = x.Student.User.PaternalSurname,
                    MaternalSurname = x.Student.User.MaternalSurname,
                    Name = x.Student.User.Name,
                    Career = x.Student.Career.Name,
                    DNI = x.Student.User.Dni
                })
                .Distinct()
                .ToListAsync();

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConditionedStudentSectionsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? careerId = null, byte? conditionType = null, string search = null)
        {
            Expression<Func<AcademicHistory, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Student.User.UserName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Student.User.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Student.Career.Name);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Course.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Try);
                    break;
                default:
                    break;
            }

            var query = _context.AcademicHistories
               .AsNoTracking();

            if (termId.HasValue && termId != Guid.Empty) query = query.Where(q => q.TermId == termId);

            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(q => q.Student.CareerId == careerId);

            if (conditionType == 99)
            {
                query = query.Where(x => x.Try > 5 && !x.Approved && !x.Withdraw);
            }
            else
            {
                if (conditionType.HasValue && conditionType > 1)
                    query = query.Where(x => !x.Approved && x.Try == conditionType.Value && !x.Withdraw);
            }

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.UserName.ToUpper().Contains(search.ToUpper()) || x.Student.User.FullName.ToUpper().Contains(search.ToUpper()));

            Expression<Func<AcademicHistory, dynamic>> searchFilter = (x) => new
            {
                x.Student.User.UserName,
                x.Student.User.FullName
            };

            var recordsFiltered = query
                 .Select(x => new
                 {
                     x.Id,
                     x.Student.User.UserName,
                     x.Student.User.FullName,
                     Term = x.Term.Name,
                     Course = x.Course.Name,
                     x.Try
                 }, search, searchFilter)
                .Count();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Student.User.UserName,
                    x.Student.User.FullName,
                    Term = x.Term.Name,
                    Course = x.Course.Name,
                    Career = x.Student.Career.Name,
                    x.Try
                }, search, searchFilter)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<ConditionedStudentSectionTemplate>> GetConditionedStudentSectionsData(Guid? termId = null, Guid? careerId = null, byte? conditionType = null, string search = null)
        {
            var query = _context.AcademicHistories
              .AsNoTracking();

            if (termId.HasValue && termId != Guid.Empty) query = query.Where(q => q.TermId == termId);

            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(q => q.Student.CareerId == careerId);

            if (conditionType == 99)
            {
                query = query.Where(x => !x.Approved && x.Try > 5 && !x.Withdraw);
            }
            else
            {
                if (conditionType.HasValue && conditionType > 1)
                    query = query.Where(x => !x.Approved && x.Try == conditionType.Value && !x.Withdraw);
            }

            var data = await query
                .Select(x => new ConditionedStudentSectionTemplate
                {
                    Id = x.Id,
                    UserName = x.Student.User.UserName,
                    FullName = x.Student.User.FullName,
                    Term = x.Term.Name,
                    Course = x.Course.Name,
                    Career = x.Student.Career.Name,
                    Try = x.Try
                }, search)
                .ToListAsync();

            var recordsTotal = data.Count;

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentWithdrawnDatatable(DataTablesStructs.SentParameters sentParameters,
            Guid termId, Guid? facultyId, Guid? careerId, Guid? courseId, string searchValue = null, ClaimsPrincipal user = null)
        {
            Expression<Func<StudentSection, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Student.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Section.CourseTerm.Course.Code;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Section.CourseTerm.Course.Name;
                    break;
                default:
                    break;
            }

            var query = _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == termId && x.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
                {
                    query = query.Where(x => x.Student.Career.Faculty.DeanId == userId || x.Student.Career.Faculty.SecretaryId == userId || x.Student.Career.Faculty.AdministrativeAssistantId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                        .AsNoTracking();

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.Student.CareerId));
                }
            }

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Student.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.Student.CareerId == careerId);

            if (courseId.HasValue && courseId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.CourseId == courseId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x =>
                x.Section.CourseTerm.Course.Code.Contains(searchValue) ||
                x.Section.CourseTerm.Course.Name.Contains(searchValue) ||
                x.Student.User.UserName.Contains(searchValue) ||
                x.Student.User.FullName.Contains(searchValue));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    studentCode = x.Student.User.UserName,
                    studentName = x.Student.User.FullName,
                    career = x.Student.Career.Name,
                    courseCode = x.Section.CourseTerm.Course.Code,
                    courseName = x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code
                })
                .ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<StudentWithdrawnTemplate>> GetStudentWithdrawnData(Guid termId, Guid? facultyId, Guid? careerId, Guid? courseId, string searchValue = null, ClaimsPrincipal user = null)
        {
            var query = _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == termId && x.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
                {
                    query = query.Where(x => x.Student.Career.Faculty.DeanId == userId || x.Student.Career.Faculty.AdministrativeAssistantId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {

                    if (!string.IsNullOrEmpty(userId))
                    {
                        var qryCareers = _context.Careers
                            .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                            .AsNoTracking();

                        var careers = qryCareers.Select(x => x.Id).ToHashSet();

                        query = query.Where(x => careers.Contains(x.Student.CareerId));
                    }
                }
            }



            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Student.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.Student.CareerId == careerId);

            if (courseId.HasValue && courseId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.CourseId == courseId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x =>
                x.Section.CourseTerm.Course.Code.Contains(searchValue) ||
                x.Section.CourseTerm.Course.Name.Contains(searchValue) ||
                x.Student.User.UserName.Contains(searchValue) ||
                x.Student.User.FullName.Contains(searchValue));
            }

            var data = await query
                .Select(x => new StudentWithdrawnTemplate
                {
                    UserName = x.Student.User.UserName,
                    FullName = x.Student.User.FullName,
                    Career = x.Student.Career.Name,
                    Code = x.Section.CourseTerm.Course.Code,
                    Course = x.Section.CourseTerm.Course.Name,
                    Section = x.Section.Code
                })
                .ToListAsync();

            return data;
        }

        public async Task<EnrollmentReportTemplate> GetEnrollmentReportTemplate(Guid studentId, Guid? termId = null, bool? pronabec = false, string qrUrl = null, bool? includeExtraordinaryEvaluations = false, bool? isExtraordinaryReport = false)
        {
            var template = new EnrollmentReportTemplate();

            Term term = null;
            if (termId.HasValue && termId != Guid.Empty)
                term = await _context.Terms.FindAsync(termId);
            else term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            if (term == null) return null;

            template = await _context.Students
                .Where(x => x.Id == studentId)
                .Select(x => new EnrollmentReportTemplate
                {
                    StudentName = x.User.FullName,
                    StudentCode = x.User.UserName,
                    Document = x.User.Document,
                    BirthDay = $"{x.User.BirthDate:dd/MM/yyyy}",
                    PhoneNumber = x.User.PhoneNumber,
                    Email = x.User.Email,

                    SchoolName = x.Career.Name,
                    FacultyName = x.Career.Faculty.Name,
                    Curriculum = x.Curriculum.Code,
                    CurriculumId = x.CurriculumId,
                    //CurrentSemester = x.CurrentAcademicYear,
                    CurrentSemester = ConstantHelpers.ACADEMIC_YEAR.TEXT[x.CurrentAcademicYear],

                    MaxAcademicYear = x.Curriculum.AcademicYearCourses.Max(y => y.AcademicYear),
                    AdmissionTerm = x.AcademicHistories.OrderBy(y => y.Term.Year).ThenBy(y => y.Term.Number).Select(y => y.Term.Name).FirstOrDefault(),
                    Semester = term.Name
                }).FirstOrDefaultAsync();

            if (template.CurriculumId == Guid.Parse("{02a3cb73-c3f2-476d-9361-590fa4632ce2}") && template.SchoolName == "ODONTOLOGIA")
                template.MaxAcademicYear = 12;

            var enrollmentTurn = await _context.EnrollmentTurns.FirstOrDefaultAsync(x => x.StudentId == studentId && x.TermId == term.Id);
            var adminEnrollment = await _context.AdminEnrollments
                .Where(x => x.StudentId == studentId && x.TermId == term.Id)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            template.EnrollmentDate = enrollmentTurn != null && enrollmentTurn.ConfirmationDate.HasValue
                    ? $"{enrollmentTurn.ConfirmationDate.ToDefaultTimeZone():dd/MM/yyyy}"
                    : adminEnrollment != null && adminEnrollment.CreatedAt.HasValue ? $"{adminEnrollment.CreatedAt.Value.ToDefaultTimeZone():dd/MM/yyyy}"
                    : "";
            template.EnrollmentHour = enrollmentTurn != null && enrollmentTurn.ConfirmationDate.HasValue
                    ? enrollmentTurn.ConfirmationDate.Value.ToDefaultTimeZone().ToShortTimeString()
                    : adminEnrollment != null && adminEnrollment.CreatedAt.HasValue ? adminEnrollment.CreatedAt.Value.ToDefaultTimeZone().ToShortTimeString()
                    : "";
            //PrintingDate = DateTime.UtcNow.ToDefaultTimeZone().ToString("dddd, dd MMMM yyyy", new CultureInfo("es-PE")),
            template.PrintingDate = $"{DateTime.UtcNow.ToDefaultTimeZone():dd/MM/yyyy}";
            template.PrintingHour = DateTime.UtcNow.ToDefaultTimeZone().ToShortTimeString();

            if (term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
            {
                var summary = await _context.AcademicSummaries
                    .Where(x => x.StudentId == studentId && x.TermId == term.Id)
                    .Select(x => new
                    {
                        x.CurriculumId,
                        Curriculum = x.Curriculum.Code,
                        x.StudentAcademicYear,

                        SchoolName = x.Career.Name,
                        FacultyName = x.Career.Faculty.Name,
                        MaxAcademicYear = x.CurriculumId.HasValue ? x.Curriculum.AcademicYearCourses.Max(y => y.AcademicYear) : 0,
                    })
                    .FirstOrDefaultAsync();

                if (summary != null)
                {
                    template.SchoolName = summary.SchoolName;
                    template.FacultyName = summary.FacultyName;
                    template.CurrentSemester = ConstantHelpers.ACADEMIC_YEAR.TEXT[summary.StudentAcademicYear];

                    if (summary.CurriculumId.HasValue)
                    {
                        template.CurriculumId = summary.CurriculumId.Value;
                        template.Curriculum = summary.Curriculum;
                        template.MaxAcademicYear = summary.MaxAcademicYear;
                    }
                }

            }

            #region Obtener cursos
            var courses = new List<EnrollmentReportCourseTemplate>();

            if (isExtraordinaryReport.HasValue && isExtraordinaryReport == true)
            {
                includeExtraordinaryEvaluations = true;
                template.StudentCondition = "EXTRAORD.";
                template.IsExtraordinaryReport = true;
            }
            else
            {
                //var academicHistoriesQry = _context.AcademicHistories.Where(x => x.StudentId == studentId).AsNoTracking();
                //if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD)
                //    academicHistoriesQry = academicHistoriesQry.Where(x => x.Term.Number != "A" && !x.Term.IsSummer);
                //else academicHistoriesQry = academicHistoriesQry.Where(x => x.Term.Number != "A" && x.Type != ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION);

                //var academicHistories = await academicHistoriesQry.Select(x => x.CourseId).ToListAsync();

                var studentSections = await _context.StudentSections
                    .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == term.Id)
                    .Select(x => new
                    {
                        AcademicYear = x.Section.CourseTerm.Course.AcademicYearCourses.Where(y => y.CurriculumId == template.CurriculumId).Select(y => y.AcademicYear).FirstOrDefault(),
                        CourseCode = x.Section.CourseTerm.Course.Code,
                        CourseName = x.Section.CourseTerm.Course.Name,
                        x.Section.CourseTerm.Course.Credits,
                        Section = x.Section.Code,
                        x.Status,
                        x.Try,
                        x.Section.CourseTerm.Course.TheoreticalHours,
                        x.Section.CourseTerm.Course.SeminarHours,
                        x.Section.CourseTerm.Course.PracticalHours,
                        x.Section.CourseTerm.CourseId,
                        x.Section.IsDirectedCourse
                    }).ToListAsync();

                foreach (var item in studentSections)
                {
                    var course = new EnrollmentReportCourseTemplate
                    {
                        CourseCode = item.CourseCode,
                        CourseName = item.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN ?
                        $"{item.CourseName} - RETIRADO" : item.CourseName,
                        Section = item.Section,
                        Credits = item.Credits,
                        AcademicYear = item.AcademicYear,
                        AcademicYearText = ConstantHelpers.ACADEMIC_YEAR.TEXT[item.AcademicYear],
                        LaboratoryHours = item.SeminarHours,
                        PracticalHours = item.PracticalHours,
                        TheoricalHours = item.TheoreticalHours,
                        Modality = item.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN
                        ? "RETIRADO"
                        : item.IsDirectedCourse ? "DIRIGIDO"
                        : ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES.ContainsKey(item.Try) ? ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES[item.Try] : "-",
                        Try = item.Try.ToString("D2")//academicHistories.Where(x => x == item.CourseId).Count().ToString("D2")
                    };
                    courses.Add(course);
                }

                if (studentSections.Any())
                {
                    if (studentSections.Any(x => x.IsDirectedCourse))
                    {
                        template.StudentCondition = "DIRIGIDO";
                    }
                    else
                    {
                        var conditionTry = studentSections.Max(x => x.Try);
                        template.StudentCondition = ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES.ContainsKey(conditionTry) ? ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES[conditionTry] : "REGULAR";
                    }
                }
            }

            if (includeExtraordinaryEvaluations.HasValue && includeExtraordinaryEvaluations == true)
            {
                var evaluations = await _context.ExtraordinaryEvaluationStudents
                    .Where(x => x.StudentId == studentId && x.ExtraordinaryEvaluation.TermId == term.Id)
                    .Select(x => new EnrollmentReportCourseTemplate
                    {
                        AcademicYear = x.ExtraordinaryEvaluation.Course.AcademicYearCourses
                        .Where(ayc => ayc.CourseId == x.ExtraordinaryEvaluation.CourseId)
                        .Select(ayc => ayc.AcademicYear)
                        .FirstOrDefault(),
                        Section = "",
                        Modality = "EXTRAORD.",
                        Credits = x.ExtraordinaryEvaluation.Course.Credits,
                        CourseCode = x.ExtraordinaryEvaluation.Course.Code,
                        CourseName = x.ExtraordinaryEvaluation.Course.Name
                    })
                    .ToListAsync();

                foreach (var item in evaluations)
                {
                    item.AcademicYearText = ConstantHelpers.ACADEMIC_YEAR.TEXT[item.AcademicYear];
                }

                courses.AddRange(evaluations);
            }

            template.Courses = courses.OrderBy(x => x.AcademicYear).ThenBy(x => x.CourseCode).ToList();
            template.Group = courses.Select(x => x.Section).FirstOrDefault();
            #endregion

            if (!string.IsNullOrEmpty(qrUrl))
            {
                var qrCode = GenerateQR(qrUrl);
                template.ImageQR = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(qrCode));
            }

            template.HeaderText = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_REPORT_HEADER_TEXT);
            template.SubHeaderText = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_REPORT_SUBHEADER_TEXT);
            template.FooterText = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_REPORT_FOOTER_TEXT);

            if (pronabec.HasValue) template.IsPronabec = pronabec.Value;

            return template;
        }

        private byte[] GenerateQR(string informationQR)
        {
            var qr = $"{informationQR}";
            var qrGenerator = new QRCoder.QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(qr, QRCoder.QRCodeGenerator.ECCLevel.Q);
            //var qrCode = new QRCoder.QRCode(qrCodeData);
            var qrCode = new QRCoder.PngByteQRCode(qrCodeData);
            //var stream = new MemoryStream();
            //qrCode.GetGraphic(5).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            ////System.IO.File.WriteAllBytes("D:\\QR.png",imageBytes);
            //return stream.ToArray();
            return qrCode.GetGraphic(5);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsApprovedDisapprovedBySectionDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid careerId, Guid curriculumId, int? year = null, ClaimsPrincipal user = null)
        {
            Expression<Func<Section, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                //case "0":
                //    orderByPredicate = (x) => x.CourseTerm.Term.Name;
                //    break;
                //case "1":
                //    orderByPredicate = (x) => x.Student.Career.Faculty.Name;
                //    break;
                //case "2":
                //    orderByPredicate = (x) => x.StudentId;
                //    break;
                case "0":
                    orderByPredicate = (x) => x.CourseTerm.Course.AcademicYearCourses.Where(y => y.CurriculumId == curriculumId).Select(y => y.AcademicYear).FirstOrDefault();
                    break;
                case "1":
                    orderByPredicate = (x) => x.CourseTerm.Course.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.CourseTerm.Course.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "4":
                    orderByPredicate = (x) => x.CourseTerm.Course.Credits;
                    break;
                default:
                    break;
            }

            var query = _context.Sections
                .Where(x => x.CourseTerm.TermId == termId && x.StudentSections.Any())
                .AsNoTracking();

            //if (careerId.HasValue && careerId != Guid.Empty)
            query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.Curriculum.CareerId == careerId));

            //if (curriculumId.HasValue && curriculumId != Guid.Empty)
            query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (year.HasValue)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.AcademicYear == year));

            //if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            //{
            //    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //    if (!string.IsNullOrEmpty(userId))
            //    {
            //        var qryCareers = _context.Careers
            //            .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
            //            .AsNoTracking();

            //        if (faculty.HasValue && faculty != Guid.Empty)
            //            qryCareers = qryCareers.Where(x => x.FacultyId == faculty);

            //        if (career.HasValue && career != Guid.Empty)
            //            qryCareers = qryCareers.Where(x => x.Id == career);

            //        var careers = qryCareers.Select(x => x.Id).ToHashSet();

            //        query = query.Where(x => careers.Contains(x.Student.CareerId));
            //    }
            //}
            //else
            //{
            //    if (faculty.HasValue && faculty != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == faculty);

            //    if (career.HasValue && career != Guid.Empty) query = query.Where(x => x.Student.CareerId == career);
            //}

            var recordsFiltered = await query.CountAsync();

            var curriculum = await _context.Curriculums
                .Where(x => x.Id == curriculumId)
                .Select(x => new
                {
                    x.Code,
                    Career = x.Career.Name
                }).FirstOrDefaultAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    term = x.CourseTerm.Term.Name,
                    career = curriculum.Career,//x.CourseTerm.Course.CareerId.HasValue ? x.CourseTerm.Course.Career.Name : "-",
                    curriculum = curriculum.Code,//x.CourseTerm.Course.Career.Faculty.Name,
                    code = x.CourseTerm.Course.Code,
                    name = x.CourseTerm.Course.Name,
                    section = x.Code,
                    credits = x.CourseTerm.Course.Credits,
                    academicYear = x.CourseTerm.Course.AcademicYearCourses.Where(y => y.CurriculumId == curriculumId).Select(y => y.AcademicYear).FirstOrDefault(),

                    enrolled = x.StudentSections.Where(y => y.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN
                    && y.Status != ConstantHelpers.STUDENT_SECTION_STATES.DPI && y.Student.CurriculumId == curriculumId).Count(),

                    approved = x.StudentSections.Where(y => y.Status == ConstantHelpers.STUDENT_SECTION_STATES.APPROVED
                        && y.Student.CurriculumId == curriculumId).Count(),

                    disapproved = x.StudentSections.Where(y => y.Status == ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED
                        && y.Student.CurriculumId == curriculumId).Count(),

                    teacher = x.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.FullName).FirstOrDefault(),
                    teacherReport = x.EvaluationReports.Select(y => y.Teacher.User.FullName).FirstOrDefault()
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentSectionsIntranetReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, int? academicYear = null, ClaimsPrincipal user = null)
        {

            Expression<Func<StudentSection, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Section.CourseTerm.Term.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Section.Code;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Section.CourseTerm.Course.Name;
                    break;
            }

            var query = _context.StudentSections
                .Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN &&
                            x.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.FINISHED &&
                            x.Section.CourseTerm.TermId == termId &&
                            x.Student.AcademicSummaries.Any(y => y.TermHasFinished))
                .AsNoTracking();

            if (academicYear != null)
                query = query.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(y => y.AcademicYear == academicYear));

            if (careerId != null)
                query = query.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(y => y.Curriculum.CareerId == careerId));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    sectionId = x.SectionId,
                    studentId = x.StudentId,
                    userName = x.Student.User.UserName,
                    fullName = x.Student.User.FullName,
                    term = x.Section.CourseTerm.Term.Name,
                    section = x.Section.Code,
                    course = x.Section.CourseTerm.Course.Name,
                    finalGrade = x.Student.AcademicHistories
                        .Where(y => y.StudentId == x.StudentId && y.CourseId == x.Section.CourseTerm.CourseId && y.TermId == x.Section.CourseTerm.TermId).FirstOrDefault() == null ?
                        "No se encontró registro" :
                        x.Student.AcademicHistories.Where(y => y.StudentId == x.StudentId && y.CourseId == x.Section.CourseTerm.CourseId && y.TermId == x.Section.CourseTerm.TermId).Select(x => x.Grade.ToString()).FirstOrDefault(),
                    weightedAverageGrade = x.Student.AcademicSummaries
                        .Where(y => y.StudentId == x.StudentId && y.TermId == x.Section.CourseTerm.TermId).FirstOrDefault() == null ?
                        "No se encontró registro" :
                        x.Student.AcademicSummaries.Where(y => y.StudentId == x.StudentId && y.TermId == x.Section.CourseTerm.TermId).Select(x => x.WeightedAverageGrade.ToString()).FirstOrDefault(),
                    totalClassSection = x.Section.Classes.Where(y => y.SectionId == x.SectionId).Count(),
                    attendanceClass = x.Student.ClassStudents.Where(y => y.StudentId == x.StudentId && !y.IsAbsent && y.Class.SectionId == x.SectionId).Count()
                }).ToListAsync();


            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            //Expression<Func<Section, dynamic>> orderByPredicate = null;

            //switch (sentParameters.OrderColumn)
            //{
            //    //case "0":
            //    //    orderByPredicate = (x) => x.CourseTerm.Term.Name;
            //    //    break;
            //    //case "1":
            //    //    orderByPredicate = (x) => x.Student.Career.Faculty.Name;
            //    //    break;
            //    //case "2":
            //    //    orderByPredicate = (x) => x.StudentId;
            //    //    break;
            //    case "0":
            //        orderByPredicate = (x) => x.CourseTerm.Course.AcademicYearCourses.Where(y => y.CurriculumId == curriculumId).Select(y => y.AcademicYear).FirstOrDefault();
            //        break;
            //    case "1":
            //        orderByPredicate = (x) => x.CourseTerm.Course.Code;
            //        break;
            //    case "2":
            //        orderByPredicate = (x) => x.CourseTerm.Course.Name;
            //        break;
            //    case "3":
            //        orderByPredicate = (x) => x.Code;
            //        break;
            //    case "4":
            //        orderByPredicate = (x) => x.CourseTerm.Course.Credits;
            //        break;
            //    default:
            //        break;
            //}

            //var query = _context.Sections
            //    .Where(x => x.CourseTerm.TermId == termId && x.StudentSections.Any())
            //    .AsNoTracking();

            ////if (careerId.HasValue && careerId != Guid.Empty)
            //query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.Curriculum.CareerId == careerId));

            ////if (curriculumId.HasValue && curriculumId != Guid.Empty)
            //query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            //if (year.HasValue)
            //    query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.AcademicYear == year));

            ////if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            ////{
            ////    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ////    if (!string.IsNullOrEmpty(userId))
            ////    {
            ////        var qryCareers = _context.Careers
            ////            .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
            ////            .AsNoTracking();

            ////        if (faculty.HasValue && faculty != Guid.Empty)
            ////            qryCareers = qryCareers.Where(x => x.FacultyId == faculty);

            ////        if (career.HasValue && career != Guid.Empty)
            ////            qryCareers = qryCareers.Where(x => x.Id == career);

            ////        var careers = qryCareers.Select(x => x.Id).ToHashSet();

            ////        query = query.Where(x => careers.Contains(x.Student.CareerId));
            ////    }
            ////}
            ////else
            ////{
            ////    if (faculty.HasValue && faculty != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == faculty);

            ////    if (career.HasValue && career != Guid.Empty) query = query.Where(x => x.Student.CareerId == career);
            ////}

            //var recordsFiltered = await query.CountAsync();

            //var curriculum = await _context.Curriculums
            //    .Where(x => x.Id == curriculumId)
            //    .Select(x => new
            //    {
            //        x.Code,
            //        Career = x.Career.Name
            //    }).FirstOrDefaultAsync();

            //var data = await query
            //    .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
            //    .Skip(sentParameters.PagingFirstRecord)
            //    .Take(sentParameters.RecordsPerDraw)
            //    .Select(x => new
            //    {
            //        term = x.CourseTerm.Term.Name,
            //        career = curriculum.Career,//x.CourseTerm.Course.CareerId.HasValue ? x.CourseTerm.Course.Career.Name : "-",
            //        curriculum = curriculum.Code,//x.CourseTerm.Course.Career.Faculty.Name,
            //        code = x.CourseTerm.Course.Code,
            //        name = x.CourseTerm.Course.Name,
            //        section = x.Code,
            //        credits = x.CourseTerm.Course.Credits,
            //        academicYear = x.CourseTerm.Course.AcademicYearCourses.Where(y => y.CurriculumId == curriculumId).Select(y => y.AcademicYear).FirstOrDefault(),

            //        enrolled = x.StudentSections.Where(y => y.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN
            //        && y.Status != ConstantHelpers.STUDENT_SECTION_STATES.DPI && y.Student.CurriculumId == curriculumId).Count(),

            //        approved = x.StudentSections.Where(y => y.Status == ConstantHelpers.STUDENT_SECTION_STATES.APPROVED
            //            && y.Student.CurriculumId == curriculumId).Count(),

            //        disapproved = x.StudentSections.Where(y => y.Status == ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED
            //            && y.Student.CurriculumId == curriculumId).Count(),

            //        teacher = x.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.FullName).FirstOrDefault(),
            //        teacherReport = x.EvaluationReports.Select(y => y.Teacher.User.FullName).FirstOrDefault()
            //    }).ToListAsync();

            //var recordsTotal = data.Count;

            //return new DataTablesStructs.ReturnedData<object>
            //{
            //    Data = data,
            //    DrawCounter = sentParameters.DrawCounter,
            //    RecordsFiltered = recordsFiltered,
            //    RecordsTotal = recordsFiltered
            //};
        }
        public async Task<List<SectionApprovedDataTemplate>> GetStudentsApprovedDisapprovedBySectionData(Guid termId, Guid careerId, Guid curriculumId, int? year = null, ClaimsPrincipal user = null)
        {
            var query = _context.Sections
                .Where(x => x.CourseTerm.TermId == termId && x.StudentSections.Any())
                .AsNoTracking();

            query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.Curriculum.CareerId == careerId));

            query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (year.HasValue)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.AcademicYear == year));

            //if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            //{
            //    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //    if (!string.IsNullOrEmpty(userId))
            //    {
            //        var qryCareers = _context.Careers
            //            .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
            //            .AsNoTracking();

            //        if (faculty.HasValue && faculty != Guid.Empty)
            //            qryCareers = qryCareers.Where(x => x.FacultyId == faculty);

            //        if (career.HasValue && career != Guid.Empty)
            //            qryCareers = qryCareers.Where(x => x.Id == career);

            //        var careers = qryCareers.Select(x => x.Id).ToHashSet();

            //        query = query.Where(x => careers.Contains(x.Student.CareerId));
            //    }
            //}
            //else
            //{
            //    if (faculty.HasValue && faculty != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == faculty);

            //    if (career.HasValue && career != Guid.Empty) query = query.Where(x => x.Student.CareerId == career);
            //}
            var curriculum = await _context.Curriculums
                .Where(x => x.Id == curriculumId)
                .Select(x => new
                {
                    x.Code,
                    Career = x.Career.Name
                }).FirstOrDefaultAsync();
            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new SectionApprovedDataTemplate
                {
                    Term = x.CourseTerm.Term.Name,
                    Career = curriculum.Career,//x.CourseTerm.Course.CareerId.HasValue ? x.CourseTerm.Course.Career.Name : "-",
                    Curriculum = curriculum.Code,//x.CourseTerm.Course.Career.Faculty.Name,
                    Code = x.CourseTerm.Course.Code,
                    Name = x.CourseTerm.Course.Name,
                    Section = x.Code,
                    Credits = x.CourseTerm.Course.Credits,
                    AcademicYear = x.CourseTerm.Course.AcademicYearCourses.Where(y => y.CurriculumId == curriculumId).Select(y => y.AcademicYear).FirstOrDefault(),

                    Enrolled = x.StudentSections.Where(y => y.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN
                    && y.Status != ConstantHelpers.STUDENT_SECTION_STATES.DPI && y.Student.CurriculumId == curriculumId).Count(),

                    Approved = x.StudentSections.Where(y => y.Status == ConstantHelpers.STUDENT_SECTION_STATES.APPROVED
                        && y.Student.CurriculumId == curriculumId).Count(),

                    Disapproved = x.StudentSections.Where(y => y.Status == ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED
                        && y.Student.CurriculumId == curriculumId).Count(),

                    Teacher = x.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.FullName).FirstOrDefault(),
                    TeacherReport = x.EvaluationReports.Select(y => y.Teacher.User.FullName).FirstOrDefault()
                }).ToListAsync();

            var recordsTotal = data.Count;

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAcademicPerformanceReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            var term = await _context.Terms.FindAsync(termId);
            var enrollmentReservations = await _context.EnrollmentReservations
                .Where(x => x.TermId == termId)
                .Select(x => new
                {
                    x.Student.CareerId
                })
                .ToListAsync();

            var query = _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == term.Id
                && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                .AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (facultyId.HasValue && facultyId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                    if (careerId.HasValue && careerId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var userCareers = qryCareers.Select(x => x.Id).ToHashSet();
                    query = query.Where(x => userCareers.Contains(x.Student.CareerId));
                }
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Student.CareerId == careerId);
            }

            var dataDb = await query
                .Select(x => new
                {
                    CareerCode = x.Student.Career.Code,
                    Career = x.Student.Career.Name,
                    x.Student.CareerId,
                    x.StudentId,
                    x.Status
                }).ToListAsync();

            var students = dataDb
                .GroupBy(x => new { x.StudentId, x.CareerId, x.CareerCode, x.Career })
                .Select(x => new
                {
                    x.Key.StudentId,
                    x.Key.CareerId,
                    x.Key.Career,
                    x.Key.CareerCode,
                    DisappprovedCourses = x.Where(y => y.Status == ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED).Count()
                }).ToList();

            var careers = students
                .GroupBy(x => new { x.CareerId, x.CareerCode, x.Career })
                .Select(x => new
                {
                    x.Key.CareerId,
                    x.Key.Career,
                    x.Key.CareerCode,
                    Total = x.Count() + enrollmentReservations.Where(y => y.CareerId == x.Key.CareerId).Count(),
                    Unbeaten = x.Where(y => y.DisappprovedCourses == 0).Count(),
                    UnbeatenPercentage = Math.Round(x.Where(y => y.DisappprovedCourses == 0).Count() * 100.0 / (x.Count() + enrollmentReservations.Where(y => y.CareerId == x.Key.CareerId).Count()), 2, MidpointRounding.AwayFromZero),
                    OneDisapprovedCourse = x.Where(y => y.DisappprovedCourses == 1).Count(),
                    OneDisapprovedCoursePercentage = Math.Round(x.Where(y => y.DisappprovedCourses == 1).Count() * 100.0 / (x.Count() + enrollmentReservations.Where(y => y.CareerId == x.Key.CareerId).Count()), 2, MidpointRounding.AwayFromZero),
                    TwoDisapprovedCourse = x.Where(y => y.DisappprovedCourses == 2).Count(),
                    TwoDisapprovedCoursePercentage = Math.Round(x.Where(y => y.DisappprovedCourses == 2).Count() * 100.0 / (x.Count() + enrollmentReservations.Where(y => y.CareerId == x.Key.CareerId).Count()), 2, MidpointRounding.AwayFromZero),
                    ThreeDisapprovedCourse = x.Where(y => y.DisappprovedCourses >= 3).Count(),
                    ThreeDisapprovedCoursePercentage = Math.Round(x.Where(y => y.DisappprovedCourses >= 3).Count() * 100.0 / (x.Count() + enrollmentReservations.Where(y => y.CareerId == x.Key.CareerId).Count()), 2, MidpointRounding.AwayFromZero),
                    Reserve = enrollmentReservations.Where(y => y.CareerId == x.Key.CareerId).Count(),
                    ReservePercentage = Math.Round(enrollmentReservations.Where(y => y.CareerId == x.Key.CareerId).Count() * 100.0 / (x.Count() + enrollmentReservations.Where(y => y.CareerId == x.Key.CareerId).Count()), 2, MidpointRounding.AwayFromZero),
                }).ToList();

            var recordsFiltered = careers.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = careers,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<CareerAcademicPerformanceTemplate>> GetAcademicPerformanceReportData(Guid termId, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            var term = await _context.Terms.FindAsync(termId);
            var enrollmentReservations = await _context.EnrollmentReservations
                .Where(x => x.TermId == termId)
                .Select(x => new
                {
                    x.Student.CareerId
                })
                .ToListAsync();

            var query = _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == term.Id
                && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                .AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (facultyId.HasValue && facultyId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                    if (careerId.HasValue && careerId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var userCareers = qryCareers.Select(x => x.Id).ToHashSet();
                    query = query.Where(x => userCareers.Contains(x.Student.CareerId));
                }
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Student.CareerId == careerId);
            }

            var dataDb = await query
                .Select(x => new
                {
                    CareerCode = x.Student.Career.Code,
                    Career = x.Student.Career.Name,
                    x.Student.CareerId,
                    x.StudentId,
                    x.Status
                }).ToListAsync();

            var students = dataDb
                .GroupBy(x => new { x.StudentId, x.CareerId, x.CareerCode, x.Career })
                .Select(x => new
                {
                    x.Key.StudentId,
                    x.Key.CareerId,
                    x.Key.Career,
                    x.Key.CareerCode,
                    DisappprovedCourses = x.Where(y => y.Status == ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED).Count()
                }).ToList();

            var careers = students
                .GroupBy(x => new { x.CareerId, x.CareerCode, x.Career })
                .Select(x => new CareerAcademicPerformanceTemplate
                {
                    Id = x.Key.CareerId,
                    Code = x.Key.Career,
                    Career = x.Key.CareerCode,
                    Total = x.Count() + enrollmentReservations.Where(y => y.CareerId == x.Key.CareerId).Count(),
                    Unbeaten = x.Where(y => y.DisappprovedCourses == 0).Count(),
                    UnbeatenPercentage = Math.Round(x.Where(y => y.DisappprovedCourses == 0).Count() * 100.0 / (x.Count() + enrollmentReservations.Where(y => y.CareerId == x.Key.CareerId).Count()), 2, MidpointRounding.AwayFromZero),
                    OneDisapprovedCourse = x.Where(y => y.DisappprovedCourses == 1).Count(),
                    OneDisapprovedCoursePercentage = Math.Round(x.Where(y => y.DisappprovedCourses == 1).Count() * 100.0 / (x.Count() + enrollmentReservations.Where(y => y.CareerId == x.Key.CareerId).Count()), 2, MidpointRounding.AwayFromZero),
                    TwoDisapprovedCourse = x.Where(y => y.DisappprovedCourses == 2).Count(),
                    TwoDisapprovedCoursePercentage = Math.Round(x.Where(y => y.DisappprovedCourses == 2).Count() * 100.0 / (x.Count() + enrollmentReservations.Where(y => y.CareerId == x.Key.CareerId).Count()), 2, MidpointRounding.AwayFromZero),
                    ThreeDisapprovedCourse = x.Where(y => y.DisappprovedCourses >= 3).Count(),
                    ThreeDisapprovedCoursePercentage = Math.Round(x.Where(y => y.DisappprovedCourses >= 3).Count() * 100.0 / (x.Count() + enrollmentReservations.Where(y => y.CareerId == x.Key.CareerId).Count()), 2, MidpointRounding.AwayFromZero),
                    Reserve = enrollmentReservations.Where(y => y.CareerId == x.Key.CareerId).Count(),
                    ReservePercentage = Math.Round(enrollmentReservations.Where(y => y.CareerId == x.Key.CareerId).Count() * 100.0 / (x.Count() + enrollmentReservations.Where(y => y.CareerId == x.Key.CareerId).Count()), 2, MidpointRounding.AwayFromZero),
                }).ToList();

            return careers;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDisapprovedStudentsDetailedReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termid, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null, byte? studentTry = null)
        {
            Expression<Func<StudentSection, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Student.Career.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.Curriculum.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Section.CourseTerm.Course.AcademicYearCourses.FirstOrDefault().AcademicYear;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Section.CourseTerm.Course.Code;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Section.CourseTerm.Course.Name;
                    break;
                case "6":
                    orderByPredicate = (x) => x.Try;
                    break;
                case "7":
                    orderByPredicate = (x) => x.Section.CourseTerm.Term.Name;
                    break;
                default:
                    //orderByPredicate = (x) => x.Student.User.FullName;
                    break;
            }

            var query = _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == termid && x.Try > 1)
                .AsNoTracking();

            if (studentTry.HasValue)
            {
                if (studentTry == 99)
                {
                    query = query.Where(x => x.Try > 5);
                }
                else
                {
                    query = query.Where(x => x.Try == studentTry);
                }
            }

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (facultyId.HasValue && facultyId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                    if (careerId.HasValue && careerId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var userCareers = qryCareers.Select(x => x.Id).ToHashSet();
                    query = query.Where(x => userCareers.Contains(x.Student.CareerId));
                }
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Student.CareerId == careerId);
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    career = x.Student.Career.Name,
                    curriculum = x.Student.Curriculum.Code,
                    username = x.Student.User.UserName,
                    fullname = x.Student.User.FullName,
                    year = x.Section.CourseTerm.Course.AcademicYearCourses.Where(y => y.CurriculumId == x.Student.CurriculumId).Select(y => y.AcademicYear).FirstOrDefault(),
                    code = x.Section.CourseTerm.Course.Code,
                    course = x.Section.CourseTerm.Course.Name,
                    time = x.Try,
                    term = x.Section.CourseTerm.Term.Name,
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<DisapprovedStudentSectionTemplate>> GetDisapprovedStudentsDetailedReportData(Guid termid, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null, byte? studentTry = null)
        {
            var query = _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == termid && x.Try > 1)
                .AsNoTracking();

            if (studentTry.HasValue)
            {
                if (studentTry == 99)
                {
                    query = query.Where(x => x.Try > 5);
                }
                else
                {
                    query = query.Where(x => x.Try == studentTry);
                }
            }


            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (facultyId.HasValue && facultyId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                    if (careerId.HasValue && careerId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var userCareers = qryCareers.Select(x => x.Id).ToHashSet();
                    query = query.Where(x => userCareers.Contains(x.Student.CareerId));
                }
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Student.CareerId == careerId);
            }

            var data = await query
                .Select(x => new DisapprovedStudentSectionTemplate
                {
                    Career = x.Student.Career.Name,
                    Curriculum = x.Student.Curriculum.Code,
                    Username = x.Student.User.UserName,
                    Fullname = x.Student.User.FullName,
                    Year = x.Section.CourseTerm.Course.AcademicYearCourses.Where(y => y.CurriculumId == x.Student.CurriculumId).Select(y => y.AcademicYear).FirstOrDefault(),
                    Code = x.Section.CourseTerm.Course.Code,
                    Course = x.Section.CourseTerm.Course.Name,
                    Try = x.Try,
                    Term = x.Section.CourseTerm.Term.Name,
                })
                .ToListAsync();

            return data;
        }

        public async Task<List<StudentSection>> GetByStudentAndTerm(Guid termId, Guid studentId/*, bool? isDirectedCourse = false*/)
        {
            var model = await _context.StudentSections
                .Where(ss => ss.Section.CourseTerm.TermId == termId && ss.StudentId == studentId/* && ss.Section.IsDirectedCourse == isDirectedCourse*/)
                .ToListAsync();

            return model;
        }

        public async Task<double> GetStudentHigherAbsencePercentage(Guid studentId, Guid termId)
        {
            var term = await _context.Terms.FindAsync(termId);

            var studentSections = await _context.StudentSections
                .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termId)
                .Select(x => new
                {
                    x.SectionGroupId,
                    x.SectionId
                })
                .ToListAsync();

            var totalClasses = await _context.Classes
                .Where(x => x.Section.StudentSections.Any(y => y.StudentId == studentId) && x.Section.CourseTerm.TermId == termId)
                .Select(x => new
                {
                    x.SectionId,
                    x.ClassSchedule.SectionGroupId,
                })
                .ToListAsync();

            var totalClassStudents = await _context.ClassStudents
                .Where(x => x.Class.Section.CourseTerm.TermId == termId && x.StudentId == studentId)
                .Select(x => new
                {
                    x.Class.SectionId,
                    x.IsAbsent
                })
                .ToListAsync();

            var higherPercentage = 0.0;
            foreach (var item in studentSections)
            {
                var classes = totalClasses.Where(x => x.SectionId == item.SectionId && (!x.SectionGroupId.HasValue || x.SectionGroupId == item.SectionGroupId)).Count();
                var absence = totalClassStudents.Where(x => x.SectionId == item.SectionId && x.IsAbsent).Count();

                var percentage = Math.Round((absence * 100.0) / (classes * 1.0), 2, MidpointRounding.AwayFromZero);
                if (percentage > higherPercentage)
                    higherPercentage = percentage;
            }

            return higherPercentage;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDisapprovedStudentsByUnitReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId, Guid? curriculumId, byte? academicYear, byte unit)
        {
            //Expression<Func<StudentSection, dynamic>> orderByPredicate = null;

            //switch (sentParameters.OrderColumn)
            //{
            //    case "0":
            //        orderByPredicate = (x) => x.Student.Career.Name;
            //        break;
            //    case "1":
            //        orderByPredicate = (x) => x.Student.Curriculum.Code;
            //        break;
            //    case "2":
            //        orderByPredicate = (x) => x.Student.User.UserName;
            //        break;
            //    case "3":
            //        orderByPredicate = (x) => x.Section.CourseTerm.Course.AcademicYearCourses.FirstOrDefault().AcademicYear;
            //        break;
            //    case "4":
            //        orderByPredicate = (x) => x.Section.CourseTerm.Course.Code;
            //        break;
            //    case "5":
            //        orderByPredicate = (x) => x.Section.CourseTerm.Course.Name;
            //        break;
            //    case "6":
            //        orderByPredicate = (x) => x.Try;
            //        break;
            //    case "7":
            //        orderByPredicate = (x) => x.Section.CourseTerm.Term.Name;
            //        break;
            //    default:
            //        //orderByPredicate = (x) => x.Student.User.FullName;
            //        break;
            //}

            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            var query = _context.Grades
                .Where(x => x.StudentSection.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE
                && x.EvaluationId.HasValue && x.Evaluation.CourseUnitId.HasValue && x.Evaluation.CourseUnit.Number == unit)
                .AsNoTracking();

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.StudentSection.Student.CareerId == careerId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.StudentSection.Student.CurriculumId == curriculumId);

            if (academicYear.HasValue && academicYear > 0)
                query = query.Where(x => x.StudentSection.Student.CurrentAcademicYear == academicYear);

            var grades = await query
                .Select(x => new
                {
                    x.StudentSectionId,
                    x.StudentSection.Student.User.UserName,
                    x.StudentSection.Student.User.FullName,
                    x.Value,
                    x.Evaluation.Percentage,
                    Unit = x.Evaluation.CourseUnit.Number
                })
                .ToListAsync();

            var studentCourses = grades
                .GroupBy(x => new { x.UserName, x.FullName, x.StudentSectionId, x.Unit })
                .Select(x => new
                {
                    x.Key.UserName,
                    x.Key.FullName,
                    x.Key.StudentSectionId,
                    x.Key.Unit,
                    grade = x.Sum(y => y.Value * y.Percentage) / (1.0M * x.Sum(y => y.Percentage))
                }).ToList();

            var data = studentCourses
                .Where(x => x.grade < term.MinGrade)
                .GroupBy(x => new { x.UserName, x.FullName, x.Unit })
                .Select(x => new
                {
                    code = x.Key.UserName,
                    name = x.Key.FullName,
                    unit = x.Key.Unit,
                    count = x.Count(),
                }).ToList();

            //var data = await query
            //    .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
            //    .Skip(sentParameters.PagingFirstRecord)
            //    .Take(sentParameters.RecordsPerDraw).Select(x => new
            //    {
            //        career = x.Student.Career.Name,
            //        curriculum = x.Student.Curriculum.Code,
            //        username = x.Student.User.UserName,
            //        fullname = x.Student.User.FullName,
            //        year = x.Section.CourseTerm.Course.AcademicYearCourses.Where(y => y.CurriculumId == x.Student.CurriculumId).Select(y => y.AcademicYear).FirstOrDefault(),
            //        code = x.Section.CourseTerm.Course.Code,
            //        course = x.Section.CourseTerm.Course.Name,
            //        time = x.Try,
            //        term = x.Section.CourseTerm.Term.Name,
            //    })
            //    .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentAveragePerUnitDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid careerId, int unitNumber, string search)
        {

            var query = _context.StudentSections.Where(x => x.Section.CourseTerm.TermId == termId)
                .AsNoTracking();

            query = query.Where(x => x.Student.CareerId == careerId);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Student.User.UserName,
                    x.Student.User.FullName,
                    course = $"{x.Section.CourseTerm.Course.Code} - {x.Section.CourseTerm.Course.Name}",
                    career = x.Student.Career.Name,
                    totalEvaluations = _context.Evaluations.Where(y => y.CourseTermId == x.Section.CourseTermId && y.CourseUnit.Number == unitNumber).Count(),
                    average = Math.Round(x.Grades.Where(y => y.Evaluation.CourseUnit.Number == unitNumber).Sum(y => y.Value * y.Evaluation.Percentage / 100M), 2, MidpointRounding.AwayFromZero),
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };

        }
    }
}
