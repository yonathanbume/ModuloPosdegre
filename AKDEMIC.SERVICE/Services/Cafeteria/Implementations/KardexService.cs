using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.SERVICE.Services.Cafeteria.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Implementations
{
    public class KardexService : IKardexService
    {
        private readonly IKardexRepository _kardexRepository;
        public KardexService(IKardexRepository kardexRepository)
        {
            _kardexRepository = kardexRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetKardexDataTable(DataTablesStructs.SentParameters sentParameters, int type, DateTime startDate, DateTime endDate, Guid? ProviderId, string search)
        {
            return await _kardexRepository.GetKardexDataTable(sentParameters,type, startDate, endDate, ProviderId, search);
        }

        public async Task AddRange(IEnumerable<CafeteriaKardex> cafeteriaKardexes)
        {
            await _kardexRepository.AddRange(cafeteriaKardexes);
        }

        public async Task UpdateRange(IEnumerable<CafeteriaKardex> cafeteriaKardexes)
        {
            await _kardexRepository.UpdateRange(cafeteriaKardexes);
        }
    }
}
