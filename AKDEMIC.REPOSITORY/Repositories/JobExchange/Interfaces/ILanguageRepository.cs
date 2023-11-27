using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface ILanguageRepository:IRepository<Language>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetLanguageDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<object> GetSelect2CLientSide();
    }
}
