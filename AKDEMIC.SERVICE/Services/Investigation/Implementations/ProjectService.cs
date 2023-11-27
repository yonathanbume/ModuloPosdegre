using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Templates;
using AKDEMIC.SERVICE.Services.Investigation.Interfaces;

namespace AKDEMIC.SERVICE.Services.Investigation.Implementations
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
        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid? careerId, string searchValue = null)
        {
            return await _projectRepository.GetProjectsDatatable(sentParameters, userId, careerId, searchValue);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectEvaluatorsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, int type, string searchValue = null)
        {
            return await _projectRepository.GetProjectEvaluatorsDatatable(sentParameters, userId, type, searchValue);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetFormativeProjectsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid? sectionId, string searchValue = null)
        {
            return await _projectRepository.GetFormativeProjectsDatatable(sentParameters, userId, sectionId, searchValue);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetPublishedProjectsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid? careerId, Guid? areaId, Guid? lineId, int type, string searchValue = null)
        {
            return await _projectRepository.GetPublishedProjectsDatatable(sentParameters, userId, careerId, areaId, lineId, type, searchValue);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectsDatatable(DataTablesStructs.SentParameters sentParameters, int type, int year, Guid? facultyId, Guid? careerId, Guid? lineId, string searchValue = null)
        {
            return await _projectRepository.GetProjectsDatatable(sentParameters, type, year, facultyId, careerId, lineId, searchValue);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectsReportDatatable(DataTablesStructs.SentParameters sentParameters, int status, int type, string name, Guid? areaId, Guid? lineId, Guid? careerId)
        {
            return await _projectRepository.GetProjectsReportDatatable(sentParameters, status, type, name, areaId, lineId, careerId);
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
        public async Task<object> GetProjectPerStatusStatisticalBox(int status, string startDate, string endDate, List<Guid> careers, Guid? areaId, Guid? lineId, List<Guid> programs)
        {
            return await _projectRepository.GetProjectPerStatusStatisticalBox(status, startDate, endDate, careers, areaId, lineId, programs);
        }
        public async Task<object> GetProjectPerCareerStatisticalBox(int status, string startDate, string endDate, List<Guid> careers, Guid? areaId, Guid? lineId, List<Guid> programs)
        {
            return await _projectRepository.GetProjectPerCareerStatisticalBox(status, startDate, endDate, careers, areaId, lineId, programs);
        }
        public async Task<object> GetBudgetPerCareerStatisticalBox(int status, string startDate, string endDate, List<Guid> careers, Guid? areaId, Guid? lineId, List<Guid> programs)
        {
            return await _projectRepository.GetBudgetPerCareerStatisticalBox(status, startDate, endDate, careers, areaId, lineId, programs);
        }

        public async Task<object> GetOnGoingInvestigationChart(int investigationType, Guid? academicProgramId = null)
            => await _projectRepository.GetOnGoingInvestigationChart(investigationType, academicProgramId);
    }
}
