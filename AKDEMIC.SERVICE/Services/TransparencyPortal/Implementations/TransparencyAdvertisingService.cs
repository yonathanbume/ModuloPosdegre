using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyAdvertisingService : ITransparencyAdvertisingService
    {
        private readonly ITransparencyAdvertisingRepository _transparencyAdvertisingRepository;
        public TransparencyAdvertisingService(ITransparencyAdvertisingRepository transparencyAdvertisingRepository)
        {
            _transparencyAdvertisingRepository = transparencyAdvertisingRepository;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetConciliationActsDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            return await _transparencyAdvertisingRepository.GetConciliationActsDataTable(sentParameters);
        }

        public async Task InsertRange(List<TransparencyAdvertising> part)
        {
            await _transparencyAdvertisingRepository.InsertRange(part);
        }
    }
}
