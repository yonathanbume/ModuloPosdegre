using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.SERVICE.Services.Cafeteria.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Implementations
{
    public class SupplyOrderDetailService : ISupplyOrderDetailService
    {
        private ISupplyOrderDetailRepository _supplyOrderDetailRepository;
        public SupplyOrderDetailService(ISupplyOrderDetailRepository supplyOrderDetailRepository)
        {
            _supplyOrderDetailRepository = supplyOrderDetailRepository;
        }
        public async Task AddRange(IEnumerable<SupplyOrderDetail> supplyOrderDetails)
        {
            await _supplyOrderDetailRepository.AddRange(supplyOrderDetails);
        }

        public async Task<bool> GetQuantityFinished(Guid purchaseOrderId)
        {
           return await _supplyOrderDetailRepository.GetQuantityFinished(purchaseOrderId);
        }

        public async Task UpdateRange(IEnumerable<SupplyOrderDetail> supplyOrderDetails)
        {
            await _supplyOrderDetailRepository.UpdateRange(supplyOrderDetails);
        }

        public async Task<SupplyOrderDetail> Get(Guid id)
        {
            return await _supplyOrderDetailRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSupplyOrderDetailDatatable(DataTablesStructs.SentParameters sentParameters, Guid supplyOrderId,byte turnId, string searchValue = null)
        {
            return await _supplyOrderDetailRepository.GetSupplyOrderDetailDatatable(sentParameters,supplyOrderId, turnId,searchValue);
        }

        public async Task<object> GetSupplyOrderDetailDatatableClientSide(Guid supplyOrderId)
        {
            return await _supplyOrderDetailRepository.GetSupplyOrderDetailDatatableClientSide(supplyOrderId);
        }
    }
}
