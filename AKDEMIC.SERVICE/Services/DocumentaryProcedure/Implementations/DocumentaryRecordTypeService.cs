using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class DocumentaryRecordTypeService : IDocumentaryRecordTypeService
    {
        private readonly IDocumentaryRecordTypeRepository _documentaryRecordTypeRepository;

        public DocumentaryRecordTypeService(IDocumentaryRecordTypeRepository documentaryRecordTypeRepository)
        {
            _documentaryRecordTypeRepository = documentaryRecordTypeRepository;
        }

        public async Task<DocumentaryRecordType> Get(Guid id)
        {
            return await _documentaryRecordTypeRepository.Get(id);
        }

        public async Task<IEnumerable<DocumentaryRecordType>> GetAll()
        {
            return await _documentaryRecordTypeRepository.GetAll();
        }

        public async Task<IEnumerable<DocumentaryRecordType>> GetDocumentaryRecordTypes()
        {
            return await _documentaryRecordTypeRepository.GetDocumentaryRecordTypes();
        }

        public async Task<DataTablesStructs.ReturnedData<DocumentaryRecordType>> GetDocumentaryRecordTypesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _documentaryRecordTypeRepository.GetDocumentaryRecordTypesDatatable(sentParameters, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetDocumentaryRecordTypesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _documentaryRecordTypeRepository.GetDocumentaryRecordTypesSelect2(requestParameters, searchValue);
        }

        public async Task Delete(DocumentaryRecordType documentaryRecordType)
        {
            await _documentaryRecordTypeRepository.Delete(documentaryRecordType);
        }

        public async Task Insert(DocumentaryRecordType documentaryRecordType)
        {
            await _documentaryRecordTypeRepository.Insert(documentaryRecordType);
        }

        public async Task Update(DocumentaryRecordType documentaryRecordType)
        {
            await _documentaryRecordTypeRepository.Update(documentaryRecordType);
        }




















        public async Task<bool> HasRelatedUserProcedures(Guid documentaryRecordTypeId)
        {
            return await _documentaryRecordTypeRepository.HasRelatedUserProcedures(documentaryRecordTypeId);
        }

        public async Task<bool> IsCodeDuplicated(string code)
        {
            return await _documentaryRecordTypeRepository.IsCodeDuplicated(code);
        }

        public async Task<bool> IsCodeDuplicated(string code, Guid documentaryRecordTypeId)
        {
            return await _documentaryRecordTypeRepository.IsCodeDuplicated(code, documentaryRecordTypeId);
        }

        public async Task<Tuple<int, List<DocumentaryRecordType>>> GetDocumentaryRecordTypes(DataTablesStructs.SentParameters sentParameters)
        {
            return await _documentaryRecordTypeRepository.GetDocumentaryRecordTypes(sentParameters);
        }

        public async Task Add(DocumentaryRecordType entity)
            => await _documentaryRecordTypeRepository.Add(entity);
    }
}
