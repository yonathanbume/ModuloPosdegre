using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IAgreementService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetAgreementDatatable(DataTablesStructs.SentParameters sentParameters, int state , bool isActive, string searchValue = null);
        Task<Agreement> Get(Guid id);
        Task Insert(Agreement agreement);
        Task Delete(Agreement agreement);
        Task Update(Agreement agreement);
        Task<object> GetAgreementsSelect2();
    }
}
