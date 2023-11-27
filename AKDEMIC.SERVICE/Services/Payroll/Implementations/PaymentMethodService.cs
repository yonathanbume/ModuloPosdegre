using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<object> GetPaymentMethods()
            => await _paymentMethodRepository.GetPaymentMethods();
        public async Task ChangeStatus(Guid id) => await _paymentMethodRepository.ChangeStatus(id);

        public async Task Delete(Guid id) => await _paymentMethodRepository.DeleteById(id);

        public async Task<PaymentMethod> Get(Guid id) => await _paymentMethodRepository.Get(id);

        public async Task<IEnumerable<PaymentMethod>> GetAll() => await _paymentMethodRepository.GetAll();

        public async Task Insert(PaymentMethod paymentMethod) => await _paymentMethodRepository.Insert(paymentMethod);

        public async Task Update(PaymentMethod paymentMethod) => await _paymentMethodRepository.Update(paymentMethod);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllPaymentMethodsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _paymentMethodRepository.GetAllPaymentMethodsDatatable(sentParameters, searchValue);
    }
}
