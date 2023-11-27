using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IPhaseRequirementRepository : IRepository<PhaseRequirement>
    {
        Task<object> GetSelectProcess(Guid id);
    }
}
