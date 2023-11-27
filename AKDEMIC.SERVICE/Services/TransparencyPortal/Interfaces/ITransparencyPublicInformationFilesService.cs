using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Portal;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ITransparencyPublicInformationFilesService
    {
        Task Insert(TransparencyPublicInformationFile regulation);
        Task<TransparencyPublicInformationFile> Get(Guid id);
        Task Update(TransparencyPublicInformationFile regulation);
        Task DeleteById(Guid id);
        Task<List<TransparencyPublicInformationFile>> GetByTransparencyPublicInformationId(Guid FinancialStatementId);
    }
}
