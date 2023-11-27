using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IAgreementRepository: IRepository<Agreement>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetAgreementDatatable(DataTablesStructs.SentParameters sentParameters, int state, bool isActive, string searchValue = null);
        Task<object> GetAgreementsSelect2();
    }
}
