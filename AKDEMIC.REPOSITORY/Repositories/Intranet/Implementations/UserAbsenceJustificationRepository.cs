using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class UserAbsenceJustificationRepository : Repository<UserAbsenceJustification>, IUserAbsenceJustificationRepository
    {
        public UserAbsenceJustificationRepository(AkdemicContext context) : base(context)
        {
        }

        public override async Task<UserAbsenceJustification> Get(Guid id)
            => await _context.UserAbsenceJustifications
                .Include(x => x.WorkingDay.User)
                .Where(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<bool> Any(Guid workingDayId, int? status = null)
        {
            var query = _context.UserAbsenceJustifications
                .Where(x => x.WorkingDayId == workingDayId)
                .AsNoTracking();
            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);
            return await query.AnyAsync();
        }

        public async Task<IEnumerable<UserAbsenceJustification>> GetAll(Guid? termId = null, string userId = null)
        {
            var query = _context.UserAbsenceJustifications
                .AsNoTracking();
            if (termId.HasValue)
                query = query.Where(x => x.WorkingDay.TermId == termId.Value);
            if (!string.IsNullOrEmpty(userId))
                query = query.Where(x => x.WorkingDay.UserId == userId);
            return await query.ToListAsync();
        }
    }
}
