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
    public class InstitutionalActivityRepository : Repository<InstitutionalActivity>, IInstitutionalActivityRepository
    {
        public InstitutionalActivityRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> ExistAnyWithName(Guid id, string name)
        {
            return _context.InstitutionalActivities.AsEnumerable().Any(x => x.Id != id && x.Title.Normalize() == name.Normalize());
        }

        public async Task<IEnumerable<InstitutionalActivity>> GetBySlug(string slug)
        {
            return await _context.InstitutionalActivities.Include(x => x.Files).Where(x => x.Slug == slug).ToListAsync();
        }
    }
}
