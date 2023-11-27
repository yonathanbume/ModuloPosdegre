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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Implementations
{
    public class MeetingUserRepository : Repository<MeetingUser>, IMeetingUserRepository
    {
        public MeetingUserRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Expression<Func<MeetingUser, dynamic>> GetMeetingUserDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.User.FullName);
                case "1":
                    return ((x) => x.User.FullName);
                case "2":
                    return ((x) => x.User.Email);
                default:
                    return ((x) => x.User.FullName);
            }
        }

        private Func<MeetingUser, string[]> GetMeetingUserDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.User.FullName+"",
                x.User.Email+""
            };
        }

        private async Task<DataTablesStructs.ReturnedData<MeetingUser>> GetMeetingUserDatatable(DataTablesStructs.SentParameters sentParameters,Guid meetingId,Expression<Func<MeetingUser,MeetingUser>> selectPredicate = null,Expression<Func<MeetingUser,dynamic>> orderByPredicate = null,Func<MeetingUser,string[]> searchValuePredicate = null,string searchValue = null)
        {
            var query = _context.MeetingUsers
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsQueryable();

            var result = query
                .Where(x=>x.MeetingId == meetingId)
                .Select(x => new MeetingUser
                {
                    UserId = x.User.Id,
                    UserFullName = x.User.FullName,
                    UserEmail = x.User.Email,
                    Assistance = x.Assistance
                }, searchValue)
                .AsNoTracking();

            return await result.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC 

        public async Task<DataTablesStructs.ReturnedData<MeetingUser>> GetMeetingUsersDatatable(DataTablesStructs.SentParameters sentParameters,Guid meetingId ,string searchValue = null)
            => await GetMeetingUserDatatable(sentParameters,meetingId, null, GetMeetingUserDatatableOrderByPredicate(sentParameters), GetMeetingUserDatatableSearchValuePredicate(), searchValue);

        public async Task<IEnumerable<MeetingUser>> GetMeetingUsersByMeetingId(Guid meetingId)
            => await _context.MeetingUsers.Where(x => x.MeetingId == meetingId).ToArrayAsync();

        #endregion
    }
}
