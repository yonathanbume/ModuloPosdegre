using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class PaymentToValidateService : IPaymentToValidateService
    {
        private readonly IPaymentToValidateRepository _paymentToValidateRepository;
        public PaymentToValidateService(IPaymentToValidateRepository paymentToValidateRepository)
        {
            _paymentToValidateRepository = paymentToValidateRepository;
        }

        public async Task DeleteById(int id)
            => await _paymentToValidateRepository.DeleteById(id);

        public async Task<PaymentToValidate> Get(int id)
            => await _paymentToValidateRepository.Get(id);

        public async Task<IEnumerable<PaymentToValidate>> GetAll()
            => await _paymentToValidateRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search)
            => await _paymentToValidateRepository.GetDatatable(sentParameters, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetExternalPaymentsDatatable(DataTablesStructs.SentParameters sentParameters, string search)
            => await _paymentToValidateRepository.GetExternalPaymentsDatatable(sentParameters, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserPaymentsDatatable(int paymentToValidateId)
            => await _paymentToValidateRepository.GetUserPaymentsDatatable(paymentToValidateId);

        public async Task ProcessAllPayments(ClaimsPrincipal user)
            => await _paymentToValidateRepository.ProcessAllPayments(user);

        public async Task<Tuple<bool, string>> ProcessUserPayments(int paymentToValidateId, List<Guid> payments, ClaimsPrincipal user)
            => await _paymentToValidateRepository.ProcessUserPayments(paymentToValidateId, payments, user);
    }
}
