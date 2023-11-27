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
    public class EconomicActivityDivisionService : IEconomicActivityDivisionService
    {
        private readonly IEconomicActivityDivisionRepository _economicActivityDivisionRepository;

        public EconomicActivityDivisionService(IEconomicActivityDivisionRepository economicActivityDivisionRepository)
        {
            _economicActivityDivisionRepository = economicActivityDivisionRepository;
        }

        public Task<bool> AnyByCode(string code, Guid? id = null)
            => _economicActivityDivisionRepository.AnyByCode(code, id);

        public Task Delete(EconomicActivityDivision economicActivityDivision)
            => _economicActivityDivisionRepository.Delete(economicActivityDivision);

        public Task<EconomicActivityDivision> Get(Guid id)
            => _economicActivityDivisionRepository.Get(id);

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, Guid? economicActivitySectionId = null, string searchValue = null)
            => _economicActivityDivisionRepository.GetAllDatatable(sentParameters, economicActivitySectionId, searchValue);

        public Task<object> GetClientSideSelect2(Guid? economicActivitySectionId = null)
            => _economicActivityDivisionRepository.GetClientSideSelect2(economicActivitySectionId);

        public Task Insert(EconomicActivityDivision economicActivityDivision)
            => _economicActivityDivisionRepository.Insert(economicActivityDivision);

        public Task Update(EconomicActivityDivision economicActivityDivision)
            => _economicActivityDivisionRepository.Update(economicActivityDivision);
    }
}
