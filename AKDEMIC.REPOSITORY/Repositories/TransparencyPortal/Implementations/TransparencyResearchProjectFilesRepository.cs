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
    public class TransparencyResearchProjectFilesRepository : Repository<TransparencyResearchProjectFile>, ITransparencyResearchProjectFilesRepository
    {
        public TransparencyResearchProjectFilesRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<TransparencyResearchProjectFile>> GetByTransparencyResearchProjectId(Guid id)
        {
            return await _context.TransparencyResearchProjectFiles.Where(x => x.TransparencyResearchProjectId == id).ToListAsync();
        }
    }
}
