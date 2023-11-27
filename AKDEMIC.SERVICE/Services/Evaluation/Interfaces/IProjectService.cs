using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Interfaces
{
    public interface IProjectService
    {
        Task<bool> AnyProjectByName(string name, Guid? id);
        Task<int> Count();
        Task<Project> Get(Guid id);
        Task<object> GetProject(Guid id);
        Task<ProjectTemplate> GetProjectTemplate(Guid id);
        Task<IEnumerable<Project>> GetAll();
        Task<IEnumerable<object>> GetProjects();
        Task<DataTablesStructs.ReturnedData<object>> GetProjectsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null, int modality = -1, string startDate = null, string endDate = null, byte? status = null, Guid? careerId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetProjectsEvaluatorsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null, int modality = -1);
        Task<DataTablesStructs.ReturnedData<ProjectReportDataTemplate>> GetProjectsReportDatatable(DataTablesStructs.SentParameters sentParameters, int status, string name, string area, int modality = -1, Guid? careerId = null);
        Task DeleteById(Guid id);
        Task Insert(Project project);
        Task Update(Project project);
        Task<object> GetProjectPerStatusStatisticalBox();
        Task<object> GetProjectPerCareerStatisticalBox();
        Task<object> GetBudgetPerCareerStatisticalBox();
        Task<object> GetProjectsChart();
        Task<List<ProjectTemplate>> GetPublishedProjects(int page);

        Task<DataTablesStructs.ReturnedData<object>> GetProjectsWithEvaluatorsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, int modality = -1, Guid? careerId = null);
        Task<List<string>> GetEvaluatorsIdByEvaluationProjectId(Guid id, int modality = -1);
        Task<bool> ExistProjectName(string name, Guid? id = null);
        Task<ProjectWithRubricsCareersDependenciesTemplate> GetProjectWithRubrics_Careers_Dependencies(Guid id, int modality = -1);
        Task<Project> GetProjectByIdAndModality(Guid id, int modality);
        Task<object> GetAllOfProjectsByStatusChart(int year = 0);
    }
}
