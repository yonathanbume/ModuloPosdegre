using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyTravelPassageService : ITransparencyTravelPassageService
    {
        private readonly ITransparencyTravelPassageRepository _transparencyTravelPassageRepository;

        public TransparencyTravelPassageService(ITransparencyTravelPassageRepository transparencyTravelPassageRepository)
        {
            _transparencyTravelPassageRepository = transparencyTravelPassageRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTransparencyTravelPassageDataTable(DataTablesStructs.SentParameters sentParameters)
            => await _transparencyTravelPassageRepository.GetTransparencyTravelPassageDataTable(sentParameters);

        public async Task InsertRange(IEnumerable<TransparencyTravelPassage> entities)
            => await _transparencyTravelPassageRepository.InsertRange(entities);
    }
}
