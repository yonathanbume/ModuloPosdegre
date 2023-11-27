using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IProcedureDependencyRepository : IRepository<ProcedureDependency>
    {
        Task<ProcedureDependency> FirstProcedureDependencyByProcedure(Guid procedureId);
        Task<ProcedureDependency> LastProcedureDependencyByProcedure(Guid procedureId);
        Task<ProcedureDependency> GetProcedureDependency(Guid id);
        Task<IEnumerable<ProcedureDependency>> GetActiveProcedureDependencies();
        Task<IEnumerable<ProcedureDependency>> GetProcedureDependenciesByDependency(Guid dependencyId, Guid procedureId);
        Task<IEnumerable<ProcedureDependency>> GetProcedureDependenciesByProcedure(Guid procedureId);
        Task<Guid> GetGuidProcedureDependecy(Guid procedureId);
        Task<List<Guid>> GetIdProcedureDependecies(List<Guid> userDependecies);
        Task<bool> AnyDependencyByProcedureDependecy(Guid procedureId, Guid dependencyId);
    }
}
