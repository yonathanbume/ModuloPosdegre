using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface ICompanySizeRepository : IRepository<CompanySize>
    {
        Task<object> GetCompanySizeSelect2();
        Task<DataTablesStructs.ReturnedData<object>> GetCompanySizeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
