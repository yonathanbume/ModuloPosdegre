using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class UserProcedureDerivationRepository : Repository<UserProcedureDerivation>, IUserProcedureDerivationRepository
    {
        public UserProcedureDerivationRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<UserProcedureDerivation>> GetUserProcedureDerivationsByUserProcedure(Guid userProcedureId)
        {
            var query = _context.UserProcedureDerivations
                .Where(x => x.UserProcedureId == userProcedureId)
                .SelectUserProcedureDerivation();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserProcedureDerivation>> GetByUserInternalProcedure(Guid id)
            => await _context.UserProcedureDerivations
                .Where(x => x.UserProcedureId == id).ToListAsync();
    }
}
