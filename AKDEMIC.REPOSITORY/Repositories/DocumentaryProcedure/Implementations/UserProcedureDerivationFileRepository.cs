using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class UserProcedureDerivationFileRepository : Repository<UserProcedureDerivationFile>, IUserProcedureDerivationFileRepository
    {
        public UserProcedureDerivationFileRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<UserProcedureDerivationFile>> GetUserProcedureDerivationFilesByUserProcedure(Guid userProcedureId)
        {
            var query = _context.UserProcedureDerivationFiles.Include(x => x.UserProcedureDerivation).ThenInclude(x => x.DependencyFrom)
                .Where(x => x.UserProcedureDerivation.UserProcedureId == userProcedureId)
                .AsQueryable();

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<UserProcedureDerivationFile>> GetUserProcedureDerivationFilesByUserProcedurept2(Guid userProcedureId)
        {
            var query = _context.UserProcedureDerivationFiles
                .Where(x => x.UserProcedureDerivation.UserProcedureId == userProcedureId)
                .AsQueryable();

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<UserProcedureDerivationFile>> GetUserProcedureDerivationFilesByUserProcedureDerivation(Guid userProcedureDerivationId)
        {
            var query = _context.UserProcedureDerivationFiles
                .Where(x => x.UserProcedureDerivationId == userProcedureDerivationId)
                .AsQueryable();

            return await query.ToListAsync();
        }
    }
}
