using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ITransparencyPortalInterestLinkService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search, byte? type = null);
        Task Insert(TransparencyPortalInterestLink entity);
        Task Update(TransparencyPortalInterestLink entity);
        Task Delete(TransparencyPortalInterestLink entity);
        Task<TransparencyPortalInterestLink> Get(Guid id);
        Task<IEnumerable<TransparencyPortalInterestLink>> GetAll();
    }
}
