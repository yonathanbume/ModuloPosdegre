using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class PhaseRequirementService : IPhaseRequirementService
    {
        private readonly IPhaseRequirementRepository _phaseRequirementRepository;

        public PhaseRequirementService(IPhaseRequirementRepository phaseRequirementRepository)
        {
            _phaseRequirementRepository = phaseRequirementRepository;
        }

        public async Task InsertPhaseRequirement(PhaseRequirement phaseRequirement) =>
            await _phaseRequirementRepository.Insert(phaseRequirement);

        public async Task UpdatePhaseRequirement(PhaseRequirement phaseRequirement) =>
            await _phaseRequirementRepository.Update(phaseRequirement);

        public async Task DeletePhaseRequirement(PhaseRequirement phaseRequirement) =>
            await _phaseRequirementRepository.Delete(phaseRequirement);

        public async Task<PhaseRequirement> GetPhaseRequirementById(Guid id) =>
            await _phaseRequirementRepository.Get(id);

        public async Task<IEnumerable<PhaseRequirement>> GetAllPhaseRequirements() =>
            await _phaseRequirementRepository.GetAll();

        public async Task<object> GetSelectProcess(Guid id)
            => await _phaseRequirementRepository.GetSelectProcess(id);
        public async Task AddAsync(PhaseRequirement phaseRequirement)
            => await _phaseRequirementRepository.Add(phaseRequirement);
    }
}
