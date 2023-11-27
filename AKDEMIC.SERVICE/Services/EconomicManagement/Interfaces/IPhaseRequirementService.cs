using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IPhaseRequirementService
    {
        Task InsertPhaseRequirement(PhaseRequirement phaseRequirement);
        Task UpdatePhaseRequirement(PhaseRequirement phaseRequirement);
        Task DeletePhaseRequirement(PhaseRequirement phaseRequirement);
        Task<PhaseRequirement> GetPhaseRequirementById(Guid id);
        Task<IEnumerable<PhaseRequirement>> GetAllPhaseRequirements();
        Task<object> GetSelectProcess(Guid id);
        Task AddAsync(PhaseRequirement phaseRequirement);
    }
}
