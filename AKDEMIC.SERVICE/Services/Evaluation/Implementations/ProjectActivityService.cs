using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;
using AKDEMIC.SERVICE.Services.Evaluation.Interfaces;

namespace AKDEMIC.SERVICE.Services.Evaluation.Implementations
{
    public class ProjectActivityService : IProjectActivityService
    {
        private readonly IProjectActivityRepository _projectActivityRepository;
        public ProjectActivityService(IProjectActivityRepository projectActivityRepository)
        {
            _projectActivityRepository = projectActivityRepository;
        }
        public async Task<bool> AnyProjectActivityByName(string name, Guid? id, Guid projectId)
        {
            return await _projectActivityRepository.AnyProjectActivityByName(name, id, projectId);
        }
        public async Task<int> Count(Guid projectId)
        {
            return await _projectActivityRepository.Count(projectId);
        }
        public async Task<ProjectActivity> Get(Guid id)
        {
            return await _projectActivityRepository.Get(id);
        }
        public async Task<object> GetProjectActivity(Guid id)
        {
            return await _projectActivityRepository.GetProjectActivity(id);
        }
        public async Task<IEnumerable<object>> GetProjectActivities(Guid projectId)
        {
            return await _projectActivityRepository.GetProjectActivities(projectId);
        }
        public async Task<List<ProjectActivityTemplate>> GetProjectActivitiesTemplate(Guid projectId)
        {
            return await _projectActivityRepository.GetProjectActivitiesTemplate(projectId);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectActivitiesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid projectId, string searchValue = null)
        {
            return await _projectActivityRepository.GetProjectActivitiesDatatable(sentParameters, userId, projectId, searchValue);
        }
        public async Task DeleteById(Guid id)
        {
            await _projectActivityRepository.DeleteById(id);
        }
        public async Task Insert(ProjectActivity activity)
        {
            await _projectActivityRepository.Insert(activity);
        }
        public async Task Update(ProjectActivity activity)
        {
            await _projectActivityRepository.Update(activity);
        }

        public async Task<decimal> GetProjectActivitiesTotalBudget(Guid projectId,Guid? id =null)
        {
            return await _projectActivityRepository.GetProjectActivitiesTotalBudget(projectId,id);
        }
    }
}
