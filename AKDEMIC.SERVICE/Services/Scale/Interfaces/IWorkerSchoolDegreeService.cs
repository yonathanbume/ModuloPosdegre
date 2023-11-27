using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerSchoolDegreeService
    {
        Task<WorkerSchoolDegree> Get(Guid workerSchoolDegree);
        Task<WorkerSchoolDegree> GetWithIncludes(Guid id);
        Task<IEnumerable<WorkerSchoolDegree>> GetAll();
        Task<List<WorkerSchoolDegree>> GetAllByUserId(string userId);
        Task Insert(WorkerSchoolDegree workerSchoolDegree);
        Task Update(WorkerSchoolDegree workerSchoolDegree);
        Task Delete(WorkerSchoolDegree workerSchoolDegree);
        Task<int> GetWorkerSchoolDegreesQuantity(string userId);
        Task<List<SchoolDegreesTemplate>> GetWorkerSchoolDegreesByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
