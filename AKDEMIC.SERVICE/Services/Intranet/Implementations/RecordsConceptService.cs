using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.RecordConcept;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class RecordsConceptService : IRecordsConceptService
    {
        private readonly IRecordsConceptRepository _recordsConceptRepository;

        public RecordsConceptService(IRecordsConceptRepository recordsConceptRepository)
        {
            _recordsConceptRepository = recordsConceptRepository;
        }

        public async Task<RecordConcept> GetRecordConceptByConceptId(Guid conceptId)
            => await _recordsConceptRepository.GetRecordConceptByConceptId(conceptId);

        public async Task<Guid?> GetValueByRecordType(int recordType)
            => await _recordsConceptRepository.GetValueByRecordType(recordType);

        public async Task<string> SaveRecordsConcept(List<RecordConceptSaveTemplate> template)
            => await _recordsConceptRepository.SaveRecordsConcept(template);
    }
}
