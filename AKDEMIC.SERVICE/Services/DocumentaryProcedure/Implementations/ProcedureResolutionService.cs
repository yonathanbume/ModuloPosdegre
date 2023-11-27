using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class ProcedureResolutionService : IProcedureResolutionService
    {
        private readonly IProcedureResolutionRepository _procedureResolutionRepository;

        public ProcedureResolutionService(IProcedureResolutionRepository procedureResolutionRepository)
        {
            _procedureResolutionRepository = procedureResolutionRepository;
        }

        public async Task<ProcedureResolution> Get(Guid id)
        {
            return await _procedureResolutionRepository.Get(id);
        }

        public async Task<ProcedureResolution> GetProcedureResolution(Guid id)
        {
            return await _procedureResolutionRepository.GetProcedureResolution(id);
        }

        public async Task<IEnumerable<ProcedureResolution>> GetProcedureResolutionsByProcedure(Guid procedureId)
        {
            return await _procedureResolutionRepository.GetProcedureResolutionsByProcedure(procedureId);
        }

        public async Task<DataTablesStructs.ReturnedData<ProcedureResolution>> GetProcedureResolutionsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _procedureResolutionRepository.GetProcedureResolutionsDatatable(sentParameters, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<ProcedureResolution>> GetProcedureResolutionsDatatableByProcedure(DataTablesStructs.SentParameters sentParameters, Guid procedureId, string searchValue = null)
        {
            return await _procedureResolutionRepository.GetProcedureResolutionsDatatableByProcedure(sentParameters, procedureId, searchValue);
        }

        public async Task Delete(ProcedureResolution procedureResolution)
        {
            await _procedureResolutionRepository.Delete(procedureResolution);
        }

        public async Task Insert(ProcedureResolution procedureResolution)
        {
            await _procedureResolutionRepository.Insert(procedureResolution);
        }

        public async Task Update(ProcedureResolution procedureResolution)
        {
            await _procedureResolutionRepository.Update(procedureResolution);
        }
    }
}
