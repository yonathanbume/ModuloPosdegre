using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.SERVICE.Services.Evaluation.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Implementations
{
    public class ProjectScheduleHistoricService : IProjectScheduleHistoricService
    {
        private readonly IProjectScheduleHistoricRepository _projectScheduleHistoricRepository;

        public ProjectScheduleHistoricService(IProjectScheduleHistoricRepository projectScheduleHistoricRepository)
        {
            _projectScheduleHistoricRepository = projectScheduleHistoricRepository;
        }

        public async Task<ProjectScheduleHistoric> Get(Guid id)
            => await _projectScheduleHistoricRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<ProjectScheduleHistoric>> GetProjectScheduleHistoricDatatable(DataTablesStructs.SentParameters sentParameters, Guid projectId)
            => await _projectScheduleHistoricRepository.GetProjectScheduleHistoricDatatable(sentParameters, projectId);

        public async Task Insert(ProjectScheduleHistoric entity)
            => await _projectScheduleHistoricRepository.Insert(entity);
    }
}
