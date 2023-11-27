using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IExternalLinkService
    {
        Task<ExternalLink> Get(Guid id);
        Task<IEnumerable<ExternalLink>> GetAll();
        Task Insert(ExternalLink externalLink);
        Task Update(ExternalLink externalLink);
        Task Delete(ExternalLink externalLink);
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters,int? type = null, string searchValue = null);
    }
}
