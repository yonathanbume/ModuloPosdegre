using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface ICompanyTypeService
    {
        Task<object> GetCompanyTypeSelect2();
        Task<DataTablesStructs.ReturnedData<object>> GetCompanyTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task Insert(CompanyType model);
        Task Update(CompanyType model);
        Task Delete(CompanyType model);
        Task<CompanyType> Get(Guid id);
    }
}
