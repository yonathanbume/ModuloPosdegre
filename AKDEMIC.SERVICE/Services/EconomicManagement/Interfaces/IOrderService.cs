using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAll();
        Task<int> Count();
        Task<int> CountBySupplier(Guid supplierId);
        Task<Order> Get(Guid id);
        Task<Order> GetWithData(Guid id);
        Task<Order> GetByNumber(int orderNumber);
        Task<bool> ValidateOrderNumber(Guid id, int orderNumber);
        Task<Order> GetByUserRequirementOn(Guid userRequirementId);
        Task<IEnumerable<Order>> GetOrdersBySupplier(Guid supplierId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<Order>> GetOrdersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<Order>> GetOrdersDatatableByEndDate(DataTablesStructs.SentParameters sentParameters, DateTime endDate, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<Order>> GetOrdersDatatableByStartDate(DataTablesStructs.SentParameters sentParameters, DateTime startDate, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<Order>> GetOrdersDatatableBySupplier(DataTablesStructs.SentParameters sentParameters, Guid supplierId, string searchValue = null);
        Task Insert(Order order);
        Task Update(Order order);
        Task<DataTablesStructs.ReturnedData<object>> GetOrderListDeliveryDatatable(DataTablesStructs.SentParameters sentParameters, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetOrderListNoDeliveryDatatable(DataTablesStructs.SentParameters sentParameters, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetObtainDateDatatable(DataTablesStructs.SentParameters sentParameters, string searchValueSupplier, string searchValueOrderCode, string searchValue);
        Task<DataTablesStructs.ReturnedData<object>> GetOrderListDatatable(DataTablesStructs.SentParameters sentParameters, Guid? dependencyId);
        Task<bool> AnyAsync(int number);
        Task AddAsync(Order order);
        Task<List<Order>> GetAllByDependency(Guid dependencyId);
    }
}
