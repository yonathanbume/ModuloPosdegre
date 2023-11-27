
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.InterestGroupForum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Implementations
{
    public class InterestGroupForumRepository : Repository<InterestGroupForum>, IInterestGroupForumRepository
    {
        public InterestGroupForumRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Expression<Func<ForumReportTemplate, dynamic>> GetForumReportDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.ParticipantName);
                case "1":
                    return ((x) => x.CreatedForums);
                case "2":
                    return ((x) => x.ParticipantName);
                default:
                    return ((x) => x.ParticipantName);
            }
        }

        private Func<ForumReportTemplate, string[]> GetForumReportDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.ParticipantName+"",
                x.CreatedForums+"",
                x.ParticipantName+"",
            };
        }

        private Expression<Func<ForumReportTemplate, ForumReportTemplate>> ForumReport()
        {
            return (x) => new ForumReportTemplate
            {
                ParticipantName = x.ParticipantName,
                CreatedForums = x.CreatedForums,
                PaticipationInForums = x.PaticipationInForums,
            };
        }

        private async Task<DataTablesStructs.ReturnedData<ForumReportTemplate>> GetForumReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId, Expression<Func<ForumReportTemplate, ForumReportTemplate>> selectPredicate = null,
            Expression<Func<ForumReportTemplate, dynamic>> orderByPredicate = null, Func<ForumReportTemplate, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var data = await GetForumReportData(interestGroupId);

            var recordsFiltered = 0;
            recordsFiltered = data.Count();
            var result = data.Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

            var recordsTotal = result.Count();
            return new DataTablesStructs.ReturnedData<ForumReportTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

      
        #endregion

        public async Task<DataTablesStructs.ReturnedData<ForumReportTemplate>> GetForumReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId, string searchValue = null)
        {
            return await GetForumReportDatatable(sentParameters, interestGroupId, ForumReport(), GetForumReportDatatableOrderByPredicate(sentParameters), GetForumReportDatatableSearchValuePredicate(), searchValue);
        }

        public async Task<IEnumerable<InterestGroupForum>> GetInterestGroupForumsByInterestGroupId(Guid interestGroupId)
        {
            return await _context.InterestGroupForums.Where(x => x.InterestGroupId == interestGroupId).Include(x=>x.Forum).ToArrayAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<InterestGroupForum>> GetInterestGroupForumDatatable(DataTablesStructs.SentParameters sentParameters,Guid? academicProgramId = null,string searchValue = null,string userAdminId = null)
        {
            Expression<Func<InterestGroupForum, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Forum.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.InterestGroup.AcademicProgram.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Forum.Active);
                    break;
                default:
                    orderByPredicate = ((x) => x.Forum.CreatedAt);
                    break;
            }

            var query = _context.InterestGroupForums
               .Include(x => x.Forum)
               .Include(x => x.Forum.ForumCareers)
               .Include(x => x.InterestGroup.AcademicProgram)
               .Where(x => x.Forum.System == ConstantHelpers.Solution.InterestGroup)
               .AsQueryable();

            if (academicProgramId.HasValue)
                query = query.Where(x => x.InterestGroup.AcademicProgramId == academicProgramId);

            if (!string.IsNullOrEmpty(userAdminId))
                query = query.Where(x => x.InterestGroup.UserAdminId == userAdminId);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Forum.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            var result = query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            Expression<Func<InterestGroupForum, InterestGroupForum>> selectPredicate = (x) => new InterestGroupForum
            {
                State = x.Forum.Active,
                Description = x.Forum.Description,
                ForumName = x.Forum.Name,
                Id = x.Forum.Id,
                AcademicProgramName = x.InterestGroup.AcademicProgram.Name
            };

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        public async Task<IEnumerable<Forum>> GetForumByRoleAndUserId(string role, string userId)
        {
            var query = _context.InterestGroupForums.Where(x => x.Forum.Active == true &&
                x.Forum.System == ConstantHelpers.Solution.InterestGroup).OrderBy(x => x.Forum.Name).AsQueryable();

            if (role.Equals(ConstantHelpers.ROLES.SUPERADMIN) || role.Equals(ConstantHelpers.ROLES.INTEREST_GROUP_ADMIN))
            {
                return await query.Select(x => x.Forum).ToArrayAsync();
            }
            else if (!role.Equals(ConstantHelpers.ROLES.DEGREE_PROGRAM) && (role.Equals(ConstantHelpers.ROLES.TEACHERS)
                || role.Equals(ConstantHelpers.ROLES.PROGRAM_PARTICIPANT)))
            {
                return await query.Where(x => x.InterestGroup.InterestGroupUsers.Any(y => y.UserId == userId))
                    .Select(x => x.Forum).ToArrayAsync();
            }
            else if (role.Equals(ConstantHelpers.ROLES.DEGREE_PROGRAM))
            {
                return await query.Where(x => x.InterestGroup.UserAdminId == userId).Select(x => x.Forum).ToArrayAsync();
            }

            return null;
        }

        public async Task<IEnumerable<ForumReportTemplate>> GetForumReportData(Guid? interestGroupId)
        {
            var query =await _context.InterestGroupUsers
                .Include(x=>x.User.Topics)
                .Include(x=>x.User.Posts)
                .Where(x => x.InterestGroupId == interestGroupId)
                .Select(x => new
                {
                    x.User,
                    x.UserId,
                })
                .ToListAsync();

            var forums = await _context.InterestGroupForums.Where(x => x.InterestGroupId == interestGroupId).Select(x => x.Forum.Id).ToArrayAsync();

            var topics = await _context.Topics.Where(x => forums.Contains(x.ForumId)).ToArrayAsync();
            
          
            var forumReportList = query.Select(z => new ForumReportTemplate
            {
                CreatedForums = z.User.Topics.Where(x => forums.Contains(x.ForumId)).Count(),
                ParticipantName = z.User.FullName,//users.Teacher.User.FullName,
                PaticipationInForums = z.User.Posts.Where(x => topics.Any(y => y.Id == x.TopicId)).Count() == 0 ? 0 :
                            z.User.Posts.Where(x => topics.Any(y => y.Id == x.TopicId)).Count() - 1,
            }).ToList();

            return forumReportList;
        }

        public async Task<InterestGroupForum> GetByForumId(Guid forumId)
            => await _context.InterestGroupForums.Where(x => x.ForumId == forumId).FirstOrDefaultAsync();
    }
}
