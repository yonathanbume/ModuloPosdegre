using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IOrderChangeService
    {
        Task<int> Count();
        Task<OrderChangeHistory> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<OrderChangeHistory>> GetOrderChangesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task Add(OrderChangeHistory orderChange);
        Task Delete(OrderChangeHistory orderChange);
        Task Insert(OrderChangeHistory orderChange);
        Task Update(OrderChangeHistory orderChange);
        Task<DataTablesStructs.ReturnedData<object>> GetOrderHistory(DataTablesStructs.SentParameters sentParameters, Guid orderId);
    }
}
