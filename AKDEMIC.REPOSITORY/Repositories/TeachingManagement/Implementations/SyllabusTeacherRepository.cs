using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.SyllabusTeacher;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public sealed class SyllabusTeacherRepository : Repository<SyllabusTeacher>, ISyllabusTeacherRepository
    {
        public SyllabusTeacherRepository(AkdemicContext context) : base(context) { }

        public async Task<SyllabusTeacher> GetByCourseTermId(Guid courseTermId)
            => await _context.SyllabusTeachers.Where(x => x.CourseTermId == courseTermId).FirstOrDefaultAsync();

        public async Task<object> GetChartJsReport(Guid termId, Guid? facultyId)
        {
            var query = _context.SyllabusTeachers.Where(x => x.CourseTerm.TermId == termId).AsQueryable();

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.Career.FacultyId == facultyId);

            var dataCareer = await query
                .Select(x => x.CourseTerm.Course.Career)
                .ToListAsync();

            var data = dataCareer
                .GroupBy(x => x.Name)
                .Select(x => new
                {
                    x.Key,
                    count = x.Count()
                })
                .ToList();

            var result = new
            {
                Name = "Cant. de Sílabos",
                Data = data.Select(x=> new object[]
                {
                    x.Key,
                    x.count
                }).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSyllabusTeacherDatatable(DataTablesStructs.SentParameters parameters, Guid termId, Guid academicDepartmentId , string teacherSearch, ClaimsPrincipal user = null)
        {
            var teachers = _context.Teachers.Where(x => x.AcademicDepartmentId == academicDepartmentId).Include(x=>x.User).AsNoTracking();

            if (!string.IsNullOrEmpty(teacherSearch))
                teachers = teachers.Where(x => x.User.FullName.Trim().Contains(teacherSearch) || x.User.UserName.Contains(teacherSearch));

            var teachersData = await teachers.Select(x => x.UserId).ToArrayAsync();

            var query = _context.AcademicYearCourses.Where(x => x.Course.CourseTerms.Any(y => y.TermId == termId && teachersData.Contains(y.CoordinatorId)));

            if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR_GENERAL))
            {
                var configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.TeacherManagement.CAREER_DIRECTOR_GENERAL_ACADEMIC_YEAR).FirstOrDefaultAsync();
                if (configuration != null)
                {
                    Int32.TryParse(configuration.Value, out var maxAcademicYear);
                    query = query.Where(x => x.AcademicYear <= maxAcademicYear);
                }
            }

            var syllabusTeachers = _context.SyllabusTeachers.Include(x => x.CourseTerm.Course).Include(x => x.CourseTerm.Term).Where(x => x.SyllabusRequest.TermId == termId);
            var request = await _context.SyllabusRequests.FirstOrDefaultAsync(x => x.TermId == termId);

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    courseName = x.Course.Name,
                    courseCode = x.Course.Code,
                    curriculum = x.Curriculum.Name,
                    curriculumId = x.CurriculumId,
                    courseId = x.CourseId,
                    coordinator = string.Join(", ",x.Course.CourseTerms.Where(x=>x.TermId == termId && !string.IsNullOrEmpty(x.CoordinatorId)).Select(x=>x.Coordinator.FullName).ToList()),
                    onlyDigital = request.Type == ConstantHelpers.SYLLABUS_REQUEST.TYPE.DIGITAL,
                    syllabusStatus = syllabusTeachers.Where(y => y.CourseTerm.CourseId == x.Course.Id && y.CourseTerm.TermId == termId).Select(y => y.Status).FirstOrDefault(),
                    syllabus = syllabusTeachers.Where(y => y.CourseTerm.CourseId == x.Course.Id && y.CourseTerm.TermId == termId).Select(y => new { y.IsDigital, y.Id}).FirstOrDefault()
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetSyllabusTeacherOutOfDateDatatable(DataTablesStructs.SentParameters parameters, Guid termId, Guid? careerId, Guid? curriculumId)
        {
            var syllabusRequest = await _context.SyllabusRequests.Where(x => x.TermId == termId).FirstOrDefaultAsync();
            var query = _context.SyllabusTeachers.Where(x => x.SyllabusRequestId == syllabusRequest.Id && x.Status == ConstantHelpers.SYLLABUS_TEACHER.STATUS.PRESENTED && x.PresentationDate.HasValue && x.PresentationDate.Value.AddHours(-5).Date > syllabusRequest.End.Date).AsNoTracking();

            if (careerId.HasValue)
                query = query.Where(x => x.CourseTerm.Course.CareerId == careerId);

            if (curriculumId.HasValue)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    career = x.CourseTerm.Course.Career.Name,
                    curriculum = string.Join(", ", x.CourseTerm.Course.AcademicYearCourses.Select(x=>x.Curriculum.Code).ToList()),
                    course = $"{x.CourseTerm.Course.Code}-{x.CourseTerm.Course.Name}",
                    teacher = x.Teacher.FullName,
                    presentationDate = x.PresentationDate.ToLocalDateTimeFormat()
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


        public async Task<List<SyllabusTeacherReportTemplate>> GetSyllabusTeacherReport(Guid termId, Guid academicDepartmentId, ClaimsPrincipal user = null)
        {
            var teachers = _context.Teachers.Where(x => x.AcademicDepartmentId == academicDepartmentId).Include(x => x.User).AsNoTracking();
            var teachersData = await teachers.Select(x => x.UserId).ToArrayAsync();
            var query = _context.AcademicYearCourses.Where(x => x.Course.CourseTerms.Any(y => y.TermId == termId && teachersData.Contains(y.CoordinatorId)));

            if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR_GENERAL))
            {
                var configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.TeacherManagement.CAREER_DIRECTOR_GENERAL_ACADEMIC_YEAR).FirstOrDefaultAsync();
                if (configuration != null)
                {
                    Int32.TryParse(configuration.Value, out var maxAcademicYear);
                    query = query.Where(x => x.AcademicYear <= maxAcademicYear);
                }
            }

            var syllabusTeachers = _context.SyllabusTeachers.Include(x => x.CourseTerm.Course).Include(x => x.CourseTerm.Term).Where(x => x.SyllabusRequest.TermId == termId);
            var request = await _context.SyllabusRequests.FirstOrDefaultAsync(x => x.TermId == termId);

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new SyllabusTeacherReportTemplate
                {
                    Coordinator = x.Course.CourseTerms.Where(x => x.TermId == termId && !string.IsNullOrEmpty(x.CoordinatorId)).Select(x => x.Coordinator.FullName).FirstOrDefault(),
                    Course = $"{x.Course.Code}-{x.Course.Name}",
                    Curriculum = x.Curriculum.Code,
                    Status = ConstantHelpers.SYLLABUS_TEACHER.STATUS.VALUES.ContainsKey(syllabusTeachers.Where(y => y.CourseTerm.CourseId == x.Course.Id && y.CourseTerm.TermId == termId).Select(y => y.Status).FirstOrDefault()) ? ConstantHelpers.SYLLABUS_TEACHER.STATUS.VALUES[syllabusTeachers.Where(y => y.CourseTerm.CourseId == x.Course.Id && y.CourseTerm.TermId == termId).Select(y => y.Status).FirstOrDefault()] : "Sin Registrar",
                    UploadDate = syllabusTeachers.Where(y => y.CourseTerm.CourseId == x.Course.Id && y.CourseTerm.TermId == termId).Select(y => y.PresentationDate.ToLocalDateTimeFormat()).FirstOrDefault()
                })
                .ToListAsync();

            return data;
        }

        public async Task<IEnumerable<SyllabusTeacherTemplate>> GetSyllabusTeacher(Guid termId, string teacherId)
        {
            var request = await _context.SyllabusRequests.FirstOrDefaultAsync(x => x.TermId == termId);
            if (request is null)
                return null;

            var query = _context.AcademicYearCourses.Where(x => x.Course.CourseTerms.Any(y => y.TermId == termId && y.CoordinatorId == teacherId));
            var syllabusTeachers = _context.SyllabusTeachers.Include(x => x.CourseTerm.Course).Include(x => x.CourseTerm.Term).Where(x => x.SyllabusRequest.TermId == termId);
            var data = await query
                .Select(x => new SyllabusTeacherTemplate
                {
                    Course = x.Course.Name,
                    Code = x.Course.Code,
                    Curriculum = x.Curriculum.Name,
                    Status = syllabusTeachers.Where(y => y.CourseTerm.CourseId == x.Course.Id && y.CourseTerm.TermId == termId).Select(y => y.Status).FirstOrDefault()
                })
                .ToListAsync();

            return data;

        }

        public async Task<List<SyllabusTeacher>> GetSyllabusTeacherCourses(Guid termId,Guid careerId ,Guid curriculumId)
        {
            var result = await _context.SyllabusTeachers
                .Where(x => x.SyllabusRequest.TermId == termId && x.CourseTerm.Course.CareerId == careerId && x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId))
                .Select(x => new SyllabusTeacher
                {
                    Id = x.Id,
                    Url = x.Url,
                    CourseTerm = new ENTITIES.Models.Enrollment.CourseTerm
                    {
                        Id = x.CourseTerm.Id,
                        CourseId = x.CourseTerm.CourseId,
                        Course = new ENTITIES.Models.Enrollment.Course
                        {
                            Name = x.CourseTerm.Course.Name,
                            Code = x.CourseTerm.Course.Code
                        },
                        TermId = x.CourseTerm.TermId
                    }
                })
                .ToListAsync();

            return result;
        }
    }
}