using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Portal;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ITransparencyResearchProjectFilesService
    {
        Task Insert(TransparencyResearchProjectFile regulation);
        Task<TransparencyResearchProjectFile> Get(Guid id);
        Task Update(TransparencyResearchProjectFile regulation);
        Task DeleteById(Guid id);
        Task<List<TransparencyResearchProjectFile>> GetByTransparencyResearchProjectId(Guid FinancialStatementId);
    }
}
