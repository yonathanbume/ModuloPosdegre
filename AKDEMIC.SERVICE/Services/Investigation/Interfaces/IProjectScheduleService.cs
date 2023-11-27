using System;
using System.Threading.Tasks;

using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;

namespace AKDEMIC.SERVICE.Services.Investigation.Interfaces
{
    public interface IProjectScheduleService
    {
        Task<int> Count(Guid projectId);
        Task<DataTablesStructs.ReturnedData<object>> GetProjectSchedulesDatatable(DataTablesStructs.SentParameters sentParameters, Guid projectId);
        Task DeleteById(Guid id);
        Task Insert(ProjectSchedule advance);
        Task Update(ProjectSchedule advance);
    }
}
