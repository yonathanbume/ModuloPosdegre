using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Portal;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ITransparencyPortalGeneralService
    {
        Task Insert(TransparencyPortalGeneral regulation);
        Task<TransparencyPortalGeneral> Get(Guid id);
        Task Update(TransparencyPortalGeneral regulation);
        Task DeleteById(Guid id);
        Task<List<TransparencyPortalGeneral>> GetByType(int type);
        Task<IEnumerable<TransparencyPortalGeneral>> GetAll();
        Task<TransparencyPortalGeneral> GetFirstByType(int type);
    }
}
