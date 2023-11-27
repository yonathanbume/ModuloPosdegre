using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IAbilityRepository:IRepository<Ability>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetAbilityDatatable(DataTablesStructs.SentParameters sentParameters , int status, string searchValue = null);
        Task<object> GetAbilitiesSelect2ClientSide();
    }
}
