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
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class StudentAbsenceJustificationRepository : Repository<StudentAbsenceJustification>, IStudentAbsenceJustificationRepository
    {
        public StudentAbsenceJustificationRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> Any(Guid classStudentId, int? status = null)
        {
            var query = _context.StudentAbsenceJustifications
                .Where(x => x.ClassStudentId == classStudentId)
                .AsNoTracking();
            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);
            return await query.AnyAsync();
        }

        public override async Task<StudentAbsenceJustification> Get(Guid id)
            => await _context.StudentAbsenceJustifications
                .Include(x => x.ClassStudent.Class.Section.CourseTerm.Course)
                .Where(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<StudentAbsenceJustification>> GetAll(Guid? termId = null, Guid? studentId = null)
        {
            var query = _context.StudentAbsenceJustifications
                .AsNoTracking();
            if (termId.HasValue)
                query = query.Where(x => x.ClassStudent.Class.Section.CourseTerm.TermId == termId.Value);
            if (studentId.HasValue)
                query = query.Where(x => x.ClassStudent.StudentId == studentId);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<StudentAbsenceJustification>> GetAll(Guid? termId = null, string userId = null)
        {
            var query = _context.StudentAbsenceJustifications.AsNoTracking();

            if (termId.HasValue) query = query.Where(x => x.ClassStudent.Class.Section.CourseTerm.TermId == termId.Value);

            if (!string.IsNullOrEmpty(userId)) query = query.Where(x => x.ClassStudent.Student.UserId == userId);

            return await query.Include(x => x.ClassStudent.Class.Section.CourseTerm.Course).ToListAsync();
        }

        public async Task<IEnumerable<StudentAbsenceJustification>> GetAll(Guid? termId = null)
        {
            var query = _context.StudentAbsenceJustifications
                .AsNoTracking();
            if (termId.HasValue)
                query = query.Where(x => x.ClassStudent.Class.Section.CourseTerm.TermId == termId.Value);
            return await query.ToListAsync();
        }

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string searchValue = null, byte? status = null)
        {
            Expression<Func<StudentAbsenceJustification, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.RegisterDate;
                    break;
                case "1":
                    orderByPredicate = (x) => x.ClassStudent.Student.User.FullName;
                    break;
                default:
                    orderByPredicate = (x) => x.RegisterDate;
                    break;
            }

            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            if (term == null) return EmptyTableObject();

            var query = _context.StudentAbsenceJustifications
                .Where(x => x.ClassStudent.Class.Section.CourseTerm.TermId == term.Id)
                .AsTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.TEACHERS))
                {
                    var sections = _context.TeacherSections.Where(x => x.TeacherId == userId && x.Section.CourseTerm.TermId == term.Id).Select(x => x.SectionId).ToHashSet();
                    query = query.Where(x => sections.Contains(x.ClassStudent.Class.SectionId));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                {
                    query = query.Where(x => x.ClassStudent.Student.UserId == userId);
                }
            }

            if (status.HasValue && status != 0)
                query = query.Where(x => x.Status == status);

            if (!string.IsNullOrEmpty(searchValue)) 
                query = query.Where(x => x.ClassStudent.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()) 
                || x.ClassStudent.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var dbData = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.ClassStudent.Student.User.FullName,
                    code = x.ClassStudent.Student.User.UserName,
                    courseCode = x.ClassStudent.Class.Section.CourseTerm.Course.Code,
                    course = x.ClassStudent.Class.Section.CourseTerm.Course.Name,
                    status = x.Status,
                    file = x.File,
                    description = x.Justification,
                    date = x.RegisterDate,
                    classDate = x.ClassStudent.Class.StartTime,
                    observation = x.Observation,
                    teacher = x.Teacher.User.FullName ?? "-"
                }).ToListAsync();

            var data = dbData
                .Select(x => new
                {
                    x.id,
                    x.name,
                    x.code,
                    course = $"{x.courseCode} - {x.course}",
                    x.status,
                    x.file,
                    x.description,
                    date = $"{x.date.ToDefaultTimeZone():dd/MM/yyyy}",
                    classDate = $"{x.classDate.ToDefaultTimeZone():dd/MM/yyyy}",
                    x.observation
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

        public async Task<object> GetAdminDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string searchValue = null, Guid? termId = null, byte? status = null)
        {
            Expression<Func<StudentAbsenceJustification, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.RegisterDate;
                    break;
                case "1":
                    orderByPredicate = (x) => x.ClassStudent.Student.User.UserName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.ClassStudent.Student.User.FullName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.ClassStudent.Class.Section.CourseTerm.Course.Code;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Status;
                    break;
                default:
                    orderByPredicate = (x) => x.RegisterDate;
                    break;
            }

            var query = _context.StudentAbsenceJustifications
                .AsTracking();

            if(termId.HasValue && termId != Guid.Empty)
            {
                query = query.Where(x => x.ClassStudent.Class.Section.CourseTerm.TermId == termId);
            }
            else
            {
                var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
                if (term == null) return EmptyTableObject();
                query = query.Where(x => x.ClassStudent.Class.Section.CourseTerm.TermId == term.Id);
            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR)) 
                    query = query.Where(x => x.ClassStudent.Class.Section.CourseTerm.Course.AcademicYearCourses.Any(y => y.Curriculum.Career.CareerDirectorId == userId));

            }

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.ClassStudent.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper())
                || x.ClassStudent.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper()));

            if (status.HasValue && status != 0)
                query = query.Where(x => x.Status == status);

            var recordsFiltered = await query.CountAsync();

            var dbData = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.ClassStudent.Student.User.FullName,
                    code = x.ClassStudent.Student.User.UserName,
                    courseCode = x.ClassStudent.Class.Section.CourseTerm.Course.Code,
                    course = x.ClassStudent.Class.Section.CourseTerm.Course.Name,
                    status = x.Status,
                    file = x.File,
                    description = x.Justification,
                    date = x.RegisterDate,
                    teacher = x.Teacher.User.FullName ?? "-",
                    classDate = x.ClassStudent.Class.StartTime,
                    observation = x.Observation
                }).ToListAsync();

            var data = dbData
                .Select(x => new
                {
                    x.id,
                    x.name,
                    x.code,
                    course = $"{x.courseCode} - {x.course}",
                    x.status,
                    x.file,
                    x.description,
                    date = $"{x.date.ToDefaultTimeZone():dd/MM/yyyy}",
                    classDate = $"{x.classDate.ToDefaultTimeZone():dd/MM/yyyy}",
                    x.observation
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

        private DataTablesStructs.ReturnedData<object> EmptyTableObject()
        {
            return new DataTablesStructs.ReturnedData<object>
            {
                Data = new List<string>(),
                DrawCounter = 0,
                RecordsFiltered = 0,
                RecordsTotal = 0
            };
        }
    }
}
