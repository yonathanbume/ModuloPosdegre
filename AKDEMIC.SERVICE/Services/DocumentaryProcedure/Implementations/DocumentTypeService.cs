using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly IDocumentTypeRepository _documentTypeRepository;

        public DocumentTypeService(IDocumentTypeRepository documentTypeRepository)
        {
            _documentTypeRepository = documentTypeRepository;
        }

        public async Task<DocumentType> Get(Guid id)
        {
            return await _documentTypeRepository.Get(id);
        }

        public async Task<IEnumerable<DocumentType>> GetAll()
        {
            return await _documentTypeRepository.GetAll();
        }

        public async Task<IEnumerable<DocumentType>> GetDocumentTypes()
        {
            return await _documentTypeRepository.GetDocumentTypes();
        }

        public async Task<DataTablesStructs.ReturnedData<DocumentType>> GetDocumentTypesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _documentTypeRepository.GetDocumentTypesDatatable(sentParameters, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetDocumentTypesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _documentTypeRepository.GetDocumentTypesSelect2(requestParameters, searchValue);
        }

        public async Task Delete(DocumentType documentType)
        {
            await _documentTypeRepository.Delete(documentType);
        }

        public async Task Insert(DocumentType documentType)
        {
            await _documentTypeRepository.Insert(documentType);
        }

        public async Task Update(DocumentType documentType)
        {
            await _documentTypeRepository.Update(documentType);
        }


        public async Task<Tuple<int, List<DocumentType>>> GetDatatableDocumentTypes(DataTablesStructs.SentParameters sentParameters)
        {
            return await _documentTypeRepository.GetDatatableDocumentTypes(sentParameters);
        }

        public async Task<bool> HasCode(string code)
        {
            return await _documentTypeRepository.HasCode(code);
        }

        public async Task<bool> HasCode(string code, Guid id)
        {
            return await _documentTypeRepository.HasCode(code, id);
        }

        public async Task<bool> HasRelated(Guid id)
        {
            return await _documentTypeRepository.HasRelated(id);
        }

        public async Task<DocumentType> GetByCode(string code)
            => await _documentTypeRepository.GetByCode(code);

        public async Task<DocumentType> GetByName(string name)
            => await _documentTypeRepository.GetByName(name);
    }
}
