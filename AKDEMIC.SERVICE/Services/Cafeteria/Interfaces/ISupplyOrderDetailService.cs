using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Interfaces
{
    public interface ISupplyOrderDetailService
    {
        Task AddRange(IEnumerable<SupplyOrderDetail> supplyOrderDetails);
        Task UpdateRange(IEnumerable<SupplyOrderDetail> supplyOrderDetails);
        Task<bool> GetQuantityFinished(Guid purchaseOrderId);
        Task<SupplyOrderDetail> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetSupplyOrderDetailDatatable(DataTablesStructs.SentParameters sentParameters, Guid supplyOrderId,byte turnId, string searchValue = null);
        Task<object> GetSupplyOrderDetailDatatableClientSide(Guid supplyOrderId);
    }
}
