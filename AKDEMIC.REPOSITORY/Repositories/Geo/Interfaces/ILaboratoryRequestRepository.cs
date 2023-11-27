using AKDEMIC.ENTITIES.Models.Geo;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Geo.Templates;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Geo.Interfaces
{
    public interface ILaboratoryRequestRepository : IRepository<LaboratoyRequest>
    {
        Task<ATSTemplate> GetATS(Guid id);
        Task<object> GetRequestsByUser(string userId);
    }
}
