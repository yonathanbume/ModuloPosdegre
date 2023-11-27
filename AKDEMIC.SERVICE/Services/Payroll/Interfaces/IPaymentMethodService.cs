using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethod>> GetAll();

        Task<object> GetPaymentMethods();

        Task<PaymentMethod> Get(Guid id);

        Task ChangeStatus(Guid id);

        Task Insert(PaymentMethod paymentMethod);

        Task Update(PaymentMethod paymentMethod);

        Task Delete(Guid id);

        Task<DataTablesStructs.ReturnedData<object>> GetAllPaymentMethodsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
