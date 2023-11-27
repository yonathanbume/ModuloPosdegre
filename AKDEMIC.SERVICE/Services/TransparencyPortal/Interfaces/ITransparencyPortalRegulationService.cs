using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Portal;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ITransparencyPortalRegulationService
    {
        Task Insert(TransparencyPortalRegulation regulation);
        Task<TransparencyPortalRegulation> Get(Guid id);
        Task Update(TransparencyPortalRegulation regulation);
        Task DeleteById(Guid id);
        Task<object> GetDataTable(PaginationParameter paginationParameter);
        Task<IEnumerable<TransparencyPortalRegulation>> GetAll();
        IQueryable<TransparencyPortalRegulation> GetIQueryable();
    }
}
