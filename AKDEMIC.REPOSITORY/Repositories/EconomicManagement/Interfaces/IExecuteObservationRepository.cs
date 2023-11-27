using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IExecuteObservationRepository : IRepository<ExecuteObservation>
    {
        Task<object> GetExecutionObs(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetFiles(DataTablesStructs.SentParameters sentParameters, Guid id);
    }
}
