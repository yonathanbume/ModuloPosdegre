using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IOrderChangeFileService
    {
        Task<DataTablesStructs.ReturnedData<OrderChangeFileHistory>> GetOrderChangeFilesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetOrderChangeFilesByOrderDatatable(DataTablesStructs.SentParameters sentParameters,Guid orderId);
        Task Delete(OrderChangeFileHistory orderChangeFile);
        Task Insert(OrderChangeFileHistory orderChangeFile);
        void DeleteRangeWithOutSaving(IEnumerable<OrderChangeFileHistory> orderChangeFiles);
        Task Add(OrderChangeFileHistory orderChangeFile);
        Task<OrderChangeFileHistory> Get(Guid id);
        Task<IEnumerable<OrderChangeFileHistory>> GetAllByOrder(Guid orderId);
        Task<IEnumerable<OrderChangeFileHistory>> CompareFilesFromOrder(Guid orderId,IEnumerable<Guid> ids);
        Task<DataTablesStructs.ReturnedData<object>> GetOrderChangeFileDatatable(DataTablesStructs.SentParameters sentParameters, Guid id);
    }
}
