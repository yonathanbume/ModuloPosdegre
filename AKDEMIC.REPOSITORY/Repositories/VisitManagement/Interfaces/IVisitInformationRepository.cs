using AKDEMIC.ENTITIES.Models.VisitManagement;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.VisitManagement.Interfaces
{
    public interface IVisitInformationRepository : IRepository<VisitorInformation>
    {
        Task<object> GetVisitsByVisitorsChart();
    }
}
