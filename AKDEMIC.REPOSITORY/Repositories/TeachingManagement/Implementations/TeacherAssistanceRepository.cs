using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public sealed class TeacherAssistanceRepository : Repository<TeacherAssistance>, ITeacherAssistanceRepository
    {
        public TeacherAssistanceRepository(AkdemicContext context) : base(context) { }

        Task<TeacherAssistance> ITeacherAssistanceRepository.GetByFilter(string userId, DateTime? time)
        {
            var query = _context.TeacherAssistance.AsQueryable();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(x => x.UserId == userId);

            if (time.HasValue)
                query = query.Where(x => x.Time.ToShortDateString() == time.Value.ToShortDateString());

            return query.FirstOrDefaultAsync();
        }
        Task<List<TeacherAssistance>> ITeacherAssistanceRepository.GetByFilter(DateTime? time)
        {
            var query = _context.TeacherAssistance.AsQueryable();

            if (time.HasValue)
                query = query.Where(x => x.Time.ToShortDateString() == time.Value.ToShortDateString());

            return query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<WorkingDay>> GetReport(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? careerId, DateTime? starttime, DateTime? endtime)
        {
            Expression<Func<WorkingDay, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.UserName); break;
                default:
                    orderByPredicate = ((x) => x.User.UserName); break;
            }

            var query = await GetReportData(facultyId, careerId, starttime, endtime);

            int recordsFiltered = query.Count();

            var data = query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<WorkingDay>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<WorkingDay>> GetReportData(Guid? facultyId, Guid? careerId, DateTime? starttime, DateTime? endtime)
        {
            var query = _context.WorkingDays.Include(x => x.User).AsNoTracking();
            var teachers = _context.Teachers.AsQueryable();

            if (facultyId.HasValue)
                query = query.Where(x => teachers.Any(y => y.Career.FacultyId == facultyId.Value && x.UserId == y.UserId));

            if (careerId.HasValue)
                query = query.Where(x => teachers.Any(y => y.CareerId == careerId.Value && x.UserId == y.UserId));

            var result = query.AsEnumerable();

            if (starttime.HasValue && endtime.HasValue)
                result = result.Where(x => x.RegisterDate.ToDefaultTimeZone() >= starttime.Value && x.RegisterDate.ToDefaultTimeZone() <= endtime.Value);
            else if (starttime.HasValue)
                result = result.Where(x => x.RegisterDate.ToDefaultTimeZone() == starttime.Value);
            else if (endtime.HasValue)
                result = result.Where(x => x.RegisterDate.ToDefaultTimeZone() == endtime.Value);

            return result.ToList();
        }
    }
}