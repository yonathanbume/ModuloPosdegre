using System;
using System.Threading.Tasks;

using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.SERVICE.Services.Investigation.Interfaces;

namespace AKDEMIC.SERVICE.Services.Investigation.Implementations
{
    public class ProjectScheduleService : IProjectScheduleService
    {
        private readonly IProjectScheduleRepository _projectScheduleRepository;
        public ProjectScheduleService(IProjectScheduleRepository projectScheduleRepository)
        {
            _projectScheduleRepository = projectScheduleRepository;
        }
        public async Task<int> Count(Guid projectId)
        {
            return await _projectScheduleRepository.Count(projectId);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectSchedulesDatatable(DataTablesStructs.SentParameters sentParameters, Guid projectId)
        {
            return await _projectScheduleRepository.GetProjectSchedulesDatatable(sentParameters, projectId);
        }
        public async Task DeleteById(Guid id)
        {
            await _projectScheduleRepository.DeleteById(id);
        }
        public async Task Insert(ProjectSchedule schedule)
        {
            await _projectScheduleRepository.Insert(schedule);
        }
        public async Task Update(ProjectSchedule schedule)
        {
            await _projectScheduleRepository.Update(schedule);
        }
    }
}
