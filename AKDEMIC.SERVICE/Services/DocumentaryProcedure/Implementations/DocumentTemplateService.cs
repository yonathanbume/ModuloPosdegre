using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class DocumentTemplateService : IDocumentTemplateService
    {
        private readonly IDocumentTemplateRepository _documentTemplateRepository;
        public DocumentTemplateService(IDocumentTemplateRepository documentTemplateRepository)
        {
            _documentTemplateRepository = documentTemplateRepository;
        }

        public async Task DeleteById(Guid id)
            => await _documentTemplateRepository.DeleteById(id);

        public async Task<IEnumerable<DocumentTemplate>> GetAll()
            => await _documentTemplateRepository.GetAll();

        public async Task<DocumentTemplate> Get(Guid id)
            => await _documentTemplateRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search = null, byte? system = null)
            => await _documentTemplateRepository.GetDatatable(sentParameters, search, system);


        public async Task Insert(DocumentTemplate document)
            => await _documentTemplateRepository.Insert(document);


        public async Task Update(DocumentTemplate document)
            => await _documentTemplateRepository.Update(document);

    }
}
