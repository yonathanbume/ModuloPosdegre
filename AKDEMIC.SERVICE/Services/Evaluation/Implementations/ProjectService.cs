using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;
using AKDEMIC.SERVICE.Services.Evaluation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<bool> AnyProjectByName(string name, Guid? id)
        {
            return await _projectRepository.AnyProjectByName(name, id);
        }
        public async Task<int> Count()
        {
            return await _projectRepository.Count();
        }
        public async Task<Project> Get(Guid id)
        {
            return await _projectRepository.Get(id);
        }
        public async Task<object> GetProject(Guid id)
        {
            return await _projectRepository.GetProject(id);
        }
        public async Task<ProjectTemplate> GetProjectTemplate(Guid id)
        {
            return await _projectRepository.GetProjectTemplate(id);
        }
        public async Task<IEnumerable<Project>> GetAll()
        {
            return await _projectRepository.GetAll();
        }
        public async Task<IEnumerable<object>> GetProjects()
        {
            return await _projectRepository.GetProjects();
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null, int modality = -1, string startDate = null, string endDate = null, byte? status = null, Guid? careerId = null)
        {
            return await _projectRepository.GetProjectsDatatable(sentParameters, userId, searchValue, modality, startDate, endDate, status, careerId);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectsEvaluatorsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null, int modality = -1)
        {
            return await _projectRepository.GetProjectsEvaluatorDatatable(sentParameters, userId, searchValue, modality);
        }
        public async Task<DataTablesStructs.ReturnedData<ProjectReportDataTemplate>> GetProjectsReportDatatable(DataTablesStructs.SentParameters sentParameters, int status, string name, string area, int modality = -1, Guid? careerId = null)
        {
            return await _projectRepository.GetProjectsReportDatatable(sentParameters, status, name, area, modality, careerId);
        }
        public async Task DeleteById(Guid id)
        {
            await _projectRepository.DeleteById(id);
        }
        public async Task Insert(Project project)
        {
            await _projectRepository.Insert(project);
        }
        public async Task Update(Project project)
        {
            await _projectRepository.Update(project);
        }
        public async Task<object> GetProjectPerStatusStatisticalBox()
        {
            return await _projectRepository.GetProjectPerStatusStatisticalBox();
        }
        public async Task<object> GetProjectPerCareerStatisticalBox()
        {
            return await _projectRepository.GetProjectPerCareerStatisticalBox();
        }
        public async Task<object> GetBudgetPerCareerStatisticalBox()
        {
            return await _projectRepository.GetBudgetPerCareerStatisticalBox();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectsWithEvaluatorsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, int modality = -1, Guid? careerId = null)
        {
            return await _projectRepository.GetProjectsWithEvaluatorsDatatable(sentParameters, searchValue, modality, careerId);
        }

        public async Task<List<string>> GetEvaluatorsIdByEvaluationProjectId(Guid id, int modality = -1)
        {
            return await _projectRepository.GetEvaluatorsIdByEvaluationProjectId(id, modality);
        }

        public async Task<bool> ExistProjectName(string name, Guid? id = null)
        {
            return await _projectRepository.ExistProjectName(name, id);
        }

        public async Task<ProjectWithRubricsCareersDependenciesTemplate> GetProjectWithRubrics_Careers_Dependencies(Guid id, int modality = -1)
        {
            return await _projectRepository.GetProjectWithRubrics_Careers_Dependencies(id, modality);
        }

        public async Task<Project> GetProjectByIdAndModality(Guid id, int modality)
        {
            return await _projectRepository.GetProjectByIdAndModality(id, modality);
        }

        public async Task<object> GetProjectsChart()
        {
            return await _projectRepository.GetProjectsChart();
        }

        public async Task<List<ProjectTemplate>> GetPublishedProjects(int page)
        {
            return await _projectRepository.GetPublishedProjects(page);
        }

        public async Task<object> GetAllOfProjectsByStatusChart(int year = 0)
            => await _projectRepository.GetAllOfProjectsByStatusChart(year);
    }
}
