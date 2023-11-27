using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class GradeRecoveryExamRepository : Repository<GradeRecoveryExam>, IGradeRecoveryExamRepository
    {
        public GradeRecoveryExamRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<GradeRecoveryExam>> GetGradeRecoveryByStudent(Guid studentId, Guid termId)
        {
            var exams = await _context.GradeRecoveryExams
                .Where(x => x.Section.CourseTerm.TermId == termId && x.Section.StudentSections.Any(y => y.StudentId == studentId))
                .Include(x => x.Section.CourseTerm.Course)
                .Include(x => x.Classroom)
                .ToListAsync();

            return exams;
        }

        public async Task<bool> AnyBySection(Guid sectionId)
            => await _context.GradeRecoveryExams.AnyAsync(x => x.SectionId == sectionId);
        public async Task<DataTablesStructs.ReturnedData<object>> GetGradeRecoveryDetailDatatable(DataTablesStructs.SentParameters parameters, Guid? careerId, Guid? curriculumId, int? cycle, Guid? courseId, string searchValue)
        {
            var activeTerm = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
            if (activeTerm == null)
                activeTerm = new ENTITIES.Models.Enrollment.Term();

            Expression<Func<GradeRecoveryExam, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.Career.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.Code); break;
                case "2":
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.Section.Code); break;
                case "4":
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.AcademicYearCourses.Select(x => x.AcademicYear).FirstOrDefault()); break;
                default:
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.Career.Name); break;
            }

            var query = _context.GradeRecoveryExams
                .Where(x => x.Section.CourseTerm.TermId == activeTerm.Id)
                .AsQueryable();

            if (careerId.HasValue)
                query = query.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);

            if (curriculumId.HasValue)
                query = query.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (cycle.HasValue)
                query = query.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(y => y.AcademicYear == cycle));

            if (courseId.HasValue)
                query = query.Where(x => x.Section.CourseTerm.CourseId == courseId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x =>
                    x.Section.CourseTerm.Course.Code.ToLower().Contains(searchValue) ||
                    x.Section.CourseTerm.Course.Name.ToLower().Contains(searchValue));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    career = x.Section.CourseTerm.Course.Career.Name,
                    courseCode = x.Section.CourseTerm.Course.Code,
                    courseName = x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code,
                    academicYear = string.Join(", ", x.Section.CourseTerm.Course.AcademicYearCourses.Select(x =>
                        ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS.ContainsKey(x.AcademicYear) ?
                        ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[x.AcademicYear] : "-"
                        ).ToList()),
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


        public async Task<DataTablesStructs.ReturnedData<object>> GetGradeRecoveryExamByTeacherDatatable(DataTablesStructs.SentParameters parameters, byte? status, string teacherId)
        {
            var activeTerm = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
            if (activeTerm == null)
                activeTerm = new ENTITIES.Models.Enrollment.Term();
            Expression<Func<GradeRecoveryExam, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.Career.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.Code); break;
                case "2":
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.Section.Code); break;
                case "4":
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.AcademicYearCourses.Select(x => x.AcademicYear)); break;
                default:
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.Career.Name); break;
            }

            var query = _context.GradeRecoveryExams
                .Where(x => x.Section.CourseTerm.TermId == activeTerm.Id && x.Section.TeacherSections.Any(y => y.TeacherId == teacherId))
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    career = x.Section.CourseTerm.Course.Career.Name,
                    courseCode = x.Section.CourseTerm.Course.Code,
                    courseName = x.Section.CourseTerm.Course.Name,
                    x.Status,
                    section = x.Section.Code,
                    academicYear = string.Join(", ", x.Section.CourseTerm.Course.AcademicYearCourses.Select(x =>
                        ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS.ContainsKey(x.AcademicYear) ?
                        ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[x.AcademicYear] : "-"
                        ).ToList()),
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

    }
}
