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
    public class ClassRescheduleRepository : Repository<ClassReschedule>, IClassRescheduleRepository
    {
        public ClassRescheduleRepository(AkdemicContext context) : base(context)
        {
        }

        public Task<bool> AnyByClass(Guid classId, int? status = null)
        {
            var query = _context.ClassReschedules.Where(x => x.ClassId == classId)
                .AsNoTracking();

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            return query.AnyAsync();
        }

        public async Task<IEnumerable<ClassReschedule>> GetAll(string userId = null, int? status = null)
        {
            var query = _context.ClassReschedules
                .Include(x => x.Class.ClassSchedule.Section.CourseTerm.Course)
                .Include(x => x.User)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(x => x.UserId == userId);

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetClassRescheduleDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user , int? status = null, string startSearchDate = null, string endSearchDate = null, string search = null)
        {
            Expression<Func<ClassReschedule, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "1":
                    orderByPredicate = ((x) => x.Class.StartTime); break;
                case "2":
                    orderByPredicate = ((x) => x.StartDateTime); break;
                case "3":
                    orderByPredicate = ((x) => x.Class.EndTime); break;
                case "4":
                    orderByPredicate = ((x) => x.EndDateTime); break;
                case "5":
                    orderByPredicate = ((x) => x.CreatedAt); break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt); break;

            }

            var query = _context.ClassReschedules.Where(x=>x.Class.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE).AsNoTracking();

            if(user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR))
                {
                    query = query.Where(x => x.Class.Section.CourseTerm.Course.Career.CareerDirectorId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
                {
                    query = query.Where(x => x.Class.Section.TeacherSections.Any(y => y.Teacher.AcademicDepartment.AcademicDepartmentDirectorId == userId));
                }
            }

            if (status.HasValue) query = query.Where(x => x.Status == status.Value);

            if (!string.IsNullOrEmpty(startSearchDate))
            {
                var startDate = ConvertHelpers.DatepickerToUtcDateTime(startSearchDate);
                query = query.Where(x => x.CreatedAt >= startDate);
            }

            if (!string.IsNullOrEmpty(endSearchDate))
            {
                var endDate = ConvertHelpers.DatepickerToUtcDateTime(endSearchDate);
                query = query.Where(x => x.CreatedAt <= endDate);
            }

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Class.Section.CourseTerm.Course.Name.ToLower().Contains(search.ToLower().Trim()) || x.Class.Section.CourseTerm.Course.Code.ToLower().Contains(search.ToLower().Trim()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    course = $"{x.Class.ClassSchedule.Section.CourseTerm.Course.Code}-{x.Class.ClassSchedule.Section.CourseTerm.Course.Name}",
                    section = x.Class.ClassSchedule.Section.Code,
                    x.User.FullName,
                    StartTime = x.Class.StartTime.ToLocalDateTimeFormat(),
                    StartDateTime = x.StartDateTime.ToLocalDateFormat(),
                    EndTime = x.Class.EndTime.ToLocalDateFormat(),
                    EndDateTime = x.EndDateTime.ToLocalDateFormat(),
                    CreatedAt = x.CreatedAt.Value.ToLocalDateFormat(),
                    DateCreatedAt = x.CreatedAt.Value,
                    isPermanent = x.Replicate ? "Sí" : "No",
                    AcademicYearsCourses = x.Class.Section.CourseTerm.Course.AcademicYearCourses.Select(y => new
                    {
                        y.Curriculum.Career.CareerDirectorId,
                        y.Curriculum.Career.AcademicDepartmentDirectorId
                    }).ToList()
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
        public async Task<object> GetClassReschedule(Guid id)
        {
            return await _context.ClassReschedules
                .Select(x => new
                {
                    x.Id,
                    FullName = x.User.FullName,
                    StartTime = x.Class.StartTime.ToLocalDateTimeFormat(),
                    StartDateTime = x.StartDateTime.ToLocalDateTimeFormat(),
                    EndTime = x.Class.EndTime.ToLocalDateTimeFormat(),
                    EndDateTime = x.EndDateTime.ToLocalDateTimeFormat(),
                    CreatedAt = x.CreatedAt.Value.ToLocalDateFormat(),
                    isPermanent = x.Replicate ? "Sí" : "No",
                    course = x.Class.Section.CourseTerm.Course.FullName,
                    section = x.Class.Section.Code,
                })
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
