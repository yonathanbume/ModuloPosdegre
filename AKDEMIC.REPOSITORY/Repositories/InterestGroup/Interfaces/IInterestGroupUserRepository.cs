using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.InterestGroupUsersTemplate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces
{
    public interface IInterestGroupUserRepository : IRepository<InterestGroupUser>
    {
        Task<IEnumerable<InterestGroupUser>> GetInterestGroupUsersByInterestGroupId(Guid interestGroupId);
        Task<InterestGroupUser> GetInterestGroupUserByUserId(string userId);
        Task<IEnumerable<ApplicationUser>> GetUsersByInterestGroupId(Guid interestGroupId);
        Task<IEnumerable<InterestGroupUserTemplate>> GetInterestGroupUsersBySurveyId(Guid id);
        Task<DataTablesStructs.ReturnedData<InterestGroupUser>> GetInterestGroupUserDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId = null, string searchValue = null);
    }
}
