using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.PortalNew;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IPortalNewRepository : IRepository<PortalNew>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetPortalNewsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<List<PortalNewTemplate>> GeNextUpcomingNews(int newsCount = 5);
    }
}
