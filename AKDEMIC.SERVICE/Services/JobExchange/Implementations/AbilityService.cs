using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class AbilityService: IAbilityService
    {
        private readonly IAbilityRepository _abilityRepository;

        public AbilityService(IAbilityRepository abilityRepository)
        {
            _abilityRepository = abilityRepository;
        }

        public async Task Delete(Ability ability)
        {
            await _abilityRepository.Delete(ability);
        }

        public async Task<Ability> Get(Guid id)
        {
            return await _abilityRepository.Get(id);
        }

        public async Task<object> GetAbilitiesSelect2ClientSide()
        {
            return await _abilityRepository.GetAbilitiesSelect2ClientSide();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAbilityDatatable(DataTablesStructs.SentParameters sentParameters, int status, string searchValue = null)
        {
            return await _abilityRepository.GetAbilityDatatable(sentParameters, status,searchValue);
        }

        public async Task<IEnumerable<Ability>> GetAll()
        {
            return await _abilityRepository.GetAll();
        }

        public async Task Insert(Ability ability)
        {
            await _abilityRepository.Insert(ability);
        }

        public async Task Update(Ability ability)
        {
            await _abilityRepository.Update(ability);
        }
    }
}
