using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.PortalNew;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IPortalNewService
    {
        Task<PortalNew> Get(Guid id);
        Task<IEnumerable<PortalNew>> GetAll();
        Task<List<PortalNewTemplate>> GeNextUpcomingNews(int newsCount = 5); 
        Task Insert(PortalNew portalNew);
        Task Update(PortalNew portalNew);
        Task Delete(PortalNew portalNew);
        Task<DataTablesStructs.ReturnedData<object>> GetPortalNewsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
