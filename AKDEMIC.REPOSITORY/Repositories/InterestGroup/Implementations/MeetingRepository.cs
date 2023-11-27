using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.Meeting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Implementations
{
    public class MeetingRepository : Repository<Meeting>, IMeetingRepository
    {
        public MeetingRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE
        private Expression<Func<MeetingTemplate, dynamic>> GetMeetingDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.CreatedAt);
                case "1":
                    return ((x) => x.CreatedAt);
                case "2":
                    return ((x) => x.Matter);
                case "3":
                    return ((x) => x.StatusId);
                default:
                    return ((x) => x.CreatedAt);
            }
        }

        private Expression<Func<MeetingReportTemplate, dynamic>> GetMeetingReportDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.ParticipantName);
                case "1":
                    return ((x) => x.MeetingAssistance);
                default:
                    return ((x) => x.ParticipantName);
            }
        }

        private Func<MeetingTemplate, string[]> GetMeetingDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Number+"",
                x.MeetingDate+"",
                x.Matter+"",
                x.StatusId+"",
            };
        }


        private Func<MeetingReportTemplate, string[]> GetMeetingReportDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.ParticipantName+"",
                x.MeetingAssistance+"",
                x.UserId+""
            };
        }

        private Expression<Func<MeetingReportTemplate, MeetingReportTemplate>> MeetingReport()
        {
            return (x) => new MeetingReportTemplate
            {
                MeetingAssistance = x.MeetingAssistance,
                ParticipantName = x.ParticipantName,
                UserId = x.UserId
            };
        }

        private async Task<DataTablesStructs.ReturnedData<MeetingTemplate>> GetMeetingDatatable(
            DataTablesStructs.SentParameters sentParameters, Guid interestGroupId, string date, string matter, string number,
            Expression<Func<MeetingTemplate, MeetingTemplate>> selectPredicate = null,
            Expression<Func<MeetingTemplate, dynamic>> orderByPredicate = null,
            Func<MeetingTemplate, string[]> searchValuePredicate = null)
        {
            var query = _context.Meetings
                .AsQueryable();

            if (interestGroupId != Guid.Empty)
                query = query.Where(x => x.InterestGroupId == interestGroupId);

            if (!string.IsNullOrEmpty(date))
                query = query.Where(x => x.AnnouncementDate == ConvertHelpers.DatepickerToUtcDateTime(date));

            if (!string.IsNullOrEmpty(matter))
                query = query.Where(x => x.Matter.Contains(matter));

            if (!string.IsNullOrEmpty(number))
                query = query.Where(x => x.Code.Contains(number));

            var result = query
                .Select(x => new MeetingTemplate
                {
                    CreatedAt = x.CreatedAt,
                    Id = x.Id,
                    Description = x.Description,
                    Matter = x.Matter,
                    Number = x.Code,
                    MeetingDate = x.AnnouncementDate.ToLocalDateFormat(),
                    StatusId = x.Status,
                })
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            return await result.ToDataTables(sentParameters, selectPredicate);
        }


        private async Task<DataTablesStructs.ReturnedData<MeetingReportTemplate>> GetMeetingReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId, Expression<Func<MeetingReportTemplate, MeetingReportTemplate>> selectPredicate = null,
            Expression<Func<MeetingReportTemplate, dynamic>> orderBÿPredicate = null, Func<MeetingReportTemplate, string[]> searchValuePredicate = null, string searchValue = null)
        {

            var data = await GetMeetingReportData(interestGroupId);

            var tmp = data.ToList();
            var recordsFiltered = tmp.Count();

            var result = tmp
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

            var recordsTotal = result.Count;

            return new DataTablesStructs.ReturnedData<MeetingReportTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        #endregion

        #region PUBLIC
        public async override Task<Meeting> Get(Guid Id)
        {
            return await _context.Meetings
                   .Include(x => x.MeetingFiles)
                   .Include(x=>x.MeetingCriterions)                   
                   .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<MeetingReportTemplate>> GetMeetingReportData(Guid? interestGroupId)
        {
            var dataDB = await _context.MeetingUsers
                .Where(x => x.Meeting.InterestGroupId == interestGroupId && x.Assistance)
                .Include(x => x.User).Include(x => x.User)
                .ToListAsync();

            var result = dataDB
                .GroupBy(x => x.User)
                .Select(x => new MeetingReportTemplate
                {
                    ParticipantName = x.Key.FullName,
                    UserId = x.Key.Id,
                    MeetingAssistance = x.Count()
                }).ToList();

            return result;
        }
        public async Task<bool> ExistCodeByInterestGroupId(Guid MeetingId, string code, Guid? currentInterestGroupId = null)
        {
            var query = _context.Meetings.AsQueryable();

            if (currentInterestGroupId.HasValue)
            {
                var result = await query.Where(x => x.InterestGroupId == MeetingId && x.Id != currentInterestGroupId).AnyAsync(x => x.Code.Trim().ToLower() == code.Trim().ToLower());
                return result;
            }
            else
            {
                var result = await query.Where(x => x.InterestGroupId == MeetingId).AnyAsync(x => x.Code.Trim().ToLower() == code.Trim().ToLower());
                return result;
            }
        }

        public async Task<IEnumerable<Meeting>> GetMeetingsByInterestGroupId(Guid interestGroupId)
        {
            return await _context.Meetings.Where(x => x.InterestGroupId == interestGroupId).ToArrayAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<MeetingTemplate>> GetMeetingsDatatable(DataTablesStructs.SentParameters parameters, Guid interestGroupId, string date, string matter, string number)
        {
            return await GetMeetingDatatable(parameters, interestGroupId, date, matter, number, null, GetMeetingDatatableOrderByPredicate(parameters), GetMeetingDatatableSearchValuePredicate());
        }

        public async Task<int> GetCountByInterestGroupId(Guid interestGroupId)
        {
            return await _context.Meetings.Where(x => x.InterestGroupId == interestGroupId).CountAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<MeetingReportTemplate>> GetMeetingReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicProgramId = null, string searchValue = null)
        {
            return await GetMeetingReportDatatable(sentParameters, academicProgramId, MeetingReport(), GetMeetingReportDatatableOrderByPredicate(sentParameters), GetMeetingReportDatatableSearchValuePredicate(), searchValue);
        }
        
        public async Task<DataTablesStructs.ReturnedData<Meeting>> GetMeetingDetailsByUserIdDatatable(DataTablesStructs.SentParameters sentParameters,string userId)
        {
            var query = _context.Meetings.Where(x => x.MeetingUsers.Any(y => y.UserId == userId && y.Assistance))
                .OrderByDescending(x=>x.AnnouncementDate)
                .AsNoTracking();

            Expression<Func<Meeting, Meeting>> selectPredicate = (x) => new Meeting
            {
                AnnouncementDateFormatted = x.AnnouncementDate.ToLocalDateFormat(),
                Name = x.Name,
                Matter = x.Matter
            };

            return await query.ToDataTables(sentParameters, selectPredicate);

        }
        
        #endregion
    }
}
