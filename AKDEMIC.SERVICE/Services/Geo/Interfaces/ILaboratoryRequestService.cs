using AKDEMIC.ENTITIES.Models.Geo;
using AKDEMIC.REPOSITORY.Repositories.Geo.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Geo.Interfaces
{
    public interface ILaboratoryRequestService
    {
        Task InsertLaboratoyRequest(LaboratoyRequest laboratoyRequest);
        Task UpdateLaboratoyRequest(LaboratoyRequest laboratoyRequest);
        Task DeleteLaboratoyRequest(LaboratoyRequest laboratoyRequest);
        Task<LaboratoyRequest> GetLaboratoyRequestById(Guid id);
        Task<IEnumerable<LaboratoyRequest>> GetaLLLaboratoyRequests();
        Task<ATSTemplate> GetATS(Guid id);
        Task<object> GetRequestsByUser(string userId);
    }
}
