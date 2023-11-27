using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyConciliationActService : ITransparencyConciliationActService
    {
        private readonly ITransparencyConciliationActRepository _transparencyConciliationActRepository;
        public TransparencyConciliationActService(ITransparencyConciliationActRepository transparencyConciliationActRepository)
        {
            _transparencyConciliationActRepository = transparencyConciliationActRepository;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetConciliationActsDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            return await _transparencyConciliationActRepository.GetConciliationActsDataTable(sentParameters);
        }

        public async Task InsertRange(List<TransparencyConciliationAct> part)
        {
            await _transparencyConciliationActRepository.InsertRange(part);
        }
    }
}
