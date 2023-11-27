using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.DocumentFormat;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class DocumentFormatService : IDocumentFormatService
    {
        private readonly IDocumentFormatRepository _documentFormatRepository;

        public DocumentFormatService(IDocumentFormatRepository documentFormatRepository)
        {
            _documentFormatRepository = documentFormatRepository;
        }

        public async Task<bool> AnyByRecordType(byte type)
            => await _documentFormatRepository.AnyByRecordType(type);

        public async Task Delete(ENTITIES.Models.Intranet.DocumentFormat entity)
            => await _documentFormatRepository.Delete(entity);

        public async Task<ENTITIES.Models.Intranet.DocumentFormat> Get(byte id)
            => await _documentFormatRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDocumentFormatsDatatable(DataTablesStructs.SentParameters parameters)
            => await _documentFormatRepository.GetDocumentFormatsDatatable(parameters);

        public async Task<DocumentFormatTemplate> GetParsedDocumentFormat(byte recordType, Guid studentId, Guid? termId = null)
            => await _documentFormatRepository.GetParsedDocumentFormat(recordType, studentId, termId);

        public async Task Insert(ENTITIES.Models.Intranet.DocumentFormat entity)
            => await _documentFormatRepository.Insert(entity);

        public async Task Update(ENTITIES.Models.Intranet.DocumentFormat entity)
            => await _documentFormatRepository.Update(entity);
    }
}
