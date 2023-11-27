using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public sealed class WorkerLaborCategoryService : IWorkerLaborCategoryService
    {
        private readonly IWorkerLaborCategoryRepository _workerLaborCategoryRepository;
        public WorkerLaborCategoryService(IWorkerLaborCategoryRepository workerLaborCategoryRepository)
        {
            _workerLaborCategoryRepository = workerLaborCategoryRepository;
        }

        public async Task<bool> AnyByName(string name, Guid? laborRegimeId, Guid? ignoredId = null)
            => await _workerLaborCategoryRepository.AnyByName(name, laborRegimeId, ignoredId);

        public async Task Delete(WorkerLaborCategory entity)
            => await _workerLaborCategoryRepository.Delete(entity);

        public async Task<WorkerLaborCategory> Get(Guid id)
            => await _workerLaborCategoryRepository.Get(id);

        public async Task<IEnumerable<WorkerLaborCategory>> GetAll()
        {
            return await _workerLaborCategoryRepository.GetAll();
        }

        public async Task<object> GetSelect2ClientSide()
            => await _workerLaborCategoryRepository.GetSelect2ClientSide();

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkerLaborCategoryDatatable(DataTablesStructs.SentParameters sentParameters, int? status, string searchvalue = null)
            => await _workerLaborCategoryRepository.GetWorkerLaborCategoryDatatable(sentParameters, status, searchvalue);

        public Task<object> GetWorkerLaborCategorySelect()
            => _workerLaborCategoryRepository.GetWorkerLaborCategorySelect();

        public async Task Insert(WorkerLaborCategory entity)
            => await _workerLaborCategoryRepository.Insert(entity);

        public async Task Update(WorkerLaborCategory entity)
            => await _workerLaborCategoryRepository.Update(entity);

        Task<WorkerLaborCategory> IWorkerLaborCategoryService.GetAsync(Guid id)
            => _workerLaborCategoryRepository.Get(id);
    }
}