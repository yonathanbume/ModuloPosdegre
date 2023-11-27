using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IExecuteObservationService
    {
        Task AddAsync(ExecuteObservation executeObservation);
        Task<ExecuteObservation> Get(Guid id);
        Task<object> GetExecutionObs(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetFiles(DataTablesStructs.SentParameters sentParameters, Guid id);
    }
}
