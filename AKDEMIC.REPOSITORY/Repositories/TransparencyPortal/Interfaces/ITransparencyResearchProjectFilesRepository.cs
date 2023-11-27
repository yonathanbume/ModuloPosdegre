using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces
{
    public interface ITransparencyResearchProjectFilesRepository : IRepository<TransparencyResearchProjectFile>
    {
        Task<List<TransparencyResearchProjectFile>> GetByTransparencyResearchProjectId(Guid id);
    }
}
