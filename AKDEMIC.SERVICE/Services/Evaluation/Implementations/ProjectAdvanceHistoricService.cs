using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.SERVICE.Services.Evaluation.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Implementations
{
    public class ProjectAdvanceHistoricService : IProjectAdvanceHistoricService
    {
        private readonly IProjectAdvanceHistoricRepository _projectAdvanceHistoricRepository;
        public ProjectAdvanceHistoricService(IProjectAdvanceHistoricRepository projectAdvanceHistoricRepository)
        {
            _projectAdvanceHistoricRepository = projectAdvanceHistoricRepository;
        }

        public async Task<ProjectAdvanceHistoric> Get(Guid id)
        {
            return await _projectAdvanceHistoricRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAdvanceHistoricDataTable(DataTablesStructs.SentParameters parameters, Guid id, string search = null)
        {
            return await _projectAdvanceHistoricRepository.GetAdvanceHistoricDataTable(parameters, id, search);
        }

        public async Task<ProjectAdvanceHistoric> GetWithProjectAdvance(Guid id)
        {
            return await _projectAdvanceHistoricRepository.GetWithProjectAdvance(id);
        }

        public async Task Insert(ProjectAdvanceHistoric projectAdvanceHistoric)
        {
            await _projectAdvanceHistoricRepository.Insert(projectAdvanceHistoric);
        }

        public async Task Update(ProjectAdvanceHistoric update)
        {
            await _projectAdvanceHistoricRepository.Update(update);
        }
    }
}
