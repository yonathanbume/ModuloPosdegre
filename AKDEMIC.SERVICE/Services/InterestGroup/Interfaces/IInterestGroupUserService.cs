using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.InterestGroupUsersTemplate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Interfaces
{
    public interface IInterestGroupUserService
    {
        Task Insert(InterestGroupUser entity);
        Task InsertRange(IEnumerable<InterestGroupUser> entites);
        Task Delete(InterestGroupUser entity);
        Task DeleteRange(IEnumerable<InterestGroupUser> entities);
        Task<IEnumerable<InterestGroupUser>> GetInterestGroupUsersByInterestGroupId(Guid interestGroupId);
        Task<InterestGroupUser> GetInterestGroupUserByUserId(string userId);
        Task<IEnumerable<InterestGroupUserTemplate>> GetInterestGroupUsersBySurveyId(Guid surveyId);
        Task<IEnumerable<ApplicationUser>> GetUsersByInterestGroupId(Guid interestGroupId);
        Task<DataTablesStructs.ReturnedData<InterestGroupUser>> GetInterestGroupUserDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId = null, string searchValue = null);
    }
}
