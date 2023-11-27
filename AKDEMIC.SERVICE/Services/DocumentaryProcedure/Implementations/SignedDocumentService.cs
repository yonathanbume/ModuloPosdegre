using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class SignedDocumentService : ISignedDocumentService
    {
        private readonly ISignedDocumentRepository _signedDocumentRepository;
        public SignedDocumentService(ISignedDocumentRepository signedDocumentRepository)
        {
            _signedDocumentRepository = signedDocumentRepository;
        }

        public async Task DeleteById(Guid id) => await _signedDocumentRepository.DeleteById(id);

        public async Task<SignedDocument> Get(Guid id)
            => await _signedDocumentRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetSignedDocumentsDatatable(DataTablesStructs.SentParameters sentParameters, string userId = null)
            => await _signedDocumentRepository.GetSignedDocumentsDatatable(sentParameters, userId);

        public async Task<SignedDocument> GetUnsignedDocument(string userId)
            => await _signedDocumentRepository.GetUnsignedDocument(userId);

        public async Task Insert(SignedDocument document) => await _signedDocumentRepository.Insert(document);

        public async Task Update(SignedDocument document) => await _signedDocumentRepository.Update(document);
    }
}