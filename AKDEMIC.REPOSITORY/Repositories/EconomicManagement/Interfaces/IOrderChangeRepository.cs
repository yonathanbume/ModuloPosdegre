using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IOrderChangeRepository : IRepository<OrderChangeHistory>
    {
        Task<DataTablesStructs.ReturnedData<OrderChangeHistory>> GetOrderChangesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetOrderHistory(DataTablesStructs.SentParameters sentParameters, Guid orderId);
    }
}
