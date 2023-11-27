using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Templates;

namespace AKDEMIC.SERVICE.Services.Investigation.Interfaces
{
    public interface IProjectActivityService
    {
        Task<bool> AnyProjectActivityByName(string name, Guid? id, Guid projectId);
        Task<int> Count(Guid projectId);
        Task<ProjectActivity> Get(Guid id);
        Task<object> GetProjectActivity(Guid id);
        Task<IEnumerable<ProjectActivity>> GetAll();
        Task<IEnumerable<object>> GetProjectActivities(Guid projectId);
        Task<List<ProjectActivityTemplate>> GetProjectActivitiesTemplate(Guid projectId);
        Task<DataTablesStructs.ReturnedData<object>> GetProjectActivitiesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid projectId, string searchValue = null);
        Task DeleteById(Guid id);
        Task Insert(ProjectActivity activity);
        Task<decimal> GetBudgetSumByProjectId(Guid projectId, Guid? ignoredId = null);
        Task Update(ProjectActivity activity);
    }
}
