using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class RecordHistoryObservationService : IRecordHistoryObservationService
    {
        private readonly IRecordHistoryObservationRepository _recordHistoryObservationRepository;

        public RecordHistoryObservationService(IRecordHistoryObservationRepository recordHistoryObservationRepository)
        {
            _recordHistoryObservationRepository = recordHistoryObservationRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetObservationsDatatableByRecordHistoryId(DataTablesStructs.SentParameters sentParameters, Guid recordHistoryId)
            => await _recordHistoryObservationRepository.GetObservationsDatatableByRecordHistoryId(sentParameters, recordHistoryId);

        public async Task Insert(RecordHistoryObservation entity)
            => await _recordHistoryObservationRepository.Insert(entity);
    }
}
