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
    public class ProjectAdvanceService : IProjectAdvanceService
    {
        private readonly IProjectAdvanceRepository _projectAdvanceRepository;
        public ProjectAdvanceService(IProjectAdvanceRepository projectAdvanceRepository)
        {
            _projectAdvanceRepository = projectAdvanceRepository;
        }
        public async Task<bool> AnyProjectAdvanceByName(string name, Guid? id, Guid projectId)
        {
            return await _projectAdvanceRepository.AnyProjectAdvanceByName(name, id, projectId);
        }
        public async Task<int> Count(Guid projectId)
        {
            return await _projectAdvanceRepository.Count(projectId);
        }
        public async Task<ProjectAdvance> Get(Guid id)
        {
            return await _projectAdvanceRepository.Get(id);
        }
        public async Task<object> GetProjectAdvance(Guid id)
        {
            return await _projectAdvanceRepository.GetProjectAdvance(id);
        }
        public async Task<IEnumerable<object>> GetProjectAdvances(Guid projectId)
        {
            return await _projectAdvanceRepository.GetProjectAdvances(projectId);
        }
        public async Task<List<ProjectAdvanceTemplate>> GetProjectAdvancesTemplate(Guid projectId)
        {
            return await _projectAdvanceRepository.GetProjectAdvancesTemplate(projectId);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectAdvancesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid projectId, string searchValue = null)
        {
            return await _projectAdvanceRepository.GetProjectAdvancesDatatable(sentParameters, userId, projectId, searchValue);
        }
        public async Task DeleteById(Guid id)
        {
            await _projectAdvanceRepository.DeleteById(id);
        }
        public async Task Insert(ProjectAdvance Advance)
        {
            await _projectAdvanceRepository.Insert(Advance);
        }
        public async Task Update(ProjectAdvance Advance)
        {
            await _projectAdvanceRepository.Update(Advance);
        }

        public async Task<Project> GetProjectByProjectAdvanceId(Guid id)
        {
            return await _projectAdvanceRepository.GetProjectByProjectAdvanceId(id);
        }

        public async Task<ProjectAdvance> IsFinal(Guid projectId)
        {
            return await _projectAdvanceRepository.IsFinal(projectId);
        }

        public async Task<ProjectAdvance> GetWithProjectItemScores(Guid advanceId)
        {
            return await _projectAdvanceRepository.GetWithProjectItemScores(advanceId);
        }

        public async Task<string> GetAdvanceHistoryUrl(Guid id)
        {
            return await _projectAdvanceRepository.GetAdvanceHistoryUrl(id);
        }
    }
}
