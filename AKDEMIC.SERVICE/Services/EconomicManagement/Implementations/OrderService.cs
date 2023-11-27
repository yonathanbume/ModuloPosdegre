using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<IEnumerable<Order>> GetAll()
            => await _orderRepository.GetAll();
        public async Task<int> Count()
        {
            return await _orderRepository.Count();
        }

        public async Task<int> CountBySupplier(Guid supplierId)
        {
            return await _orderRepository.CountBySupplier(supplierId);
        }

        public async Task<Order> Get(Guid id)
        {
            return await _orderRepository.Get(id);
        }

        public async Task<Order> GetWithData(Guid id)
            => await _orderRepository.GetWithData(id);

        public async Task<Order> GetByUserRequirementOn(Guid userRequirementId)
        {
            return await _orderRepository.GetByUserRequirementOn(userRequirementId);
        }

        public async Task<IEnumerable<Order>> GetOrdersBySupplier(Guid supplierId, string searchValue = null)
        {
            return await _orderRepository.GetOrdersBySupplier(supplierId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Order>> GetOrdersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _orderRepository.GetOrdersDatatable(sentParameters, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Order>> GetOrdersDatatableByEndDate(DataTablesStructs.SentParameters sentParameters, DateTime endDate, string searchValue = null)
        {
            return await _orderRepository.GetOrdersDatatableByEndDate(sentParameters, endDate, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Order>> GetOrdersDatatableByStartDate(DataTablesStructs.SentParameters sentParameters, DateTime startDate, string searchValue = null)
        {
            return await _orderRepository.GetOrdersDatatableByStartDate(sentParameters, startDate, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Order>> GetOrdersDatatableBySupplier(DataTablesStructs.SentParameters sentParameters, Guid supplierId, string searchValue = null)
        {
            return await _orderRepository.GetOrdersDatatableBySupplier(sentParameters, supplierId, searchValue);
        }

        public async Task Insert(Order order) =>
            await _orderRepository.Insert(order);

        public async Task Update(Order order) =>
            await _orderRepository.Update(order);

        public async Task<bool> ValidateOrderNumber(Guid id, int orderNumber)
        {
            return await _orderRepository.ValidateOrderNumber(id,orderNumber);
        }
        public async Task<Order> GetByNumber(int orderNumber)
            => await _orderRepository.GetByNumber(orderNumber);
        public async Task<DataTablesStructs.ReturnedData<object>> GetOrderListDeliveryDatatable(DataTablesStructs.SentParameters sentParameters, string search)
            => await _orderRepository.GetOrderListDeliveryDatatable(sentParameters, search);
        public async Task<DataTablesStructs.ReturnedData<object>> GetOrderListNoDeliveryDatatable(DataTablesStructs.SentParameters sentParameters, string search)
    => await _orderRepository.GetOrderListNoDeliveryDatatable(sentParameters, search);
        public async Task<DataTablesStructs.ReturnedData<object>> GetObtainDateDatatable(DataTablesStructs.SentParameters sentParameters, string searchValueSupplier, string searchValueOrderCode, string searchValue)
            => await _orderRepository.GetObtainDateDatatable(sentParameters, searchValueSupplier, searchValueOrderCode, searchValue);
        public async Task<DataTablesStructs.ReturnedData<object>> GetOrderListDatatable(DataTablesStructs.SentParameters sentParameters, Guid? dependencyId)
            => await _orderRepository.GetOrderListDatatable(sentParameters, dependencyId);
        public async Task<bool> AnyAsync(int number)
            => await _orderRepository.AnyAsync(number);
        public async Task AddAsync(Order order)
            => await _orderRepository.Add(order);

        public async Task<List<Order>> GetAllByDependency(Guid dependencyId)
            => await _orderRepository.GetAllByDependency(dependencyId);
    }
}
