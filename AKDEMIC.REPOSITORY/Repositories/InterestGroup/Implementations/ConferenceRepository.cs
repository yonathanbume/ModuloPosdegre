using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Implementations
{
    public class ConferenceRepository : Repository<Conference> , IConferenceRepository
    {
        public ConferenceRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

      
        private async Task<DataTablesStructs.ReturnedData<ConferenceUser>> GetUserConferenceDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<ConferenceUser, ConferenceUser>> selectPredicate = null, Expression<Func<ConferenceUser, dynamic>> orderByPredicate = null, Func<ConferenceUser, string[]> searchValuePredicate = null
            , Guid? interestGroupId = null, string searchValue = null, string userId = null)
        {
            var query = _context.ConferenceUsers.Where(x => x.UserId == userId)
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsQueryable();
            //var conferencesUsers = await _context.ConferenceUsers.Include(x => x.Conference).Include(x => x.Conference.InterestGroup).Where(x => x.UserId == userId).ToArrayAsync();
            //var query = _context.Conferences.Where(x => x.ConferenceUsers.Any(y => conferencesUsers.Any(z => z.ConferenceId == y.ConferenceId)))
            //    .WhereSearchValue(searchValuePredicate, searchValue)
            //    .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Conference.Title.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            if (interestGroupId.HasValue)
                query = query.Where(x => x.Conference.InterestGroupId == interestGroupId);

            query = query.AsNoTracking();

            return await query.ToDataTables(sentParameters, selectPredicate);

        }

        #endregion

        #region PUBLIC

        public async Task<DataTablesStructs.ReturnedData<Conference>> GetConferencesDatatable(DataTablesStructs.SentParameters sentParameters,Guid? interestGroupId = null,string searchValue = null,string userAdminId=null)
        {
            Expression<Func<Conference, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Title);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.StartDateTime);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.StartDateTime);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.InterestGroup.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.HangoutCreatorEmail);
                    break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
            }

            var query = _context.Conferences
               .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Title.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            if (interestGroupId.HasValue)
                query = query.Where(x => x.InterestGroupId == interestGroupId);

            if (!string.IsNullOrEmpty(userAdminId))
                query = query.Where(x => x.InterestGroup.UserAdminId == userAdminId);

            query = query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            Expression<Func<Conference, Conference>> selectPredicate = (x) => new Conference
            {
                Id = x.Id,
                Title = x.Title,
                StartFormattedDate = x.StartDateTime.ToLocalDateFormat(),
                StartFormattedDateTime = x.StartDateTime.ToLocalTimeFormat(),
                StartDateTime = x.StartDateTime,
                InterestGroupName = x.InterestGroup.Name,
                Type = x.Type,
                TypeString = ConstantHelpers.INTEREST_GROUP_CONFERENCE.TYPE.VALUES[x.Type],
                HangoutCreatorEmail = string.IsNullOrEmpty(x.HangoutCreatorEmail) ? "-" : x.HangoutCreatorEmail,
                HangoutLink = x.HangoutLink,
                EndDateTime = x.EndDateTime,
                GoogleEventId = x.GoogleEventId
            };

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        public async Task<DataTablesStructs.ReturnedData<ConferenceUser>> GetUserConferenceDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId = null, string searchValue = null, string userId = null)
        {
            Expression<Func<ConferenceUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Conference.Title);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Conference.StartDateTime);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Conference.EndDateTime);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Conference.InterestGroup.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.Conference.CreatedAt);
                    break;
            }

            return await GetUserConferenceDatatable(sentParameters, (x) => new ConferenceUser
            {
                Conference = new Conference
                {
                    Id = x.Conference.Id,
                    Title = x.Conference.Title,
                    StartFormattedDateTime = x.Conference.StartDateTime.ToLocalTimeFormat(),
                    StartFormattedDate = x.Conference.StartDateTime.ToLocalDateFormat(),
                    InterestGroupName = x.Conference.InterestGroup.Name,
                    EndDateTime=x.Conference.EndDateTime,
                    HangoutLink = x.Conference.HangoutLink,
                    HangoutCreatorEmail = string.IsNullOrEmpty(x.Conference.HangoutCreatorEmail) ? "-" : x.Conference.HangoutCreatorEmail
                },
                Id = x.Id,
            }, orderByPredicate, (x) => new[] {x.Conference.Title }, interestGroupId, searchValue, userId);
        }
        
        public async Task<object> GetVideoConferenceReportChart(Guid? careerId = null, ClaimsPrincipal user = null)
        {
            var query = _context.Conferences.AsQueryable();

            var careersQuery = _context.Careers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.InterestGroup.AcademicProgram.Career.QualityCoordinatorId == userId);
                    careersQuery = careersQuery.Where(x => x.QualityCoordinatorId == userId);
                }
            }

            if (careerId != null)
                query = query.Where(x => x.InterestGroup.AcademicProgram.CareerId == careerId);

            var conferences = await query
                .Select(x => new
                {
                    x.Id,
                    x.InterestGroup.AcademicProgram.CareerId
                }).ToListAsync();

            var careers = await careersQuery
                .Select(x => new
                {
                    x.Id,
                    x.Name
                }).ToListAsync();

            var data = careers
                .Select(x => new
                {
                    x.Name,
                    Count = conferences.Where(y => y.CareerId == x.Id).Count()
                }).ToList();

            var result = new
            {
                categories = data.Select(x => x.Name).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        #endregion
    }


}
