using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.InterestGroupForum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Interfaces
{
    public interface IInterestGroupForumService
    {
        Task<IEnumerable<InterestGroupForum>> GetInterestGroupForumsByInterestGroupId(Guid interestGroupId);
        Task<DataTablesStructs.ReturnedData<ForumReportTemplate>> GetForumReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<InterestGroupForum>> GetInterestGroupForumDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicProgramId = null, string searchValue = null, string userAdminId = null);

        Task Insert(InterestGroupForum entity);
        Task<IEnumerable<Forum>> GetForumByRoleAndUserId(string role, string userId);
        Task<IEnumerable<ForumReportTemplate>> GetForumReportData(Guid interestGroupId);
        Task<InterestGroupForum> GetByForumId(Guid forumId);

        Task Delete(InterestGroupForum interestGroupForum);
            
    }
}
