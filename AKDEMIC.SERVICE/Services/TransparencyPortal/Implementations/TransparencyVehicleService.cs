using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyVehicleService : ITransparencyVehicleService
    {
        private readonly ITransparencyVehicleRepository _transparencyVehicleRepository;

        public TransparencyVehicleService(ITransparencyVehicleRepository transparencyVehicleRepository)
        {
            _transparencyVehicleRepository = transparencyVehicleRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTransparencyVehicleDataTable(DataTablesStructs.SentParameters sentParameters)
            => await _transparencyVehicleRepository.GetTransparencyVehicleDataTable(sentParameters);

        public async Task InsertRange(IEnumerable<TransparencyVehicle> entities)
            => await _transparencyVehicleRepository.InsertRange(entities);
    }
}
