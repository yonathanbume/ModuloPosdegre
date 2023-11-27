using AKDEMIC.ENTITIES.Models.Portal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ITransparencyResearchProjectService
    {
        Task Insert(TransparencyResearchProject regulation);
        Task<TransparencyResearchProject> Get(Guid id);
        Task Update(TransparencyResearchProject regulation);
        Task DeleteById(Guid id);
        Task<bool> ExistAnyWithName(Guid id, string name);
        Task<IEnumerable<TransparencyResearchProject>> GetAll();
        Task<IEnumerable<TransparencyResearchProject>> GetBySlug(string slug);
    }
}
