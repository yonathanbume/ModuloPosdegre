using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class WorkerSchoolDegreeService : IWorkerSchoolDegreeService
    {
        private readonly IWorkerSchoolDegreeRepository _workerSchoolDegreeRepository;

        public WorkerSchoolDegreeService(IWorkerSchoolDegreeRepository workerSchoolDegreeRepository)
        {
            _workerSchoolDegreeRepository = workerSchoolDegreeRepository;
        }

        public async Task<WorkerSchoolDegree> Get(Guid workerSchoolDegreeId)
        {
            return await _workerSchoolDegreeRepository.Get(workerSchoolDegreeId);
        }

        public async Task<IEnumerable<WorkerSchoolDegree>> GetAll()
        {
            return await _workerSchoolDegreeRepository.GetAll();
        }

        public async Task<List<WorkerSchoolDegree>> GetAllByUserId(string userId)
        {
            return await _workerSchoolDegreeRepository.GetAllByUserId(userId);
        }

        public async Task Insert(WorkerSchoolDegree workerSchoolDegree)
        {
            await _workerSchoolDegreeRepository.Insert(workerSchoolDegree);
        }

        public async Task Update(WorkerSchoolDegree workerSchoolDegree)
        {
            await _workerSchoolDegreeRepository.Update(workerSchoolDegree);
        }

        public async Task Delete(WorkerSchoolDegree workerSchoolDegree)
        {
            await _workerSchoolDegreeRepository.Delete(workerSchoolDegree);
        }

        public async Task<int> GetWorkerSchoolDegreesQuantity(string userId)
        {
            return await _workerSchoolDegreeRepository.GetWorkerSchoolDegreesQuantity(userId);
        }

        public async Task<List<SchoolDegreesTemplate>> GetWorkerSchoolDegreesByPaginationParameters(string userId, PaginationParameter paginationParameter)
        {
            return await _workerSchoolDegreeRepository.GetWorkerSchoolDegreesByPaginationParameters(userId, paginationParameter);
        }

        public async Task<WorkerSchoolDegree> GetWithIncludes(Guid id)
            => await _workerSchoolDegreeRepository.GetWithIncludes(id);
    }
}
