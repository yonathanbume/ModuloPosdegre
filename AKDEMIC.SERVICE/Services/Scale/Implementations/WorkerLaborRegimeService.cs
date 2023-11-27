using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class WorkerLaborRegimeService : IWorkerLaborRegimeService
    {
        private readonly IWorkerLaborRegimeRepository _workerLaborRegimeRepository;

        public WorkerLaborRegimeService(IWorkerLaborRegimeRepository workerLaborRegimeRepository)
        {
            _workerLaborRegimeRepository = workerLaborRegimeRepository;
        }

        public async Task<WorkerLaborRegime> Get(Guid id)
        {
            return await _workerLaborRegimeRepository.Get(id);
        }

        public async Task<Tuple<int, List<Tuple<string, int>>>> GetWorkerLaborRegimeQuantityReportByPaginationParameters(PaginationParameter paginationParameter)
        {
            return await _workerLaborRegimeRepository.GetWorkerLaborRegimeQuantityReportByPaginationParameters(paginationParameter);
        }

        public async Task<List<Tuple<string, int>>> GetWorkerLaborRegimeQuantityReport(string search)
        {
            return await _workerLaborRegimeRepository.GetWorkerLaborRegimeQuantityReport(search);
        }

        public async Task<List<Tuple<string, int>>> GetRetirementSystemReport(List<Tuple<string, byte>> retirementSystems)
        {
            return await _workerLaborRegimeRepository.GetRetirementSystemReport(retirementSystems);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkerLaborRegimeDatatable(DataTablesStructs.SentParameters sentParameters, int status, string searchValue = null)
            => await _workerLaborRegimeRepository.GetWorkerLaborRegimeDatatable(sentParameters, status, searchValue);

        public async Task<IEnumerable<WorkerLaborRegime>> GetAll(string search, bool? onlyActive)
            => await _workerLaborRegimeRepository.GetAll(search, onlyActive);

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _workerLaborRegimeRepository.AnyByName(name, ignoredId);

        public async Task Insert(WorkerLaborRegime entity)
            => await _workerLaborRegimeRepository.Insert(entity);

        public async Task Delete(WorkerLaborRegime entity)
            => await _workerLaborRegimeRepository.Delete(entity);

        public async Task Update(WorkerLaborRegime entity)
            => await _workerLaborRegimeRepository.Update(entity);

        public async Task<Tuple<bool, string>> TryDelete(Guid id)
            => await _workerLaborRegimeRepository.TryDelete(id);

        public async Task<object> GetSelect2()
            => await _workerLaborRegimeRepository.GetSelect2();
    }
}
