using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyTelephoneService : ITransparencyTelephoneService
    {
        private readonly ITransparencyTelephoneRepository _transparencyTelephoneRepository;

        public TransparencyTelephoneService(ITransparencyTelephoneRepository transparencyTelephoneRepository)
        {
            _transparencyTelephoneRepository = transparencyTelephoneRepository;
        }

        public async Task AddRange(IEnumerable<TransparencyTelephone> entites)
            => await _transparencyTelephoneRepository.AddRange(entites);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTransparencyTelephoneDataTable(DataTablesStructs.SentParameters sentParameters)
            => await _transparencyTelephoneRepository.GetTransparencyTelephoneDataTable(sentParameters);

    }
}
