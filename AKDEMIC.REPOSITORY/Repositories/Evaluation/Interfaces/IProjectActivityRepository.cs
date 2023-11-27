using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces
{
    public interface IProjectActivityRepository : IRepository<ProjectActivity>
    {
        Task<int> Count(Guid projectId);
        Task<object> GetProjectActivity(Guid id);
        Task<bool> AnyProjectActivityByName(string name, Guid? id, Guid projectId);
        Task<IEnumerable<object>> GetProjectActivities(Guid projectId);
        Task<List<ProjectActivityTemplate>> GetProjectActivitiesTemplate(Guid projectId);
        Task<DataTablesStructs.ReturnedData<object>> GetProjectActivitiesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid projectId, string searchValue = null);
        Task<decimal> GetProjectActivitiesTotalBudget(Guid projectId, Guid? id = null);
    }
}
