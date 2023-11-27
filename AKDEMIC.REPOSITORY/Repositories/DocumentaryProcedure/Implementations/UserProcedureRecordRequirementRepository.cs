using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class UserProcedureRecordRequirementRepository : Repository<UserProcedureRecordRequirement>, IUserProcedureRecordRequirementRepository
    {
        public UserProcedureRecordRequirementRepository(AkdemicContext context) : base(context) { }

        public async Task<List<ProcedureRequirement>> GetRecordRequirementsByUserProcedureRecordId(Guid userProcedureRecordId)
        {
            var userProcedureRecord = await _context.UserProcedureRecords
                .Include(x => x.UserProcedure)
                .FirstOrDefaultAsync(x => x.Id == userProcedureRecordId);

            var requirements = await _context.ProcedureRequirements
                .Where(x => x.ProcedureId == userProcedureRecord.UserProcedure.ProcedureId)
                .Select(x => new ProcedureRequirement
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Cost = x.Cost,
                    HasUserProcedureRecordRequirement = _context.UserProcedureRecordRequirements.Any(y => y.ProcedureRequirementId == x.Id && y.UserProcedureRecordId == userProcedureRecord.Id)
                })
                .AsNoTracking()
                .ToListAsync();

            return requirements;
        }

        public async Task<List<UserProcedureRecordRequirement>> GetUserProcedureRecordRequirementsByUserProcedureRecordId(Guid userProcedureRecordId)
        {
            return await _context.UserProcedureRecordRequirements
                .Where(x => x.UserProcedureRecordId == userProcedureRecordId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserProcedureRecordRequirement>> GetUserProcedureRecordRequirementByUserProcedureRecordAsync(Guid userProcedureRecordId)
        {
            var query = _context.UserProcedureRecordRequirements
                .Where(x => x.UserProcedureRecordId == userProcedureRecordId)
                .SelectUserProcedureRecordRequirement()
                .AsQueryable();

            return await query.ToListAsync();
        }
    }
}
