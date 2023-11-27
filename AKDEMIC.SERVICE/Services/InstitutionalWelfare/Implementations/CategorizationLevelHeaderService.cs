using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations
{
    public class CategorizationLevelHeaderService : ICategorizationLevelHeaderService
    {
        private readonly ICategorizationLevelHeaderRepository _categorizationLevelHeaderRepository;

        public CategorizationLevelHeaderService(ICategorizationLevelHeaderRepository categorizationLevelHeaderRepository)
        {
            _categorizationLevelHeaderRepository = categorizationLevelHeaderRepository;
        }

        public async Task Delete(CategorizationLevelHeader categorizationLevelHeader)
        {
            await _categorizationLevelHeaderRepository.Delete(categorizationLevelHeader);
        }

        public async Task<CategorizationLevelHeader> Get(Guid id)
        {
            return await _categorizationLevelHeaderRepository.Get(id);
        }

        public async Task<IEnumerable<CategorizationLevelHeader>> GetAll()
        {
            return await _categorizationLevelHeaderRepository.GetAll();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCategorizationLevelHeaderDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _categorizationLevelHeaderRepository.GetCategorizationLevelHeaderDatatable(sentParameters, searchValue);
        }

        public async Task<object> GetCategorizationLevelHeaderSelect2()
        {
            return await _categorizationLevelHeaderRepository.GetCategorizationLevelHeaderSelect2();
        }

        public async Task Insert(CategorizationLevelHeader categorizationLevelHeader)
        {
            await _categorizationLevelHeaderRepository.Insert(categorizationLevelHeader);
        }

        public async Task Update(CategorizationLevelHeader categorizationLevelHeader)
        {
            await _categorizationLevelHeaderRepository.Update(categorizationLevelHeader);
        }
    }
}
