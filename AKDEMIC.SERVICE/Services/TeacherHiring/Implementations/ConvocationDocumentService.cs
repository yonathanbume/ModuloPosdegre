using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeacherHiring;
using AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Interfaces;
using AKDEMIC.SERVICE.Services.TeacherHiring.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeacherHiring.Implementations
{
    public class ConvocationDocumentService : IConvocationDocumentService
    {
        private readonly IConvocationDocumentRepository _convocationDocumentRepository;

        public ConvocationDocumentService(IConvocationDocumentRepository convocationDocumentRepository)
        {
            _convocationDocumentRepository = convocationDocumentRepository;
        }

        public async Task Delete(ConvocationDocument entity)
            => await _convocationDocumentRepository.Delete(entity);

        public async Task<ConvocationDocument> Get(Guid id)
            => await _convocationDocumentRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid convocationId, string search)
            => await _convocationDocumentRepository.GetDatatable(parameters, convocationId, search);

        public async Task<IEnumerable<ConvocationDocument>> GetDocuments(Guid convocationId)
            => await _convocationDocumentRepository.GetDocuments(convocationId);

        public async Task Insert(ConvocationDocument entity)
            => await _convocationDocumentRepository.Insert(entity);

        public async Task Update(ConvocationDocument entity)
            => await _convocationDocumentRepository.Update(entity);
    }
}
