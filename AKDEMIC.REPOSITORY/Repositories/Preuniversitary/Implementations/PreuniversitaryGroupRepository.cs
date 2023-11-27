using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Implementations
{
    public class PreuniversitaryGroupRepository : Repository<PreuniversitaryGroup>, IPreuniversitaryGroupRepository
    {
        public PreuniversitaryGroupRepository(AkdemicContext context) : base(context) { }

        public override async Task<PreuniversitaryGroup> Get(Guid id)
            => await _context.PreuniversitaryGroups.Where(x => x.Id == id).Include(x => x.PreuniversitaryCourse).FirstOrDefaultAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchValue)
        {
            Expression<Func<PreuniversitaryGroup, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.Code); break;
                case "2":
                    orderByPredicate = ((x) => x.PreuniversitaryCourse.FullName); break;
                case "3":
                    orderByPredicate = ((x) => x.Teacher.RawFullName); break;
                default:
                    orderByPredicate = ((x) => x.Code); break;
            }

            var query = _context.PreuniversitaryGroups
                    .Where(x => x.PreuniversitaryTermId == preuniversitaryTermId)
                    .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Code.ToLower().Contains(searchValue.Trim().ToLower()));

            int recordsFiltered = await query.CountAsync();
            var data = await query
              .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
              .Skip(sentParameters.PagingFirstRecord)
              .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    code = x.Code,
                    course = x.PreuniversitaryCourse.FullName,
                    teacher = x.Teacher.FullName
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

        public async Task<object> GetStudentsByGroupId(Guid groupId)
        {
            var group = await _context.PreuniversitaryGroups
                .Include(x => x.PreuniversitaryTerm)
                .Include(x => x.PreuniversitarySchedules)
                .FirstOrDefaultAsync(x => x.Id == groupId);
            var weeks = Math.Ceiling((group.PreuniversitaryTerm.ClassEndDate - group.PreuniversitaryTerm.ClassStartDate).TotalDays / 7);
            var query = _context.PreuniversitaryUserGroups
                .Where(x => x.PreuniversitaryGroupId == groupId)
                .AsQueryable();
            var result = await query
                .Select(x => new
                {
                    x.Id,
                    userName = x.ApplicationUser.UserName,
                    name = x.ApplicationUser.FullName,
                    total = group.PreuniversitarySchedules.Count() * weeks,
                    maxAbsent = Math.Round(group.PreuniversitarySchedules.Count() * weeks * 0.3).ToString("0.00"),
                    current = x.PreuniversitaryAssistanceStudents.Count(),
                    notAbsent = x.PreuniversitaryAssistanceStudents.Count(pas => !pas.IsAbsent),
                    absent = x.PreuniversitaryAssistanceStudents.Count(pas => pas.IsAbsent),
                    absentPercent = (x.PreuniversitaryAssistanceStudents.Any() ? (x.PreuniversitaryAssistanceStudents.Count(pas => pas.IsAbsent) / Math.Round(x.PreuniversitaryGroup.PreuniversitarySchedules.Count() * weeks) * 100) : 0.00).ToString("0.00") + "%"
                })
                .AsNoTracking()
                .ToListAsync();
            return result;
        }

        public async Task<object> GetTemaries(Guid groupId, bool? status = null)
        {
            var group = await _context.PreuniversitaryGroups
                .Include(x => x.PreuniversitaryTerm)
                .FirstOrDefaultAsync(x => x.Id == groupId);

            var query = _context.PreuniversitaryTemaries
                .Where(x => x.PreuniversitaryCourseId == group.PreuniversitaryCourseId
                    && x.PreuniversitaryTermId == group.PreuniversitaryTermId)
                .Select(x => new
                {
                    id = x.Id,
                    topic = x.Topic,
                    status = x.PreuniversitaryAssistances
                        .Any(y => y.PreuniversitarySchedule.PreuniversitaryGroupId == groupId && y.PreuniversitaryTemaryId == x.Id),
                    date = x.PreuniversitaryAssistances
                        .Where(y => y.PreuniversitarySchedule.PreuniversitaryGroupId == groupId && y.PreuniversitaryTemaryId == x.Id)
                        .Select(y => y.DateTime.ToLocalDateFormat())
                        .FirstOrDefault()
                })
                .AsNoTracking();

            if (status.HasValue)
                query = query.Where(x => x.status == status.Value);

            var result = await query.ToListAsync();
            return result.OrderBy(x => x.date);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportAdvanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchValue = null)
        {
            Expression<Func<PreuniversitaryGroup, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.Code); break;
                case "2":
                    orderByPredicate = ((x) => x.PreuniversitaryCourse.FullName); break;
                case "3":
                    orderByPredicate = ((x) => x.Teacher.RawFullName); break;
                default:
                    orderByPredicate = ((x) => x.Code); break;
            }

            var query = _context.PreuniversitaryGroups
                .Where(x => x.PreuniversitaryTermId == preuniversitaryTermId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Code.Contains(searchValue) || x.PreuniversitaryCourse.Name.Contains(searchValue) || x.PreuniversitaryCourse.Code.Contains(searchValue));

            int recordsFiltered = await query.CountAsync();
            var data = await query
              .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
              .Skip(sentParameters.PagingFirstRecord)
              .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    code = x.Code,
                    course = x.PreuniversitaryCourse.FullName,
                    teacher = x.Teacher.FullName,
                    percentProgress = (x.PreuniversitaryCourse.PreuniversitaryTemaries.Any(pt => pt.PreuniversitaryTermId == x.PreuniversitaryTermId) ? ((1d * x.PreuniversitaryCourse.PreuniversitaryTemaries.Where(pt => pt.PreuniversitaryTermId == x.PreuniversitaryTermId && x.PreuniversitarySchedules.Any(ps => ps.PreuniversitaryAssistances.Any(pa => pa.PreuniversitaryTemaryId == pt.Id))).Count() / (double)x.PreuniversitaryCourse.PreuniversitaryTemaries.Where(pt => pt.PreuniversitaryTermId == x.PreuniversitaryTermId).Count() * 100)) : 0.00).ToString("0.00") + "%"
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

        public async Task<List<PreuniversitaryGroup>> GetAllByTermIdAndCourseId(Guid preuniversitaryTermId, Guid preuniversitaryCourseId)
            => await _context.PreuniversitaryGroups.Where(x => x.PreuniversitaryTermId == preuniversitaryTermId && x.PreuniversitaryCourseId == preuniversitaryCourseId).ToListAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetGroupsDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId,string searchValue = null)
        {
            Expression<Func<PreuniversitaryGroup, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.Code); break;
                case "2":
                    orderByPredicate = ((x) => x.Teacher.RawFullName); break;
                case "3":
                    orderByPredicate = ((x) => x.Capacity); break;
                default:
                    orderByPredicate = ((x) => x.Code); break;
            }
            var preuniversitaryTermAsync = _context.PreuniversitaryTerms.FindAsync(termId);
            var query = _context.PreuniversitaryGroups
                .Where(x => x.PreuniversitaryCourseId == courseId)
                .Where(x => x.PreuniversitaryTermId == termId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Code.Contains(searchValue) ||
                                            x.Capacity.ToString().Contains(searchValue) ||
                                            x.Teacher.Name.Contains(searchValue) ||
                                            x.Teacher.PaternalSurname.Contains(searchValue) ||
                                            x.Teacher.MaternalSurname.Contains(searchValue));

            var preuniversitaryTerm = await preuniversitaryTermAsync;

            int recordsFiltered = await query.CountAsync();
            var data = await query
              .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
              .Skip(sentParameters.PagingFirstRecord)
              .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    code = x.Code,
                    capacity = x.Capacity,
                    teacher = x.Teacher.FullName,
                    editable = !preuniversitaryTerm.IsFinished
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetCourseGroupByTeacherDatatable(DataTablesStructs.SentParameters sentParameters, Guid currentTermId, string teacherId, string searchValue = null)
        {
            var query = _context.PreuniversitaryGroups.Include(x => x.PreuniversitaryCourse.Career)
                .Where(x=>x.PreuniversitaryTermId == currentTermId && x.TeacherId == teacherId)
                .AsQueryable();

            if(!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.PreuniversitaryCourse.Name.Contains(searchValue) ||
                                         x.PreuniversitaryCourse.Career.Name.Contains(searchValue));

            int recordsFiltered = await query.CountAsync();
            var data = await query
              .Skip(sentParameters.PagingFirstRecord)
              .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    courseCode = x.PreuniversitaryCourse.Code,
                    courseName = x.PreuniversitaryCourse.Name,
                    groupCode = x.Code,
                    done = x.PreuniversitaryUserGroups.Where(s => s.IsQualified).Any()
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
    }
}
