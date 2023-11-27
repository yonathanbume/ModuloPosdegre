using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerLaborRegimeService
    {
        Task<WorkerLaborRegime> Get(Guid id);
        Task<Tuple<int, List<Tuple<string, int>>>> GetWorkerLaborRegimeQuantityReportByPaginationParameters(PaginationParameter paginationParameter);
        Task<List<Tuple<string, int>>> GetWorkerLaborRegimeQuantityReport(string search);
        Task<List<Tuple<string, int>>> GetRetirementSystemReport(List<Tuple<string, byte>> retirementSystems);
        Task<DataTablesStructs.ReturnedData<object>> GetWorkerLaborRegimeDatatable(DataTablesStructs.SentParameters sentParameters, int status, string searchValue = null);
        Task<IEnumerable<WorkerLaborRegime>> GetAll(string search, bool? onlyActive);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
        Task Insert(WorkerLaborRegime entity);
        Task Delete(WorkerLaborRegime entity);
        Task Update(WorkerLaborRegime entity);
        Task<Tuple<bool, string>> TryDelete(Guid id);
        Task<object> GetSelect2();
    }
}
