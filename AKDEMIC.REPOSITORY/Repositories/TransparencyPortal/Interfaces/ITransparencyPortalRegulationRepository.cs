using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces
{
    public interface ITransparencyPortalRegulationRepository:IRepository<TransparencyPortalRegulation>
    {
        Task<object> GetDataTable(PaginationParameter paginationParameter);
        IQueryable<TransparencyPortalRegulation> GetIQueryable();
    }
}
