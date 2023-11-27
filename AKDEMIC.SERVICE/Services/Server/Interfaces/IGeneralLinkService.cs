using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Server;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Server.Interfaces
{
    public interface IGeneralLinkService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, byte? type);
        Task Insert(GeneralLink entity);
        Task Update(GeneralLink entity);
        Task Delete(GeneralLink entity);
        Task<GeneralLink> Get(Guid id);
        Task<IEnumerable<GeneralLink>> GetAll(byte? type = null, ClaimsPrincipal user = null);
    }
}
