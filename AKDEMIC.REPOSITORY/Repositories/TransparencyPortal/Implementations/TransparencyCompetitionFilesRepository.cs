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
    public class TransparencyCompetitionFilesRepository : Repository<TransparencyCompetitionFile>, ITransparencyCompetitionFilesRepository
    {
        public TransparencyCompetitionFilesRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<TransparencyCompetitionFile>> GetByTransparencyCompetitionId(Guid id)
        {
            return await _context.TransparencyCompetitionFiles.Where(x => x.TransparencyCompetitionId == id).ToListAsync();
        }
    }
}
