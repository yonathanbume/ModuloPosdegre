using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Server;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Server.Interfaces
{
    public interface IGeneralLinkRepository : IRepository<GeneralLink>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, byte? type);
        Task<IEnumerable<GeneralLink>> GetAll(byte? type = null, ClaimsPrincipal user = null);
    }
}
