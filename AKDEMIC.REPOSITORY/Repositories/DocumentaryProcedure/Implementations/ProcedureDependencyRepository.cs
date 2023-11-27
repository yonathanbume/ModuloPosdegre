using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class ProcedureDependencyRepository : Repository<ProcedureDependency>, IProcedureDependencyRepository
    {
        public ProcedureDependencyRepository(AkdemicContext context) : base(context) { }

        #region PUBLIC

        public async Task<ProcedureDependency> FirstProcedureDependencyByProcedure(Guid procedureId)
        {
            var query = _context.ProcedureDependencies
                .Where(x => x.ProcedureId == procedureId)
                .OrderBy(x => x.CreatedAt)
                .SelectProcedureDependency();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<ProcedureDependency> LastProcedureDependencyByProcedure(Guid procedureId)
        {
            var query = _context.ProcedureDependencies
                .Where(x => x.ProcedureId == procedureId)
                .OrderByDescending(x => x.CreatedAt)
                .SelectProcedureDependency();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<ProcedureDependency> GetProcedureDependency(Guid id)
        {
            var query = _context.ProcedureDependencies
                .Where(x => x.Id == id)
                .OrderBy(x => x.CreatedAt)
                .SelectProcedureDependency();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProcedureDependency>> GetActiveProcedureDependencies()
        {
            var query = (from x in _context.Procedures
                         from y in _context.Dependencies
                         select new ProcedureDependency
                         {
                             DependencyId = y.Id,
                             ProcedureId = x.Id,
                             Dependency = new Dependency
                             {
                                 Id = y.Id,
                                 Name = y.Name,
                                 Signature = y.Signature,
                                 IsProcedureDependency = _context.ProcedureDependencies.Any(z => z.DependencyId == y.Id && z.ProcedureId == x.Id)
                             },
                             Procedure = x
                         });

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ProcedureDependency>> GetProcedureDependenciesByDependency(Guid dependencyId, Guid procedureId)
        {
            var query = _context.ProcedureDependencies
                .Where(x => x.DependencyId == dependencyId)
                .Where(x => x.ProcedureId == procedureId)
                .OrderBy(x => x.CreatedAt)
                .SelectProcedureDependency();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ProcedureDependency>> GetProcedureDependenciesByProcedure(Guid procedureId)
        {
            var query = _context.ProcedureDependencies
                .Where(x => x.ProcedureId == procedureId)
                .OrderBy(x => x.CreatedAt)
                .SelectProcedureDependency();

            return await query.ToListAsync();
        }

        public async Task<Guid> GetGuidProcedureDependecy(Guid procedureId)
        {
            var procedureDependency = await _context.ProcedureDependencies.Where(x => x.ProcedureId == procedureId).OrderBy(x => x.CreatedAt).Select(x => x.DependencyId).FirstOrDefaultAsync();

            return procedureDependency;
        }
        public async Task<List<Guid>> GetIdProcedureDependecies(List<Guid> userDependecies)
        {
            var procedureDependencies = await _context.ProcedureDependencies.AsNoTracking().Where(x => userDependecies.Contains(x.DependencyId)).Select(x => x.ProcedureId).ToListAsync();

            return procedureDependencies;
        }

        public async Task<bool> AnyDependencyByProcedureDependecy(Guid procedureId, Guid dependencyId)
            => await _context.ProcedureDependencies.AnyAsync(y => y.ProcedureId == procedureId && y.DependencyId == dependencyId);
        #endregion
    }
}
