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
    public class InstitutionalActivityFilesRepository : Repository<InstitutionalActivityFile>, IInstitutionalActivityFilesRepository
    {
        public InstitutionalActivityFilesRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<InstitutionalActivityFile>> GetByInstitutionalActivityId(Guid id)
        {
            return await _context.InstitutionalActivityFiles.Where(x => x.InstitutionalActivityId == id).ToListAsync();
        }
    }
}
