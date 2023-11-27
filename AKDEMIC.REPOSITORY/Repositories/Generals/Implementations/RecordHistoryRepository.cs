using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class RecordHistoryRepository : Repository<RecordHistory>, IRecordHistoryRepository
    {
        public RecordHistoryRepository(AkdemicContext context) : base(context) { }

        //public async Task<IEnumerable<RecordHistory>> GetAllByNameAndDniAndType(string name = null, string dni = null, int? type = null)
        //{
        //    var query = _context.RecordHistories.AsQueryable();
        //    if (!string.IsNullOrEmpty(name))
        //        query = query.Where(x => x.Name.Contains(name));
        //    if (!string.IsNullOrEmpty(dni))
        //        query = query.Where(x => x.Dni.Contains(dni));
        //    if (type.HasValue)
        //        query = query.Where(x => x.Type == type.Value);
        //    return await query.ToListAsync();
        //}

        public async Task<RecordHistory> GetByNumberAndType(int number, int type)
            => await _context.RecordHistories.Where(x => x.Number == number && x.Type == type).FirstOrDefaultAsync();

        public async Task<RecordHistory> GetLatestByType(int type)
            => await _context.RecordHistories.Where(x => x.Type == type).OrderByDescending(x => x.Date).FirstOrDefaultAsync();

        public async Task<int> GetLatestRecordNumberByType(int type)
            => await _context.RecordHistories.Where(x => x.Type == type).OrderByDescending(x => x.Date).Select(x => x.Number).FirstOrDefaultAsync();

        public async Task<int> GetLatestRecordNumberByType(int type,int year)
           => await _context.RecordHistories.Where(x => x.Type == type && x.Date.Year == year).OrderByDescending(x => x.Date).Select(x => x.Number).FirstOrDefaultAsync();

        public async Task<int> GetRecordByTypeAndYear(int type, int year)
            => await _context.RecordHistories.Where(x => x.Type == type && x.Date.Year == year).CountAsync();

        //public override async Task Insert(RecordHistory recordHistory)
        //{
        //    var latestRecordNumber = await GetLatestRecordNumberByType(recordHistory.Type);
        //    recordHistory.Number = latestRecordNumber + 1;
        //    //await _context.RecordHistories.AddAsync(recordHistory);
        //    //await _context.SaveChangesAsync();
        //    await base.Insert(recordHistory);
        //}

        public async Task<object> GetReportQuantityByYearToAcademicRecord(int year)
        {
            var query = _context.RecordHistories.Where(x => x.Date.Year == year).AsQueryable();
            var result = await query.GroupBy(x => x.Type)
                .Select(x => new
                {
                    name = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[x.Key],
                    y = x.Count()
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<object> GetReportStatusByYearToAcademicRecord(int year)
        {
            //var recordHistories =  await _context.RecordHistories.Where(x => x.Date.Year == year).ToArrayAsync();
            //var result = await _context.UserInternalProcedures.Where(x => x.CreatedAt.Value.Year == year && recordHistories.Any(y => y.InternalProcedureId == x.InternalProcedureId))
            //    .GroupBy(x => x.Status)
            //    .Select(x => new
            //    {
            //        name = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ALLVALUES[x.Key],
            //        y = x.Count()
            //    })
            //    .ToArrayAsync();

            //return result;
            return null;
        }

        public async Task<object> GetReportFinishedVsPendingByMonth(int month)
        {
            var recordHistories = await _context.RecordHistories
                .Where(x => x.Date.Month == month &&(x.Status== ConstantHelpers.RECORD_HISTORY_STATUS.GENERATED||x.Status == ConstantHelpers.RECORD_HISTORY_STATUS.FINALIZED))
                .ToListAsync();

           var total = recordHistories.Count();
            var finished = recordHistories.Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.FINALIZED).Count();
            var pending = total - finished;
            var result = new
            {
                finished,
                pending
            };

            return result;
        }

        public async Task<object> GetReportByAcademicRecordChart()
        {
            var usersId = await _context.Users.Where(x => x.UserRoles.Any(y => y.Role.Name == ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                .Select(x=>x.Id).ToArrayAsync();

            var usersDb = await _context.UserInternalProcedures.Where(x => usersId.Contains(x.UserId))
                .Select(x => x.User)
                .ToListAsync();
            var users = usersDb
                .GroupBy(x => x)
                .Select(x => new
                {
                    name = x.Key.FullName,
                    y = x.Count(),
                    drilldown = x.Key.Id
                })
                .ToArray();

            var detailsDb = await _context.UserInternalProcedures.Where(x => usersId.Contains(x.UserId))
                 .Select(x => x.User)
                .ToListAsync();

            var details = detailsDb
               .GroupBy(x => x)
               .Select(x => new ReportByAcademicRecordChartTemplate
               {
                   Name = x.Key.FullName,
                   Id = x.Key.Id
               })
               .ToArray();

            foreach (var item in details)
            {
                var objectDetails = new List<object[]>();
                var totalStatus = await _context.UserInternalProcedures.Where(x => x.User.Id == item.Id)
                    .Select(x=>x.Status).ToArrayAsync();

                var total = totalStatus.Count();
                var finished = totalStatus.Where(x => x == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ACCEPTED).Count();
                var pending = total - finished;

                objectDetails.Add(new object[] { "Total", total });
                objectDetails.Add(new object[] { "Pendientes", pending });
                objectDetails.Add(new object[] { "Finalizado", finished});

                item.Data = objectDetails;
            }

            return new { users, details };
        }

        public async Task<object> GetReportByMonthAndUserIdChart(int month, string userId)
        {
            var today = DateTime.UtcNow;
            //var result = await _context.RecordHistories.Where(x => x.InternalProcedure.CreatedAt.Value.Month == month &&
            //x.InternalProcedure.CreatedAt.Value.Year == today.Year && x.InternalProcedure.UserId == userId)
            //    .GroupBy(x => x.Type)
            //    .Select(x => new
            //    {
            //        name = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[x.Key],
            //        y = x.Count()
            //    })
            //    .ToArrayAsync();
            return null;
        }

        public async Task<IEnumerable<RecordHistory>> GetAllByStudentId(Guid studentId)
            => await _context.RecordHistories.Where(x => x.StudentId == studentId).ToArrayAsync();

        public class ReportByAcademicRecordChartTemplate
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public List<object[]> Data { get; set; }
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRecordHistoriesDataTable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, int? status, string searchValue)
        {
            Expression<Func<RecordHistory, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Student.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.Student.User.FullName); break;
                case "3":
                    orderByPredicate = ((x) => x.Type); break;
                case "4":
                    orderByPredicate = ((x) => x.ReceiptCode); break;
                default:
                    orderByPredicate = ((x) => x.Date); break;
            }

            var query = _context.RecordHistories.AsQueryable();

            if(user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    query = query.Where(x => x.DerivedUserId == userId);
                }
            }

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            //if (!string.IsNullOrEmpty(searchValue))
            //    query = query.Where(x => x.UserProcedure.User.FullName.ToLower().Contains(searchValue.Trim().ToLower()));

            var needData = new List<int>()
            {
                ConstantHelpers.RECORDS.MERITCHART,
                ConstantHelpers.RECORDS.UPPERFIFTH,
                ConstantHelpers.RECORDS.UPPERTHIRD
            };

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(parameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    date = x.Date.ToLocalDateTimeFormat(),
                    id = x.Id,
                    username = x.Student.User.UserName,
                    student = x.Student.User.FullName,
                    code = x.Code,
                    x.ReceiptCode,
                    isTypeBachelor = x.Type == ConstantHelpers.RECORDS.BACHELOR,
                    isTypeJobTitle = x.Type == ConstantHelpers.RECORDS.JOBTITLE,
                    type = ConstantHelpers.RECORDS.VALUES.ContainsKey(x.Type) ? ConstantHelpers.RECORDS.VALUES[x.Type] : "-",
                    status = ConstantHelpers.RECORD_HISTORY_STATUS.VALUES.ContainsKey(x.Status) ? ConstantHelpers.RECORD_HISTORY_STATUS.VALUES[x.Status] : "-",
                    //userProcedureStatus = ConstantHelpers.USER_PROCEDURES.STATUS.VALUES.ContainsKey(x.UserProcedure.Status) ? ConstantHelpers.USER_PROCEDURES.STATUS.VALUES[x.UserProcedure.Status] : "-",
                    needData = needData.Contains(x.Type),
                    needToSaveFile = string.IsNullOrEmpty(x.FileURL),
                    urlFile = x.FileURL,
                    withProcedure = x.Status == ConstantHelpers.RECORD_HISTORY_STATUS.WITH_PROCEDURE
                }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRecordsByStudentDatatable(DataTablesStructs.SentParameters parameters, Guid studentId)
        {
            var query = _context.RecordHistories.Where(x => x.StudentId == studentId).AsNoTracking();

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescending(x=>x.Date)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    x.Code,
                    createdAt = x.Date.ToLocalDateTimeFormat(),
                    type = ConstantHelpers.RECORDS.VALUES.ContainsKey(x.Type) ? ConstantHelpers.RECORDS.VALUES[x.Type] : "-",
                    status = ConstantHelpers.RECORD_HISTORY_STATUS.VALUES.ContainsKey(x.Status) ? ConstantHelpers.RECORD_HISTORY_STATUS.VALUES[x.Status] : "-",
                    withProcedure = x.Status == ConstantHelpers.RECORD_HISTORY_STATUS.WITH_PROCEDURE
                }).ToListAsync();

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
