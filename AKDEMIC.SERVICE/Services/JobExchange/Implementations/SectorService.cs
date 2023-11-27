using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class SectorService : ISectorService
    {
        private readonly ISectorRepository _sectorRepository;

        public SectorService(ISectorRepository sectorRepository)
        {
            _sectorRepository = sectorRepository;
        }

        public async Task Delete(Sector model)
        {
            await _sectorRepository.Delete(model);
        }

        public async Task<Sector> Get(Guid id)
        {
            return await _sectorRepository.Get(id);
        }

        public async Task<IEnumerable<Sector>> GetAll()
        {
            return await _sectorRepository.GetAll();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectorsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _sectorRepository.GetSectorsDatatable(sentParameters, searchValue);
        }

        public async Task<object> GetSectorsSelect2(Guid? sectorId)
        {
           return  await _sectorRepository.GetSectorsSelect2(sectorId);
        }

        public async Task<object> GetSectorsSelect2V2()
        {
            return await _sectorRepository.GetSectorsSelect2V2();
        }

        public async Task Insert(Sector model)
        {
            await _sectorRepository.Insert(model);
        }

        public async Task Update(Sector model)
        {
            await _sectorRepository.Update(model);
        }
    }
}
