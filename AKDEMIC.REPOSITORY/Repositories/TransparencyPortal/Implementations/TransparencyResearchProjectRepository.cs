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
    public class TransparencyResearchProjectRepository : Repository<TransparencyResearchProject>, ITransparencyResearchProjectRepository
    {
        public TransparencyResearchProjectRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> ExistAnyWithName(Guid id, string name)
        {
            if (id == Guid.Empty)
                return _context.TransparencyResearchProjects.AsEnumerable().Any(x => x.Title.Normalize() == name.Normalize());
            return _context.TransparencyResearchProjects.AsEnumerable().Any(x => x.Id != id && x.Title.Normalize() == name.Normalize());
        }

        public async Task<IEnumerable<TransparencyResearchProject>> GetBySlug(string slug)
        {
            return await _context.TransparencyResearchProjects.Include(x => x.Files).Where(x => x.Slug == slug).ToListAsync();
        }
    }
}
