using AKDEMIC.ENTITIES.Models.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Server.Interfaces
{
    public interface IGeneralLinkRoleService
    {
        Task DeleteRange(IEnumerable<GeneralLinkRole> entities);
        Task InsertRange(IEnumerable<GeneralLinkRole> entities);
        Task<List<GeneralLinkRole>> GetGeneralLinkRoles(Guid generalLinkId);
    }
}
