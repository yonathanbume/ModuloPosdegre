using AKDEMIC.ENTITIES.Models.Server;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Server.Implementations
{
    public class GeneralLinkRoleRepository : Repository<GeneralLinkRole> ,IGeneralLinkRoleRepository
    {
        public GeneralLinkRoleRepository(AkdemicContext context) : base(context) { }

        public async Task<List<GeneralLinkRole>> GetGeneralLinkRoles(Guid generalLinkId)
            => await _context.GeneralLinkRoles.Where(x => x.GeneralLinkId == generalLinkId).ToListAsync();

    }
}
