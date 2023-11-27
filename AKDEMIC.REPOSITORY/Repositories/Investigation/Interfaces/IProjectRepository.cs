using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Templates;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<object> GetProject(Guid id);
        Task<ProjectTemplate> GetProjectTemplate(Guid id);
        Task<bool> AnyProjectByName(string name, Guid? id);
        Task<IEnumerable<object>> GetProjects();
        Task<DataTablesStructs.ReturnedData<object>> GetProjectsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid? careerId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetProjectEvaluatorsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, int type, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetFormativeProjectsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid? sectionId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetPublishedProjectsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid? careerId, Guid? areaId, Guid? lineId, int type, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetProjectsDatatable(DataTablesStructs.SentParameters sentParameters, int type, int year, Guid? facultyId, Guid? careerId, Guid? lineId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetProjectsReportDatatable(DataTablesStructs.SentParameters sentParameters, int status, int type, string name, Guid? areaId, Guid? lineId, Guid? careerId);
        Task<object> GetProjectPerStatusStatisticalBox(int status, string startDate, string endDate, List<Guid> careers, Guid? areaId, Guid? lineId, List<Guid> programs);
        Task<object> GetProjectPerCareerStatisticalBox(int status, string startDate, string endDate, List<Guid> careers, Guid? areaId, Guid? lineId, List<Guid> programs);
        Task<object> GetBudgetPerCareerStatisticalBox(int status, string startDate, string endDate, List<Guid> careers, Guid? areaId, Guid? lineId, List<Guid> programs);
        Task<object> GetOnGoingInvestigationChart(int investigationType, Guid? academicProgramId = null);
    }
}
