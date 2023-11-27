using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ResolutiveActs;
using AKDEMIC.REPOSITORY.Repositories.ResolutiveActs.Interfaces;
using AKDEMIC.SERVICE.Services.ResolutiveActs.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ResolutiveActs.Implementations
{
    public class DocumentFileService : IDocumentFileService
    {
        private readonly IDocumentFileRepository _documentFileRepository;

        public DocumentFileService(IDocumentFileRepository documentFileRepository)
        {
            _documentFileRepository = documentFileRepository;
        }

        public async Task<IEnumerable<DocumentFile>> GetDocumentFilesByDocumentId(Guid documentId)
            => await _documentFileRepository.GetDocumentFilesByDocumentId(documentId);

        public async Task<DataTablesStructs.ReturnedData<DocumentFile>> GetDocumentFilesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? documentId,string searchValue = null)
            => await _documentFileRepository.GetDocumentFilesDatatable(sentParameters,documentId,searchValue);

        public async Task DeleteRangeAsync(IEnumerable<DocumentFile> documentFiles)
            => await _documentFileRepository.DeleteRange(documentFiles);

        public async Task DeleteAsync(DocumentFile documentFile)
            => await _documentFileRepository.Delete(documentFile);

        public async Task DeleteByIdAsync(Guid documentFileId)
            => await _documentFileRepository.DeleteById(documentFileId);

        public async Task InsertAsync(DocumentFile documentFile)
            => await _documentFileRepository.Insert(documentFile);

        public async Task<DocumentFile> Get(Guid id)
            => await _documentFileRepository.Get(id);
    }
}
