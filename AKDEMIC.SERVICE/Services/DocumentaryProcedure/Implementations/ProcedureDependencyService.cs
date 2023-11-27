using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class ProcedureDependencyService : IProcedureDependencyService
    {
        private readonly IProcedureDependencyRepository _procedureDependencyRepository;

        public ProcedureDependencyService(IProcedureDependencyRepository procedureDependencyRepository)
        {
            _procedureDependencyRepository = procedureDependencyRepository;
        }

        public async Task<IEnumerable<ProcedureDependency>> GetAll()
            => await _procedureDependencyRepository.GetAll();
        public async Task<ProcedureDependency> First()
        {
            return await _procedureDependencyRepository.First();
        }

        public async Task<ProcedureDependency> FirstProcedureDependencyByProcedure(Guid procedureId)
        {
            return await _procedureDependencyRepository.FirstProcedureDependencyByProcedure(procedureId);
        }

        public async Task<ProcedureDependency> Last()
        {
            return await _procedureDependencyRepository.Last();
        }

        public async Task<ProcedureDependency> LastProcedureDependencyByProcedure(Guid procedureId)
        {
            return await _procedureDependencyRepository.LastProcedureDependencyByProcedure(procedureId);
        }

        public async Task<ProcedureDependency> Get(Guid id)
        {
            return await _procedureDependencyRepository.Get(id);
        }

        public async Task<ProcedureDependency> GetProcedureDependency(Guid id)
        {
            return await _procedureDependencyRepository.GetProcedureDependency(id);
        }

        public async Task<IEnumerable<ProcedureDependency>> GetActiveProcedureDependencies()
        {
            return await _procedureDependencyRepository.GetActiveProcedureDependencies();
        }

        public async Task<IEnumerable<ProcedureDependency>> GetProcedureDependenciesByDependency(Guid dependencyId, Guid procedureId)
        {
            return await _procedureDependencyRepository.GetProcedureDependenciesByDependency(dependencyId, procedureId);
        }

        public async Task<IEnumerable<ProcedureDependency>> GetProcedureDependenciesByProcedure(Guid procedureId)
        {
            return await _procedureDependencyRepository.GetProcedureDependenciesByProcedure(procedureId);
        }

        public async Task Delete(ProcedureDependency procedureDependency)
        {
            await _procedureDependencyRepository.Delete(procedureDependency);
        }

        public async Task Insert(ProcedureDependency procedureDependency)
        {
            await _procedureDependencyRepository.Insert(procedureDependency);
        }

        public async Task Update(ProcedureDependency procedureDependency)
        {
            await _procedureDependencyRepository.Update(procedureDependency);
        }

        public async Task<Guid> GetGuidProcedureDependecy(Guid procedureId)
            => await _procedureDependencyRepository.GetGuidProcedureDependecy(procedureId);

        public async Task<List<Guid>> GetIdProcedureDependecies(List<Guid> userDependecies)
            => await _procedureDependencyRepository.GetIdProcedureDependecies(userDependecies);

        public async Task<bool> AnyDependencyByProcedureDependecy(Guid procedureId, Guid dependencyId)
            => await _procedureDependencyRepository.AnyDependencyByProcedureDependecy(procedureId, dependencyId);
    }
}
