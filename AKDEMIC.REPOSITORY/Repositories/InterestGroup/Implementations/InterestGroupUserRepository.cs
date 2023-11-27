using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.InterestGroupUsersTemplate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Implementations
{
    public class InterestGroupUserRepository : Repository<InterestGroupUser>, IInterestGroupUserRepository
    {

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<InterestGroupUser>> GetInterestGroupUserDatatable(DataTablesStructs.SentParameters sentParameters,Expression<Func<InterestGroupUser,InterestGroupUser>> selectPredicate = null,Expression<Func<InterestGroupUser,dynamic>> orderByPredicate = null,Func<InterestGroupUser,string[]> searchValuePredicate = null
            ,Guid? interestGroupId = null,string searchValue=null)
        {
            var query = _context.InterestGroupUsers
                .WhereSearchValue(searchValuePredicate, searchValue)
                .AsQueryable();

            if (interestGroupId.HasValue)
                query = query.Where(x => x.InterestGroupId == interestGroupId);

            query = query.AsNoTracking();

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC 

        public InterestGroupUserRepository(AkdemicContext context) : base(context) { }

        public async Task<InterestGroupUser> GetInterestGroupUserByUserId(string userId)
            => await _context.InterestGroupUsers.Where(x => x.UserId == userId).FirstOrDefaultAsync();

        public async Task<IEnumerable<InterestGroupUser>> GetInterestGroupUsersByInterestGroupId(Guid interestGroupId)
        {
            var users = await _context.InterestGroupUsers.Where(x => x.InterestGroupId == interestGroupId)
                .Select(x => new InterestGroupUser
                {
                    InterestGroupId = x.InterestGroupId,
                    UserId = x.UserId,
                    UserFullName = x.User.FullName,
                    UserEmail = x.User.Email
                })
                .ToArrayAsync();

            return users;
        }

        public async Task<IEnumerable<InterestGroupUserTemplate>> GetInterestGroupUsersBySurveyId(Guid id)
        {
            return await _context.InterestGroupUsers
                   .Where(x => x.InterestGroup.InterestGroupSurveys.Any(y => y.SurveyId == id))
                   .Select(x => new InterestGroupUserTemplate
                   {
                       Id = x.UserId,
                       Name = x.User.Name,
                       PaternalSurname = x.User.PaternalSurname,
                       MaternalSurname = x.User.MaternalSurname,
                       Email = x.User.Email
                   }).ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByInterestGroupId(Guid interestGroupId)
            => await _context.InterestGroupUsers.Include(x=>x.User).Where(x => x.InterestGroupId == interestGroupId).Select(x => x.User).ToArrayAsync();

        public async Task<DataTablesStructs.ReturnedData<InterestGroupUser>> GetInterestGroupUserDatatable(DataTablesStructs.SentParameters sentParameters,Guid? interestGroupId = null,string searchValue = null)
        {
            Expression<Func<InterestGroupUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.Email);
                    break;
                default:
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
            }

            return await GetInterestGroupUserDatatable(sentParameters, (x) => new InterestGroupUser
            {
                UserId = x.UserId,
                UserEmail = x.User.Email,
                UserFullName = x.User.FullName
            }, orderByPredicate, (x) => new[] { x.User.FullName }, interestGroupId, searchValue);
        }

        #endregion

    }
}
