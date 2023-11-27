using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class ProcedureRequirementService : IProcedureRequirementService
    {
        private readonly IProcedureRequirementRepository _procedureRequirementRepository;

        public ProcedureRequirementService(IProcedureRequirementRepository procedureRequirementRepository)
        {
            _procedureRequirementRepository = procedureRequirementRepository;
        }

        public async Task<ProcedureRequirement> Get(Guid id)
        {
            return await _procedureRequirementRepository.Get(id);
        }

        public async Task<ProcedureRequirement> GetProcedureRequirement(Guid id)
        {
            return await _procedureRequirementRepository.GetProcedureRequirement(id);
        }

        public async Task<decimal> GetProcedureRequirementsCostSumByProcedure(Guid procedureId)
        {
            return await _procedureRequirementRepository.GetProcedureRequirementsCostSumByProcedure(procedureId);
        }

        public async Task<IEnumerable<ProcedureRequirement>> GetProcedureRequirements()
        {
            return await _procedureRequirementRepository.GetProcedureRequirements();
        }

        public async Task<IEnumerable<ProcedureRequirement>> GetProcedureRequirementsByProcedure(Guid procedureId)
        {
            return await _procedureRequirementRepository.GetProcedureRequirementsByProcedure(procedureId);
        }

        public async Task<IEnumerable<ProcedureRequirement>> GetProcedureRequirementsByUserProcedure(Guid userProcedureId)
        {
            return await _procedureRequirementRepository.GetProcedureRequirementsByUserProcedure(userProcedureId);
        }

        public async Task<DataTablesStructs.ReturnedData<ProcedureRequirement>> GetProcedureRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _procedureRequirementRepository.GetProcedureRequirementsDatatable(sentParameters, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<ProcedureRequirement>> GetProcedureRequirementsDatatableByProcedure(DataTablesStructs.SentParameters sentParameters, Guid procedureId, string searchValue = null)
        {
            return await _procedureRequirementRepository.GetProcedureRequirementsDatatableByProcedure(sentParameters, procedureId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<ProcedureRequirement>> GetProcedureRequirementsDatatableByUserProcedure(DataTablesStructs.SentParameters sentParameters, Guid userProcedureId, string searchValue = null)
        {
            return await _procedureRequirementRepository.GetProcedureRequirementsDatatableByUserProcedure(sentParameters, userProcedureId, searchValue);
        }

        public async Task Delete(ProcedureRequirement procedureRequirement)
        {
            await _procedureRequirementRepository.Delete(procedureRequirement);
        }

        public async Task Insert(ProcedureRequirement procedureRequirement)
        {
            await _procedureRequirementRepository.Insert(procedureRequirement);
        }

        public async Task Update(ProcedureRequirement procedureRequirement)
        {
            await _procedureRequirementRepository.Update(procedureRequirement);
        }



















        public async Task<IEnumerable<ProcedureRequirement>> GetProcedureRequirementsByUserProcedureRecord(Guid userProcedureRecordId)
        {
            return await _procedureRequirementRepository.GetProcedureRequirementsByUserProcedureRecord(userProcedureRecordId);
        }
        public async Task<decimal> GetProcedureAmount(Guid id)
            => await _procedureRequirementRepository.GetProcedureAmount(id);
    }
}
