using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ResolutiveActs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ResolutiveActs.Interfaces
{
    public interface IDocumentFileService
    {
        Task<DataTablesStructs.ReturnedData<DocumentFile>> GetDocumentFilesDatatable(DataTablesStructs.SentParameters sentParameters,Guid? documentId, string searchValue = null);
        Task<IEnumerable<DocumentFile>> GetDocumentFilesByDocumentId(Guid documentId);
        Task DeleteRangeAsync(IEnumerable<DocumentFile> documentFiles);
        Task DeleteAsync(DocumentFile documentFile);
        Task InsertAsync(DocumentFile documentFile);
        Task DeleteByIdAsync(Guid documentFileId);
        Task<DocumentFile> Get(Guid id);
    }
}
