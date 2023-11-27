using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyEmployeeService : ITransparencyEmployeeService
    {
        private readonly ITransparencyEmployeeRepository _transparencyEmployeeRepository;
        public TransparencyEmployeeService(ITransparencyEmployeeRepository transparencyEmployeeRepository)
        {
            _transparencyEmployeeRepository = transparencyEmployeeRepository;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetConciliationActsDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            return await _transparencyEmployeeRepository.GetConciliationActsDataTable(sentParameters);
        }

        public async Task InsertRange(List<TransparencyEmployee> part)
        {
            await _transparencyEmployeeRepository.InsertRange(part);
        }
    }
}
