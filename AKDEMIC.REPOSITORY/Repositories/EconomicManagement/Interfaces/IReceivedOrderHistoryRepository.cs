using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IReceivedOrderHistoryRepository : IRepository<ReceivedOrderHistory>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatableByReceivedOrderId(DataTablesStructs.SentParameters sentParameters, Guid receivedOrderId);
    }
}
