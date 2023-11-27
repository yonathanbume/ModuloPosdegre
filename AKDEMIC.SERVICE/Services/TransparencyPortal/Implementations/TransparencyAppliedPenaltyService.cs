using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyAppliedPenaltyService : ITransparencyAppliedPenaltyService
    {
        private readonly ITransparencyAppliedPenaltyRepository _transparencyAppliedPenaltyRepository;
        public TransparencyAppliedPenaltyService(ITransparencyAppliedPenaltyRepository transparencyAppliedPenaltyRepository)
        {
            _transparencyAppliedPenaltyRepository = transparencyAppliedPenaltyRepository;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetConciliationActsDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            return await _transparencyAppliedPenaltyRepository.GetConciliationActsDataTable(sentParameters);
        }

        public async Task InsertRange(List<TransparencyAppliedPenalty> part)
        {
            await _transparencyAppliedPenaltyRepository.InsertRange(part);
        }
    }
}
