using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IOrderChangeFileRepository : IRepository<OrderChangeFileHistory>
    {
        Task<DataTablesStructs.ReturnedData<OrderChangeFileHistory>> GetOrderChangeFilesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetOrderChangeFilesByOrderDatatable(DataTablesStructs.SentParameters sentParameters, Guid orderId);
        Task<IEnumerable<OrderChangeFileHistory>> CompareFilesFromOrder(Guid orderId, IEnumerable<Guid> ids);
        void DeleteRangeWithOutSaving(IEnumerable<OrderChangeFileHistory> orderChangeFiles);
        Task<IEnumerable<OrderChangeFileHistory>> GetAllByOrder(Guid orderId);
        Task<DataTablesStructs.ReturnedData<object>> GetOrderChangeFileDatatable(DataTablesStructs.SentParameters sentParameters, Guid id);
    }
}
