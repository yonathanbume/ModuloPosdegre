using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class EconomicActivityService: IEconomicActivityService
    {
        private readonly IEconomicActivityRepository _economicActivityRepository;

        public EconomicActivityService(IEconomicActivityRepository economicActivityRepository)
        {
            _economicActivityRepository = economicActivityRepository;
        }

        public async Task Delete(EconomicActivity model)
        {
            await _economicActivityRepository.Delete(model);
        }

        public async Task<bool> ExistCode(string code, Guid? Id = null)
        {
            return await _economicActivityRepository.ExistCode(code, Id);
        }

        public async Task<EconomicActivity> Get(Guid id)
        {
            return await _economicActivityRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEcnomicActivityDatatable(DataTablesStructs.SentParameters sentParameters, Guid? economicActivityDivisionId = null, string searchValue = null)
        {
            return await _economicActivityRepository.GetEcnomicActivityDatatable(sentParameters, economicActivityDivisionId, searchValue);
        }

        public async Task<object> GetClientSideSelect2(Guid? economicActivityDivisionId = null)
        {
            return await _economicActivityRepository.GetClientSideSelect2(economicActivityDivisionId);
        }

        public async Task Insert(EconomicActivity model)
        {
            await _economicActivityRepository.Insert(model);
        }

        public async Task Update(EconomicActivity model)
        {
            await _economicActivityRepository.Update(model);
        }
    }
}
