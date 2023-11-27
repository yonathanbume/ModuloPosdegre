using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Implementations
{
    public class SessionRecordRepository : Repository<SessionRecord>, ISessionRecordRepository
    {
        public SessionRecordRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> ExistAnyWithName(Guid id, string name)
        {
            return _context.SessionRecords.AsEnumerable().Any(x => x.Id != id && x.Name.Normalize() == name.Normalize());
        }

        public async Task<SessionRecord> GetBySlug(string slug)
        {
            return await _context.SessionRecords.Where(x => x.Slug == slug).FirstOrDefaultAsync();
        }
    }
}
