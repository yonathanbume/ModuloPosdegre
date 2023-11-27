using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces
{
    public interface IPaymentMethodRepository : IRepository<PaymentMethod>
    {
        Task ChangeStatus(Guid id);
        Task<object> GetPaymentMethods();

        Task<DataTablesStructs.ReturnedData<object>> GetAllPaymentMethodsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
