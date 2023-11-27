using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces
{
    public interface ITransparencyPortalGeneralRepository:IRepository<TransparencyPortalGeneral>
    {
        Task<List<TransparencyPortalGeneral>> GetByType(int type);
        Task<TransparencyPortalGeneral> GetFirstByType(int type);
    }
}
