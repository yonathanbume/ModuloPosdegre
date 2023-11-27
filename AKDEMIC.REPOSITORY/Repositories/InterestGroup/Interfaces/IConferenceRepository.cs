using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces
{
    public interface IConferenceRepository : IRepository<Conference>
    {
        Task<DataTablesStructs.ReturnedData<Conference>> GetConferencesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId = null, string searchValue = null,string userAdminId = null);
        //Task<DataTablesStructs.ReturnedData<Conference>> GetUserConferenceDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId = null, string searchValue = null, string userId = null);
        Task<DataTablesStructs.ReturnedData<ConferenceUser>> GetUserConferenceDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId = null, string searchValue = null, string userId = null);

        Task<object> GetVideoConferenceReportChart(Guid? careerId = null, ClaimsPrincipal user = null);
    }
}
