using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class ExecuteObservationService : IExecuteObservationService
    {
        private readonly IExecuteObservationRepository _executeObservationRepository;
        public ExecuteObservationService(IExecuteObservationRepository executeObservationRepository)
        {
            _executeObservationRepository = executeObservationRepository;
        }
        public async Task AddAsync(ExecuteObservation executeObservation)
            => await _executeObservationRepository.Add(executeObservation);
        public async Task<ExecuteObservation> Get(Guid id)
            => await _executeObservationRepository.Get(id);
        public async Task<object> GetExecutionObs(Guid id)
            => await _executeObservationRepository.GetExecutionObs(id);
        public async Task<DataTablesStructs.ReturnedData<object>> GetFiles(DataTablesStructs.SentParameters sentParameters, Guid id)
            => await _executeObservationRepository.GetFiles(sentParameters, id);
    }
}
