using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Interfaces
{
    public interface IProjectActivityService
    {
        Task<bool> AnyProjectActivityByName(string name, Guid? id, Guid projectId);
        Task<int> Count(Guid projectId);
        Task<ProjectActivity> Get(Guid id);
        Task<object> GetProjectActivity(Guid id);
        Task<IEnumerable<object>> GetProjectActivities(Guid projectId);
        Task<List<ProjectActivityTemplate>> GetProjectActivitiesTemplate(Guid projectId);
        Task<DataTablesStructs.ReturnedData<object>> GetProjectActivitiesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid projectId, string searchValue = null);
        Task DeleteById(Guid id);
        Task Insert(ProjectActivity activity);
        Task Update(ProjectActivity activity);
        Task<decimal> GetProjectActivitiesTotalBudget(Guid projectId, Guid? id = null);
    }
}
