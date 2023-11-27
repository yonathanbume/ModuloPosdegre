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
    public class TransparencyPublicInformationFilesRepository : Repository<TransparencyPublicInformationFile>, ITransparencyPublicInformationFilesRepository
    {
        public TransparencyPublicInformationFilesRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<TransparencyPublicInformationFile>> GetByTransparencyPublicInformationId(Guid id)
        {
            return await _context.TransparencyPublicInformationFiles.Where(x => x.TransparencyPublicInformationId == id).ToListAsync();
        }
    }
}
