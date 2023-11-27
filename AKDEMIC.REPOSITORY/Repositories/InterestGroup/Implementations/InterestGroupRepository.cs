using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.InterestGroup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Implementations
{
    public class InterestGroupRepository : Repository<ENTITIES.Models.InterestGroup.InterestGroup>, IInterestGroupRepository
    {
        public InterestGroupRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Expression<Func<ENTITIES.Models.InterestGroup.InterestGroup, dynamic>> GetInterestGroupDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Name);
                case "1":
                    return ((x) => x.AcademicProgram.Name);
                case "2":
                    return ((x) => x.StartDate);
                case "3":
                    return ((x) => (x.StartDate.Date <= DateTime.UtcNow.Date && x.EndDate.Date >= DateTime.UtcNow.Date));
                default:
                    return ((x) => x.CreatedAt);
            }
        }

        private Expression<Func<ActivityReportTemplate, dynamic>> GetActivityReportDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.ParticipantName);
                case "1":
                    return ((x) => x.NumberOfComments);
                case "2":
                    return ((x) => x.AmountOfAssistance);
                case "3":
                    return ((x) => x.NumberOfSurveysAnswered);
                default:
                    return ((x) => x.ParticipantName);
            }
        }

        private Func<ActivityReportTemplate, string[]> GetActivityReportDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.ParticipantName+"",
                x.NumberOfComments+"",
                x.AmountOfAssistance+"",
                x.NumberOfSurveysAnswered+"",
                x.ParticipantName+""
            };
        }

        private Func<ENTITIES.Models.InterestGroup.InterestGroup, string[]> GetInterestGroupDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Name+"",
                x.AcademicProgram.Name+"",
                x.StartDate+"",
                x.Active+"",
                x.Name+""
            };
        }

        private Expression<Func<ActivityReportTemplate, ActivityReportTemplate>> ActivityReport()
        {
            return (x) => new ActivityReportTemplate
            {
                ParticipantName = x.ParticipantName,
                NumberOfComments = x.NumberOfComments,
                AmountOfAssistance = x.AmountOfAssistance,
                NumberOfSurveysAnswered = x.NumberOfSurveysAnswered
            };
        }

        private async Task<DataTablesStructs.ReturnedData<ActivityReportTemplate>> GetActivityReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId, Expression<Func<ActivityReportTemplate, ActivityReportTemplate>> selectPredicate = null,
            Expression<Func<ActivityReportTemplate, dynamic>> orderByPredicate = null, Func<ActivityReportTemplate, string[]> searchValuePredicate = null, string searchValue = null)
        {
            List<ActivityReportTemplate> data = (await GetActivityReportData(interestGroupId)).ToList();

            var recordsFiltered = 0;
            recordsFiltered = data.Count();
            var result = data.Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

            var recordsTotal = result.Count();
            return new DataTablesStructs.ReturnedData<ActivityReportTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        private async Task<DataTablesStructs.ReturnedData<object>> GetInterestGroupDatatable(DataTablesStructs.SentParameters sentParameters, string name, Guid? academicProgramId, string date, string userId,
            Expression<Func<ENTITIES.Models.InterestGroup.InterestGroup, ENTITIES.Models.InterestGroup.InterestGroup>> selectPredicate = null, Expression<Func<ENTITIES.Models.InterestGroup.InterestGroup, dynamic>> orderByPredicate = null,
            Func<ENTITIES.Models.InterestGroup.InterestGroup, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.InterestGroups
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(searchValue) || x.AcademicProgram.Name.ToLower().Contains(searchValue));
            }

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(x => x.UserAdminId == userId);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.Name.ToLower().ToLower().Contains(name.Trim().ToLower()));

            if (academicProgramId.HasValue)
                query = query.Where(x => x.AcademicProgramId == academicProgramId);

            if (!string.IsNullOrEmpty(date))
                query = query.Where(x => x.StartDate == ConvertHelpers.DatepickerToUtcDateTime(date));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    AcademicProgramName = x.AcademicProgram.Name,
                    StartFormattedDate = x.StartDate.ToLocalDateFormat(),
                    x.Active,
                    members = x.InterestGroupUsers.Count()
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

        private async Task<Select2Structs.ResponseParameters> GetActiveInterestGroupsByForumsSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<ENTITIES.Models.InterestGroup.InterestGroup, Select2Structs.Result>> selectPredicate, Func<ENTITIES.Models.InterestGroup.InterestGroup, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.InterestGroups
                            .Where(x => (x.StartDate.Date <= DateTime.UtcNow.Date && x.EndDate.Date >= DateTime.UtcNow.Date))
                //.WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }
        private async Task<Select2Structs.ResponseParameters> GetActiveInterestGroupsByForumsSelect2(string UserId, Select2Structs.RequestParameters requestParameters, Expression<Func<ENTITIES.Models.InterestGroup.InterestGroup, Select2Structs.Result>> selectPredicate, Func<ENTITIES.Models.InterestGroup.InterestGroup, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.InterestGroups
                           .Where(x => (x.StartDate.Date <= DateTime.UtcNow.Date && x.EndDate.Date >= DateTime.UtcNow.Date) && x.UserAdminId == UserId)
                //.WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        private async Task<Select2Structs.ResponseParameters> GetActiveInterestGroupByUserAdminIdSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<ENTITIES.Models.InterestGroup.InterestGroup, Select2Structs.Result>> selectPredicate, Func<ENTITIES.Models.InterestGroup.InterestGroup, string[]> searchValuePredicate = null, string userAdminId = null, string searchValue = null)
        {
            var query = _context.InterestGroups
                .Where(x => (x.StartDate.Date <= DateTime.UtcNow.Date && x.EndDate.Date >= DateTime.UtcNow.Date) && userAdminId == x.UserAdminId)
                .Include(x => x.AcademicProgram)
                //.WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        private async Task<Select2Structs.ResponseParameters> GetActiveInterestGroupByUserIdSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<ENTITIES.Models.InterestGroup.InterestGroup, Select2Structs.Result>> selectPredicate, Func<ENTITIES.Models.InterestGroup.InterestGroup, string[]> searchValuePredicate = null, string userId = null, string searchValue = null)
        {
            var interestGroups = await _context.InterestGroupUsers.Where(x => x.UserId == userId).Select(x => x.InterestGroupId).ToArrayAsync();

            var query = _context.InterestGroups.Where(x => (x.StartDate.Date <= DateTime.UtcNow.Date && x.EndDate.Date >= DateTime.UtcNow.Date) && interestGroups.Contains(x.Id))
                .Include(x => x.AcademicProgram)
                //.WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        #endregion

        #region PUBLIC
        public async Task<IEnumerable<ActivityReportTemplate>> GetActivityReportData(Guid? interestGroupId)
        {
            var query =await _context.InterestGroupUsers
                .Include(x => x.User.Posts)
                .Where(x => x.InterestGroupId == interestGroupId)
                .Select(x => new
                {
                    x.User,
                    x.UserId,
                })
                .ToListAsync();
            var forums = await _context.InterestGroupForums.Where(x => x.InterestGroupId == interestGroupId).Select(x => x.Forum.Id).ToArrayAsync();
            var topics = await _context.Topics.Where(x => forums.Contains(x.ForumId)).ToArrayAsync();

            var meetings = await _context.Meetings.Include(x => x.MeetingUsers).Where(x => x.InterestGroupId == interestGroupId).ToArrayAsync();

            var surveys = await _context.InterestGroupSurveys.Include(x => x.Survey.SurveyUsers).Where(x => x.InterestGroupId == interestGroupId).Select(x => x.Survey).ToArrayAsync();
            var surveysId = surveys.Select(x => x.Id).ToList();
            var surveyusers = await _context.SurveyUsers.Where(x => surveysId.Contains(x.SurveyId)).ToArrayAsync();

            var data = query.Select(z => new ActivityReportTemplate
            {
                ParticipantName = z.User.FullName,
                NumberOfComments = z.User.Posts.Where(x => topics.Any(y => y.Id == x.TopicId)).Count() == 0 ? 0 :
                                        z.User.Posts.Where(x => topics.Any(y => y.Id == x.TopicId)).Count() - 1,
                AmountOfAssistance = meetings.Where(x => x.MeetingUsers.Any(y => y.Assistance && y.UserId == z.UserId)).Count(),
                NumberOfSurveysAnswered = surveys.Where(y => surveyusers.Any(t => t.UserId == z.UserId && y.System == ConstantHelpers.Solution.InterestGroup)).Count(),
            }).ToList();
            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInterestGroupsDatatable(DataTablesStructs.SentParameters sentParameters, string name = null, Guid? academicProgramId = null, string date = null, string userId = null, string searchValue = null)
        {
            return await GetInterestGroupDatatable(sentParameters, name, academicProgramId, date, userId, null, GetInterestGroupDatatableOrderByPredicate(sentParameters), GetInterestGroupDatatableSearchValuePredicate(), searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<ActivityReportTemplate>> GetActivityReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicProgramId = null, string searchValue = null)
        {
            return await GetActivityReportDatatable(sentParameters, academicProgramId, ActivityReport(), GetActivityReportDatatableOrderByPredicate(sentParameters), GetActivityReportDatatableSearchValuePredicate(), searchValue);
        }

        public async Task<bool> ExistActiveGroupByAcademicProgramId(Guid academicProgramId, DateTime startdate, DateTime endDate)
        {
            return await _context.InterestGroups.AnyAsync(x => x.AcademicProgramId == academicProgramId && (x.EndDate >= startdate || x.StartDate >= startdate));
        }

        public async Task<int> GetCountOfInterestGroupByUserAdminId(string userAdminId)
        {
            return await _context.InterestGroups.Where(x => x.UserAdminId == userAdminId).CountAsync();
        }

        public async Task<Select2Structs.ResponseParameters> GetActiveInterestGroupsByForumsSelect2(Select2Structs.RequestParameters requestParameters, Guid? id, string searchValue = null)
        {
            var foro = (id != Guid.Empty && id != null) ? (await _context.InterestGroupForums.Where(x => x.ForumId == id).FirstOrDefaultAsync())?.InterestGroupId : Guid.Empty;

            return await GetActiveInterestGroupsByForumsSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.AcademicProgram.Name + " - " + x.Name,
                Selected = x.Id == foro,
                //StartDate = $"{x.StartDate.ToDefaultTimeZone():dd-MM-yyyy}",
                //EndDate = $"{x.EndDate.ToDefaultTimeZone():dd-MM-yyyy}",
            }, (x) => new[] { x.AcademicProgram.Name, x.Name }, searchValue);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetActiveInterestGroupsByForumsSelect2ClientSide(Guid? forumId)
        {
            var foro = (forumId != Guid.Empty && forumId != null) ? (await _context.InterestGroupForums.Where(x => x.ForumId == forumId).FirstOrDefaultAsync())?.InterestGroupId : Guid.Empty;

            var result = await _context.InterestGroups
                            .Where(x => (x.StartDate.Date <= DateTime.UtcNow.Date && x.EndDate.Date >= DateTime.UtcNow.Date))
                            .Select(x => new Select2Structs.Result
                            {
                                Id = x.Id,
                                Text = $"{x.AcademicProgram.Name} - {x.Name}",
                                Selected = x.Id == foro
                            })
                            .ToArrayAsync();
            return result;
        }

        public async Task<Select2Structs.ResponseParameters> GetActiveInterestGroupByUserIdSelect2(Select2Structs.RequestParameters requestParameters, string userId, string searchValue = null)
        {
            return await GetActiveInterestGroupByUserIdSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = $"{x.AcademicProgram.Name} - {x.Name}"
            }, (x) => new[] { x.AcademicProgram.Name, x.Name }, userId, searchValue);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetActiveInterestGroupByUserIdSelect2ClientSide(string userId)
        {
            var query = await _context.InterestGroups.Where(x => (x.StartDate.Date <= DateTime.UtcNow.Date && x.EndDate.Date >= DateTime.UtcNow.Date) && x.InterestGroupUsers.Any(y => y.UserId == userId))
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = $"{x.AcademicProgram.Name} - {x.Name}"
                })
                .ToArrayAsync();

            return query;
        }

        public async Task<ApplicationUser> GetUserAdminByInterestGroupId(Guid interestGroupId)
        {
            return await _context.InterestGroups.Where(x => x.Id == interestGroupId).Select(x => x.UserAdmin).FirstOrDefaultAsync();
        }

        public async Task<Select2Structs.ResponseParameters> GetActiveInterestGroupByUserAdminIdSelect2(Select2Structs.RequestParameters requestParameters, string userAdminId, string searchValue = null)
        {
            return await GetActiveInterestGroupByUserAdminIdSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = $"{x.AcademicProgram.Name} - {x.Name}"
            }, (x) => new[] { x.AcademicProgram.Name, x.Name }, userAdminId, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetActiveInterestGroupsByForumsSelect2(Select2Structs.RequestParameters requestParameters, string UserId, Guid? id, string searchValue = null)
        {
            var foro = (id != Guid.Empty && id != null) ? (await _context.InterestGroupForums.Where(x => x.ForumId == id).FirstOrDefaultAsync())?.InterestGroupId : Guid.Empty;

            return await GetActiveInterestGroupsByForumsSelect2(UserId, requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.AcademicProgram.Name + " - " + x.Name,
                Selected = x.Id == foro,
                //StartDate = $"{x.StartDate.ToDefaultTimeZone():dd-MM-yyyy}",
                //EndDate = $"{x.EndDate.ToDefaultTimeZone():dd-MM-yyyy}",
            }, (x) => new[] { x.AcademicProgram.Name, x.Name }, searchValue);
        }

        public async Task<ENTITIES.Models.InterestGroup.InterestGroup> GetAcademicProgramByInterestGroupId(Guid interestGroupId)
        {
            return await _context.InterestGroups.Where(x => x.Id == interestGroupId).Include(x => x.AcademicProgram).ThenInclude(x => x.Career).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ENTITIES.Models.InterestGroup.InterestGroup>> GetActiveInterestGroupsByUserId(string userId)
            => await _context.InterestGroups.Where(x => x.InterestGroupUsers.Any(y => y.UserId == userId) && (x.StartDate.Date <= DateTime.UtcNow.Date && x.EndDate.Date >= DateTime.UtcNow.Date)).OrderByDescending(x => x.CreatedAt).ToArrayAsync();

        public async Task<object> GetSurveyReportChart(Guid interestGroupId)
        {
            var interest = await _context.InterestGroupSurveys.Where(x => x.InterestGroupId == interestGroupId)
                                        .Select(x => new {
                                            y = x.Survey.SurveyUsers.Count(),
                                            Name = x.Survey.Name
                                        }).ToListAsync();

            return interest;
        }

        public async Task<object> GetInterestGroupActiveSelect2()
        {
            var careers = await _context.InterestGroups
                        .Where(x => x.StartDate.Date <= DateTime.UtcNow.Date && x.EndDate.Date >= DateTime.UtcNow.Date)
                        .Select(x => new { Id = $"{x.Id}", Text = x.AcademicProgram.Name + " - " + x.Name, })
                        .ToListAsync();

            return careers;
        }

        #endregion
    }
}
