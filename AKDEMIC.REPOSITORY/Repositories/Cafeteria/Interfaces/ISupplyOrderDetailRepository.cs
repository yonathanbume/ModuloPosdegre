using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces
{
    public interface ISupplyOrderDetailRepository : IRepository<SupplyOrderDetail>
    {        
        Task<bool> GetQuantityFinished(Guid purchaseOrderId);
        Task<DataTablesStructs.ReturnedData<object>> GetSupplyOrderDetailDatatable(DataTablesStructs.SentParameters sentParameters, Guid supplyOrderId,byte turnId, string searchValue = null);
        Task<object> GetSupplyOrderDetailDatatableClientSide(Guid purchaseOrderId);
    }
}
