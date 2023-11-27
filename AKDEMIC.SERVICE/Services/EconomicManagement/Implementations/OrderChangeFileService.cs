using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class OrderChangeFileService : IOrderChangeFileService
    {
        private readonly IOrderChangeFileRepository _orderChangeFileRepository;

        public OrderChangeFileService(IOrderChangeFileRepository orderChangeFileRepository)
        {
            _orderChangeFileRepository = orderChangeFileRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<OrderChangeFileHistory>> GetOrderChangeFilesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _orderChangeFileRepository.GetOrderChangeFilesDatatable(sentParameters, searchValue);
        }

        public async Task Delete(OrderChangeFileHistory orderChangeFile) =>
            await _orderChangeFileRepository.Delete(orderChangeFile);

        public async Task Insert(OrderChangeFileHistory orderChangeFile) =>
            await _orderChangeFileRepository.Insert(orderChangeFile);

        public async Task<OrderChangeFileHistory> Get(Guid id)
        {
            return await _orderChangeFileRepository.Get(id);
        }

        public async Task<IEnumerable<OrderChangeFileHistory>> CompareFilesFromOrder(Guid orderId ,IEnumerable<Guid> ids)
        {
            return await _orderChangeFileRepository.CompareFilesFromOrder(orderId,ids);
        }

        public void DeleteRangeWithOutSaving(IEnumerable<OrderChangeFileHistory> orderChangeFiles)
        {
            _orderChangeFileRepository.DeleteRangeWithOutSaving(orderChangeFiles);
        }

        public async Task Add(OrderChangeFileHistory orderChangeFile)
        {
            await _orderChangeFileRepository.Add(orderChangeFile);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetOrderChangeFilesByOrderDatatable(DataTablesStructs.SentParameters sentParameters, Guid orderId)
        {
            return await _orderChangeFileRepository.GetOrderChangeFilesByOrderDatatable(sentParameters, orderId);
        }

        public async Task<IEnumerable<OrderChangeFileHistory>> GetAllByOrder(Guid orderId)
        {
            return await _orderChangeFileRepository.GetAllByOrder(orderId);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetOrderChangeFileDatatable(DataTablesStructs.SentParameters sentParameters, Guid id)
            => await _orderChangeFileRepository.GetOrderChangeFileDatatable(sentParameters, id);
    }
}
