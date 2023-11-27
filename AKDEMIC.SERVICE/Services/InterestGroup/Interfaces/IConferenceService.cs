using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Interfaces
{
    public interface IConferenceService
    {
        Task<DataTablesStructs.ReturnedData<Conference>> GetConferencesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId = null, string searchValue = null,string userAdminId = null);
        //Task<DataTablesStructs.ReturnedData<Conference>> GetUserConferenceDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId = null, string searchValue = null, string userId = null);
        Task<DataTablesStructs.ReturnedData<ConferenceUser>> GetUserConferenceDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId = null, string searchValue = null, string userId = null);
        Task Insert(Conference conference);
        Task Delete(Conference conference);
        Task<object> GetVideoConferenceReportChart(Guid? careerId = null, ClaimsPrincipal user = null);
        Task<Conference> Get(Guid id);
    }
}
