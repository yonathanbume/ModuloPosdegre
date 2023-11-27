using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class UserProcedureFileRepository : Repository<UserProcedureFile>, IUserProcedureFileRepository
    {
        public UserProcedureFileRepository(AkdemicContext context) : base(context) { }

        public async Task<List<UserProcedureFile>> GetUserProcedureFiles(Guid userProcedureId)
            => await _context.UserProcedureFiles.Where(x => x.UserProcedureId == userProcedureId).Include(x=>x.ProcedureRequirement).ToListAsync();
    }
}
