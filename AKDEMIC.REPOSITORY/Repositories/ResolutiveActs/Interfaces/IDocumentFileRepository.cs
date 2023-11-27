using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ResolutiveActs;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ResolutiveActs.Interfaces
{
    public interface IDocumentFileRepository : IRepository<DocumentFile>
    {
        Task<DataTablesStructs.ReturnedData<DocumentFile>> GetDocumentFilesDatatable(DataTablesStructs.SentParameters sentParameters,Guid? documentId, string searchValue = null);
        Task<IEnumerable<DocumentFile>> GetDocumentFilesByDocumentId(Guid documentId);
    }
}
