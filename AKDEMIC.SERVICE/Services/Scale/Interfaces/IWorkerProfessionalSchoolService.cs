using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerProfessionalSchoolService
    {
        Task<WorkerProfessionalSchool> Get(Guid workerProfessionalSchoolId);
        Task<WorkerProfessionalSchool> GetWithIncludes(Guid id);
        Task<IEnumerable<WorkerProfessionalSchool>> GetAll();
        Task<List<WorkerProfessionalSchool>> GetAllByUserId(string userId);
        Task Insert(WorkerProfessionalSchool workerProfessionalSchool);
        Task Update(WorkerProfessionalSchool workerProfessionalSchool);
        Task Delete(WorkerProfessionalSchool workerProfessionalSchool);
        Task<int> GetWorkerProfessionalSchoolsQuantity(string userId);
        Task<List<ProfesionalSchoolTemplate>> GetWorkerProfessionalSchoolsByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
