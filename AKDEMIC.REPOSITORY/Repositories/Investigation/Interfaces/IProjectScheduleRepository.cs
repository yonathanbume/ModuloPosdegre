using System;
using System.Threading.Tasks;

using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces
{
    public interface IProjectScheduleRepository : IRepository<ProjectSchedule>
    {
        Task<int> Count(Guid projectId);
        Task<object> GetProjectSchedule(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetProjectSchedulesDatatable(DataTablesStructs.SentParameters sentParameters, Guid projectId);
    }
}
