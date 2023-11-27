using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencySelectionCommitteeService : ITransparencySelectionCommitteeService
    {
        private readonly ITransparencySelectionCommitteeRepository _transparencySelectionCommitteeRepository;
        public TransparencySelectionCommitteeService(ITransparencySelectionCommitteeRepository transparencySelectionCommitteeRepository)
        {
            _transparencySelectionCommitteeRepository = transparencySelectionCommitteeRepository;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetConciliationActsDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            return await _transparencySelectionCommitteeRepository.GetConciliationActsDataTable(sentParameters);
        }

        public async Task InsertRange(List<TransparencySelectionCommittee> part)
        {
            await _transparencySelectionCommitteeRepository.InsertRange(part);
        }
    }
}
