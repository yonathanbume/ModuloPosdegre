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
    public class WorkerProfessionalSchoolService : IWorkerProfessionalSchoolService
    {
        private readonly IWorkerProfessionalSchoolRepository _workerProfessionalSchoolRepository;

        public WorkerProfessionalSchoolService(IWorkerProfessionalSchoolRepository workerProfessionalSchoolRepository)
        {
            _workerProfessionalSchoolRepository = workerProfessionalSchoolRepository;
        }

        public async Task<WorkerProfessionalSchool> Get(Guid workerProfessionalSchoolId)
        {
            return await _workerProfessionalSchoolRepository.Get(workerProfessionalSchoolId);
        }

        public async Task<IEnumerable<WorkerProfessionalSchool>> GetAll()
        {
            return await _workerProfessionalSchoolRepository.GetAll();
        }

        public async Task<List<WorkerProfessionalSchool>> GetAllByUserId(string userId)
        {
            return await _workerProfessionalSchoolRepository.GetAllByUserId(userId);
        }

        public async Task Insert(WorkerProfessionalSchool workerProfessionalSchool)
        {
            await _workerProfessionalSchoolRepository.Insert(workerProfessionalSchool);
        }

        public async Task Update(WorkerProfessionalSchool workerProfessionalSchool)
        {
            await _workerProfessionalSchoolRepository.Update(workerProfessionalSchool);
        }

        public async Task Delete(WorkerProfessionalSchool workerProfessionalSchool)
        {
            await _workerProfessionalSchoolRepository.Delete(workerProfessionalSchool);
        }

        public async Task<int> GetWorkerProfessionalSchoolsQuantity(string userId)
        {
            return await _workerProfessionalSchoolRepository.GetWorkerProfessionalSchoolsQuantity(userId);
        }

        public async Task<List<ProfesionalSchoolTemplate>> GetWorkerProfessionalSchoolsByPaginationParameters(string userId, PaginationParameter paginationParameter)
        {
            return await _workerProfessionalSchoolRepository.GetWorkerProfessionalSchoolsByPaginationParameters(userId, paginationParameter);
        }

        public async Task<WorkerProfessionalSchool> GetWithIncludes(Guid id)
            => await _workerProfessionalSchoolRepository.GetWithIncludes(id);
    }
}
