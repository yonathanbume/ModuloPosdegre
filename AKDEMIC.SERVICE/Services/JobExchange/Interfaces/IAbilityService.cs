using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IAbilityService
    {
        Task<IEnumerable<Ability>> GetAll();
        Task<DataTablesStructs.ReturnedData<object>> GetAbilityDatatable(DataTablesStructs.SentParameters sentParameters, int status, string searchValue = null);
        Task Insert(Ability ability);
        Task Update(Ability ability);
        Task Delete(Ability ability);
        Task<Ability> Get(Guid id);
        Task<object> GetAbilitiesSelect2ClientSide();
    }
}
