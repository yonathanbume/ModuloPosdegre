using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class ConceptHistoryService: IConceptHistoryService
    {
        private readonly IConceptHistoryRepository _conceptHistoryRepository;

        public ConceptHistoryService(IConceptHistoryRepository conceptHistoryRepository)
        {
            _conceptHistoryRepository = conceptHistoryRepository;
        }

        public Task Add(ConceptHistory conceptHistory)
            => _conceptHistoryRepository.Add(conceptHistory);

        public Task<ConceptHistory> GetLastChangeByConceptId(Guid conceptId)
            => _conceptHistoryRepository.GetLastChangeByConceptId(conceptId);

        public Task Insert(ConceptHistory conceptHistory)
            => _conceptHistoryRepository.Insert(conceptHistory);

        public Task<DataTablesStructs.ReturnedData<object>> GetConceptHistoryDatatable(DataTablesStructs.SentParameters sentParameters, Guid conceptId)
            => _conceptHistoryRepository.GetConceptHistoryDatatable(sentParameters,conceptId);

    }
}
