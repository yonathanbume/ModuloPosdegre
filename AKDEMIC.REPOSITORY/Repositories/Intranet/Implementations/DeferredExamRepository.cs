using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
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
    public class DeferredExamRepository : Repository<DeferredExam>, IDeferredExamRepository
    {
        public DeferredExamRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDeferredExamDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? careerId, Guid? curriculumId, int? academicYear, string search, ClaimsPrincipal user = null)
        {
            if (!termId.HasValue)
                termId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();

            var query = _context.DeferredExams
                .OrderByDescending(x=>x.CreatedAt)
                .Where(x => x.Section.CourseTerm.TermId == termId)
                .AsNoTracking();

            if(user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.TEACHERS))
                {
                    query = query.Where(x => x.Section.TeacherSections.Any(y => y.TeacherId == userId));
                }
            }

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
                    career = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y=>y.Curriculum.Career.Name).FirstOrDefault(),
                    courseCode = x.Section.CourseTerm.Course.Code,
                    courseName = x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code,
                    EvaluationReport = _context.EvaluationReports.Where(y=> y.Type == ConstantHelpers.Intranet.EvaluationReportType.DEFERRED && y.EntityId == x.Id).FirstOrDefault()
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
            => await _context.DeferredExams.AnyAsync(x => x.SectionId == sectionId);

        public async Task<List<StudentSection>> GetStudentSectionsAvailableToDeferredExam(Guid sectionId)
        {
            var minAvgDeferredExam = Convert.ToDecimal(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.MIN_AVG_DEFERRED_EXAM));
            var term = await _context.Sections.Where(x => x.Id == sectionId).Select(x => x.CourseTerm.Term).FirstOrDefaultAsync();
            var section = await _context.Sections.Where(x => x.Id == sectionId).FirstOrDefaultAsync();
            var evaluations = await _context.Evaluations.Where(x => x.CourseTermId == section.CourseTermId).ToListAsync();

            var studentSections = await _context.StudentSections.Where(x => x.SectionId == sectionId && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN && x.Grades.Count() >= evaluations.Count())
                .Where(x=>x.FinalGrade >= minAvgDeferredExam && x.FinalGrade < term.MinGrade)
                .ToListAsync();

            return studentSections;
        }
    }
}
