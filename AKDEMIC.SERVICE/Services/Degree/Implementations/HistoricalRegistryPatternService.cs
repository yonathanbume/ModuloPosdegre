using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.REPOSITORY.Repositories.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Degree.Implementations
{
    public class HistoricalRegistryPatternService : IHistoricalRegistryPatternService
    {
        private readonly IHistoricalRegistryPatternRepository _historicalRegistryPatternRepository;

        public HistoricalRegistryPatternService(IHistoricalRegistryPatternRepository historicalRegistryPatternRepository)
        {
            _historicalRegistryPatternRepository = historicalRegistryPatternRepository;
        }

        public async Task Delete(HistoricalRegistryPattern entity)
            => await _historicalRegistryPatternRepository.Delete(entity);

        public async Task<HistoricalRegistryPattern> Get(Guid id)
            => await _historicalRegistryPatternRepository.Get(id);
    
        public async Task<DataTablesStructs.ReturnedData<object>> GetHistoricalRegistryPatternsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _historicalRegistryPatternRepository.GetHistoricalRegistryPatternsDatatable(sentParameters, searchValue);

        public async Task Insert(HistoricalRegistryPattern historicalRegistryPattern)
            => await _historicalRegistryPatternRepository.Insert(historicalRegistryPattern);

        public async Task Update(HistoricalRegistryPattern entity)
            => await _historicalRegistryPatternRepository.Update(entity);
    }
}
