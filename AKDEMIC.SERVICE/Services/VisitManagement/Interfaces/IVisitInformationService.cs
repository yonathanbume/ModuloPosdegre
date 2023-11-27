using AKDEMIC.ENTITIES.Models.VisitManagement;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.VisitManagement.Interfaces
{
    public interface IVisitInformationService
    {
        Task<VisitorInformation> Get(Guid id);
        Task Delete(VisitorInformation visitorInformation);
        Task<object> GetVisitsByVisitorsChart();
    }
}
