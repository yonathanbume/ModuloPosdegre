using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class EconomicActivitySectionService: IEconomicActivitySectionService
    {
        private readonly IEconomicActivitySectionRepository _economicActivitySectionRepository;
        public EconomicActivitySectionService(IEconomicActivitySectionRepository economicActivitySectionRepository)
        {
            _economicActivitySectionRepository = economicActivitySectionRepository;
        }

        public Task<bool> AnyByCode(string code, Guid? id = null)
            => _economicActivitySectionRepository.AnyByCode(code, id);

        public Task Delete(EconomicActivitySection economicActivitySection)
            => _economicActivitySectionRepository.Delete(economicActivitySection);

        public Task<EconomicActivitySection> Get(Guid id)
            => _economicActivitySectionRepository.Get(id);

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _economicActivitySectionRepository.GetAllDatatable(sentParameters, searchValue);

        public Task<object> GetClientSideSelect2()
            => _economicActivitySectionRepository.GetClientSideSelect2();

        public Task Insert(EconomicActivitySection economicActivitySection)
            => _economicActivitySectionRepository.Insert(economicActivitySection);

        public Task Update(EconomicActivitySection economicActivitySection)
            => _economicActivitySectionRepository.Update(economicActivitySection);
    }
}
