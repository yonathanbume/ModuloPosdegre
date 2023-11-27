using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class LivingCostService : ILivingCostService
    {
        private readonly ILivingCostRepository _livingCostRepository;

        public LivingCostService(ILivingCostRepository livingCostRepository)
        {
            _livingCostRepository = livingCostRepository;
        }

        public async Task<LivingCost> Get(Guid id)
            => await _livingCostRepository.Get(id);

        public async Task GetAll()
            => await _livingCostRepository.GetAll();

        public async Task Insert(LivingCost entity)
            => await _livingCostRepository.Insert(entity);

        public async Task Update(LivingCost entity)
            => await _livingCostRepository.Update(entity);

        public async Task<LivingCost> First()
            => await _livingCostRepository.First();

        public async Task<DataTablesStructs.ReturnedData<LivingCost>> GetLivingCostDataTable(DataTablesStructs.SentParameters parameters, string search)
        {
            return await _livingCostRepository.GetLivingCostDataTable(parameters, search);
        }

        public async Task DeleteById(Guid id)
        {
            await _livingCostRepository.DeleteById(id);
        }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _livingCostRepository.AnyByName(name, ignoredId);
    }
}
