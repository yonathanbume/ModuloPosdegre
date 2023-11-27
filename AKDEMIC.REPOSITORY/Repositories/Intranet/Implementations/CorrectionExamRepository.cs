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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class CorrectionExamRepository : Repository<CorrectionExam>, ICorrectionExamRepository
    {
        public CorrectionExamRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? careerId, Guid? curriculumId, int? academicYear, string search, ClaimsPrincipal user = null)
        {
            if (!termId.HasValue)
                termId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();

            var query = _context.CorrectionExams
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => x.Section.CourseTerm.TermId == termId)
                .AsNoTracking();

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(y => y.Curriculum.CareerId == careerId));

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (academicYear.HasValue)
                query = query.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(y => y.AcademicYear == academicYear));

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(x => x.Section.CourseTerm.Course.Name.ToLower().Contains(search) || x.Section.CourseTerm.Course.Code.ToLower().Contains(search));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    career = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Career.Name).FirstOrDefault(),
                    courseCode = x.Section.CourseTerm.Course.Code,
                    courseName = x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code,
                    EvaluationReport = _context.EvaluationReports.Where(y=>y.Type == ConstantHelpers.Intranet.EvaluationReportType.CORRECTION_EXAM && y.EntityId == x.Id).FirstOrDefault()
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, string teacherId, string search)
        {
            if (!termId.HasValue)
                termId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();

            var query = _context.CorrectionExams
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => x.Section.CourseTerm.TermId == termId && x.TeacherId == teacherId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(x => x.Section.CourseTerm.Course.Name.ToLower().Contains(search) || x.Section.CourseTerm.Course.Code.ToLower().Contains(search));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    career = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Career.Name).FirstOrDefault(),
                    courseCode = x.Section.CourseTerm.Course.Code,
                    courseName = x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code,
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<bool> AnyBySection(Guid sectionId)
            => await _context.CorrectionExams.AnyAsync(x => x.SectionId == sectionId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentToCorrectionExam(DataTablesStructs.SentParameters parameters, Guid correctionExamId, string search)
        {
            var query = _context.CorrectionExamStudents
              .Where(x => x.CorrectionExamId == correctionExamId)
              .OrderBy(x => x.Student.User.FullName)
              .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.FullName.Trim().ToLower().Contains(search.Trim().ToLower()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    username = x.Student.User.UserName,
                    fullName = x.Student.User.FullName,
                    grade = !x.Grade.HasValue ? "Sin calificar" : x.Grade.ToString(),
                    status = x.Status
                })
                .ToListAsync();

            var recordsTotal = data.Count;

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
