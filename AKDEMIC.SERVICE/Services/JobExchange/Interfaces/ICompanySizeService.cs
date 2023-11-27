using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface ICompanySizeService
    {
        Task<object> GetCompanySizeSelect2();
        Task<DataTablesStructs.ReturnedData<object>> GetCompanySizeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task Insert(CompanySize model);
        Task Update(CompanySize model);
        Task Delete(CompanySize model);
        Task<CompanySize> Get(Guid id);
    }
}
