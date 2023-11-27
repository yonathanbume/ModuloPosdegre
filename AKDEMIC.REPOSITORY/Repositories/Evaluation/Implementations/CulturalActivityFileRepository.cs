using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Implementations
{
    public class CulturalActivityFileRepository : Repository<CulturalActivityFile>, ICulturalActivityFileRepository
    {
        public CulturalActivityFileRepository(AkdemicContext context) : base(context) { }
        public async Task<IEnumerable<CulturalActivityFile>> GetFilesByActivity(Guid id)
        {
            return await _context.CulturalActivityFiles.Where(x => x.CulturalActivityId == id).ToListAsync();
        }
    }
}
