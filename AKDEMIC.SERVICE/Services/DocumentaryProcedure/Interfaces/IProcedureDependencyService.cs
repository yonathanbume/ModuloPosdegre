using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IProcedureDependencyService
    {
        Task<IEnumerable<ProcedureDependency>> GetAll();
        Task<ProcedureDependency> First();
        Task<ProcedureDependency> FirstProcedureDependencyByProcedure(Guid procedureId);
        Task<ProcedureDependency> Last();
        Task<ProcedureDependency> LastProcedureDependencyByProcedure(Guid procedureId);
        Task<ProcedureDependency> Get(Guid id);
        Task<ProcedureDependency> GetProcedureDependency(Guid id);
        Task<IEnumerable<ProcedureDependency>> GetActiveProcedureDependencies();
        Task<IEnumerable<ProcedureDependency>> GetProcedureDependenciesByDependency(Guid dependencyId, Guid procedureId);
        Task<IEnumerable<ProcedureDependency>> GetProcedureDependenciesByProcedure(Guid procedureId);
        Task Delete(ProcedureDependency procedureDependency);
        Task Insert(ProcedureDependency procedureDependency);
        Task Update(ProcedureDependency procedureDependency);
        Task<Guid> GetGuidProcedureDependecy(Guid procedureId);
        Task<List<Guid>> GetIdProcedureDependecies(List<Guid> userDependecies);
        Task<bool> AnyDependencyByProcedureDependecy(Guid procedureId, Guid dependencyId);
    }
}
