using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Portal;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface IInstitutionalActivityFilesService
    {
        Task Insert(InstitutionalActivityFile regulation);
        Task<InstitutionalActivityFile> Get(Guid id);
        Task Update(InstitutionalActivityFile regulation);
        Task DeleteById(Guid id);
        Task<List<InstitutionalActivityFile>> GetByInstitutionalActivityId(Guid FinancialStatementId);
    }
}
