using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<object> GetProject(Guid id);
        Task<ProjectTemplate> GetProjectTemplate(Guid id);
        Task<bool> AnyProjectByName(string name, Guid? id);
        Task<IEnumerable<object>> GetProjects();
        Task<DataTablesStructs.ReturnedData<object>> GetProjectsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null, int modality = -1,string startDate = null, string endDate = null,byte? status = null, Guid? careerId = null);
        Task<DataTablesStructs.ReturnedData<ProjectReportDataTemplate>> GetProjectsReportDatatable(DataTablesStructs.SentParameters sentParameters, int status, string name, string area, int modality = -1, Guid? careerId = null);
        Task<object> GetProjectPerStatusStatisticalBox();
        Task<object> GetProjectPerCareerStatisticalBox();
        Task<object> GetBudgetPerCareerStatisticalBox();
        Task<object> GetProjectsChart();
        Task<List<ProjectTemplate>> GetPublishedProjects(int page);
        Task<DataTablesStructs.ReturnedData<object>> GetProjectsEvaluatorDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null, int modality = -1);
        Task<DataTablesStructs.ReturnedData<object>> GetProjectsWithEvaluatorsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, int modality = -1, Guid? careerId = null);
        Task<List<string>> GetEvaluatorsIdByEvaluationProjectId(Guid id, int modality = -1);
        Task<bool> ExistProjectName(string name, Guid? id);
        Task<ProjectWithRubricsCareersDependenciesTemplate> GetProjectWithRubrics_Careers_Dependencies(Guid id, int modality = -1);
        Task<Project> GetProjectByIdAndModality(Guid id, int modality);
        Task<object> GetAllOfProjectsByStatusChart(int year = 0);
    }
}
