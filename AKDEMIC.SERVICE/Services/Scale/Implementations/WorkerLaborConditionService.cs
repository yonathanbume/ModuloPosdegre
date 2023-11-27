using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class WorkerLaborConditionService : IWorkerLaborConditionService
    {
        private readonly IWorkerLaborConditionRepository _workerLaborConditionRepository;

        public WorkerLaborConditionService(IWorkerLaborConditionRepository workerLaborConditionRepository)
        {
            _workerLaborConditionRepository = workerLaborConditionRepository;
        }

        public async Task<bool> AnyByName(string name,Guid? laborRegimeId,Guid? ignoredId = null)
            => await _workerLaborConditionRepository.AnyByName(name, laborRegimeId, ignoredId);

        public async Task Delete(WorkerLaborCondition entity)
            => await _workerLaborConditionRepository.Delete(entity);

        public async Task<IEnumerable<WorkerLaborCondition>> GetAll()
        {
            return await _workerLaborConditionRepository.GetAll();
        }

        public async Task<IEnumerable<WorkerLaborCondition>> GetAllWithIncludes(int? status = null)
        {
            return await _workerLaborConditionRepository.GetAllWithIncludes(status);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkerLaborConditionDataTable(DataTablesStructs.SentParameters sentParameters, int? status, string searchValue = null)
            => await _workerLaborConditionRepository.GetWorkerLaborConditionDataTable(sentParameters, status, searchValue);

        public async Task Insert(WorkerLaborCondition entity)
            => await _workerLaborConditionRepository.Insert(entity);

        public async Task Update(WorkerLaborCondition entity)
            => await _workerLaborConditionRepository.Update(entity);

        public async Task<WorkerLaborCondition> Get(Guid id)
            => await _workerLaborConditionRepository.Get(id);

        public async Task<object> GetAllAsSelect2ClientSide(bool includeTitle = false)
            => await _workerLaborConditionRepository.GetAllAsSelect2ClientSide(includeTitle);
    }
}