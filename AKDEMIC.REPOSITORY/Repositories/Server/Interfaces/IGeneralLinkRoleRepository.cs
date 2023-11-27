using AKDEMIC.ENTITIES.Models.Server;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Server.Interfaces
{
    public interface IGeneralLinkRoleRepository : IRepository<GeneralLinkRole>
    {
        Task<List<GeneralLinkRole>> GetGeneralLinkRoles(Guid generalLinkId);
    }
}
