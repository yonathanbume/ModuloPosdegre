using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeachingLoadType;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class TeachingLoadTypeRepository : Repository<TeachingLoadType>, ITeachingLoadTypeRepository
    {
        public TeachingLoadTypeRepository(AkdemicContext context) : base(context)
        {
        }

        public Task<TeachingLoadType> GetByName(string name, int? category = null)
        {
            var query = _context.TeachingLoadTypes.AsNoTracking();

            if (category.HasValue)
                query = query.Where(x => x.Category == category.Value);

            return query.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IEnumerable<TeachingLoadType>> GetAll(int? category = null)
        {
            var query = _context.TeachingLoadTypes.AsNoTracking();

            if (category.HasValue)
                query = query.Where(x => x.Category == category.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetSelect2ClientSide(int? category = null, bool? enabled = null)
        {
            var query = _context.TeachingLoadTypes.AsNoTracking();

            if (category.HasValue)
                query = query.Where(x => x.Category == category.Value);

            if (enabled.HasValue && enabled.Value)
                query = query.Where(x => x.Enabled);

            return await query.Select(x => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }).ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachingLoadTypeDatatable(DataTablesStructs.SentParameters parameters, int? category)
        {
            var query = _context.TeachingLoadTypes.AsNoTracking();


            if (category.HasValue)
                query = query.Where(x => x.Category == category.Value);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
              .Take(parameters.RecordsPerDraw)
              .Select(x => new
              {
                  x.Id,
                  x.Name,
                  x.Enabled,
                  category = ConstantHelpers.TEACHING_LOAD.CATEGORY.VALUES[x.Category]
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherLoadTypeReportDatatable(DataTablesStructs.SentParameters parameters,Guid? termId, Guid? teachingLoadTypeId, Guid? academicDepartmentId,string searchValue)
        {
            Expression<Func<NonTeachingLoad, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Teacher.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Teacher.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.TeachingLoadType.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Minutes;
                    break;
                default:
                    orderByPredicate = (x) => x.Teacher.User.UserName;
                    break;
            }

            var query = _context.NonTeachingLoads.AsNoTracking();

            if(!termId.HasValue || termId == Guid.Empty)
            {
                var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
                termId = term.Id;
            }

            query = query.Where(x => x.TermId == termId);

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.Teacher.AcademicDepartmentId == academicDepartmentId);

            if (teachingLoadTypeId.HasValue && teachingLoadTypeId != Guid.Empty)
                query = query.Where(x => x.TeachingLoadTypeId == teachingLoadTypeId);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Teacher.User.FullName.ToLower().Contains(searchValue.Trim().ToLower()) || x.Teacher.User.UserName.ToLower().Contains(searchValue.Trim().ToLower()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    username = x.Teacher.User.UserName,
                    teacher = x.Teacher.User.FullName,
                    teachingLoadType = x.TeachingLoadType.Name,
                    name = x.Name,
                    hours = Math.Round((x.Minutes / 60m), 2, MidpointRounding.AwayFromZero),
                    minutes = x.Minutes,
                    endDate = x.EndDate.ToLocalDateFormat()
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

        public async Task<List<TeachingLoadTypeReport>> GetTeacherLoadTypeReportData(Guid? teachingLoadTypeId)
        { 
            var query = _context.NonTeachingLoads.OrderBy(x=>x.TeacherId).AsNoTracking();

            if (teachingLoadTypeId.HasValue && teachingLoadTypeId != Guid.Empty)
                query = query.Where(x => x.TeachingLoadTypeId == teachingLoadTypeId);

            var data = await query
                .Select(x => new TeachingLoadTypeReport
                {
                    UserName = x.Teacher.User.UserName,
                    FullName = x.Teacher.User.FullName,
                    LoadType = x.TeachingLoadType.Name,
                    Name = x.Name,
                    Hours = Math.Round((x.Minutes / 60m), 2, MidpointRounding.AwayFromZero),
                    EndDate = x.EndDate.ToLocalDateFormat()
                })
                .ToListAsync();

            return data;
        }

        public async Task<List<TeachingLoadTypeReportV2Template>> GetTeacherLoadTypeReportDatatableTemplate(Guid? termId, Guid? teachingLoadTypeId, Guid? academicDepartmentId)
        { 
            var query = _context.NonTeachingLoads.AsNoTracking();

            if (!termId.HasValue || termId == Guid.Empty)
            {
                var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
                termId = term.Id;
            }

            query = query.Where(x => x.TermId == termId);

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.Teacher.AcademicDepartmentId == academicDepartmentId);

            if (teachingLoadTypeId.HasValue && teachingLoadTypeId != Guid.Empty)
                query = query.Where(x => x.TeachingLoadTypeId == teachingLoadTypeId);

            var data = await query
                .Select(x => new TeachingLoadTypeReportV2Template
                {
                    TeacherUserName = x.Teacher.User.UserName,
                    Teacher = x.Teacher.User.FullName,
                    TeachingLoadType = x.TeachingLoadType.Name,
                    Name = x.Name,
                    Hours = Math.Round((x.Minutes / 60m), 2, MidpointRounding.AwayFromZero).ToString("0.00"),
                    EndDate = x.EndDate.ToLocalDateFormat(),
                    StartDate = x.StartDate.ToLocalDateFormat(),

                    Monday = string.Join(", ", x.NonTeachingLoadSchedules.Where(x => x.WeekDay == ConstantHelpers.WEEKDAY.MONDAY).Select(x => $"{x.StartTime.ToLocalDateTimeFormatUtc()}-{x.EndTime.ToLocalDateTimeFormatUtc()}").ToArray()),
                    Tuesday = string.Join(", ", x.NonTeachingLoadSchedules.Where(x => x.WeekDay == ConstantHelpers.WEEKDAY.TUESDAY).Select(x => $"{x.StartTime.ToLocalDateTimeFormatUtc()}-{x.EndTime.ToLocalDateTimeFormatUtc()}").ToArray()),
                    Wednesday = string.Join(", ", x.NonTeachingLoadSchedules.Where(x => x.WeekDay == ConstantHelpers.WEEKDAY.WEDNESDAY).Select(x => $"{x.StartTime.ToLocalDateTimeFormatUtc()}-{x.EndTime.ToLocalDateTimeFormatUtc()}").ToArray()),
                    Thursday = string.Join(", ", x.NonTeachingLoadSchedules.Where(x => x.WeekDay == ConstantHelpers.WEEKDAY.THURSDAY).Select(x => $"{x.StartTime.ToLocalDateTimeFormatUtc()}-{x.EndTime.ToLocalDateTimeFormatUtc()}").ToArray()),
                    Friday = string.Join(", ", x.NonTeachingLoadSchedules.Where(x => x.WeekDay == ConstantHelpers.WEEKDAY.FRIDAY).Select(x => $"{x.StartTime.ToLocalDateTimeFormatUtc()}-{x.EndTime.ToLocalDateTimeFormatUtc()}").ToArray()),
                    Saturday = string.Join(", ", x.NonTeachingLoadSchedules.Where(x => x.WeekDay == ConstantHelpers.WEEKDAY.SATURDAY).Select(x => $"{x.StartTime.ToLocalDateTimeFormatUtc()}-{x.EndTime.ToLocalDateTimeFormatUtc()}").ToArray()),
                })
                .ToListAsync();

            return data;
        }

        public async Task<bool> AnyNonTeachingLoadByTerm(Guid teachingLoadTypeId, Guid termId)
            => await _context.NonTeachingLoads.AnyAsync(x => x.TeachingLoadTypeId == teachingLoadTypeId && x.TermId == termId);       
    }
}
