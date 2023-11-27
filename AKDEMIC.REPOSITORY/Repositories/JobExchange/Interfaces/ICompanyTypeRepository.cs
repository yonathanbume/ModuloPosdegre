using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface ICompanyTypeRepository : IRepository<CompanyType>
    {
        Task<object> GetCompanyTypeSelect2();
        Task<DataTablesStructs.ReturnedData<object>> GetCompanyTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
