using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class ExoneratePaymentService : IExoneratePaymentService
    {
        private readonly IExoneratePaymentRepository _exoneratePaymentRepository;
        public ExoneratePaymentService(IExoneratePaymentRepository exoneratePaymentRepository)
        {
            _exoneratePaymentRepository = exoneratePaymentRepository;
        }

        public async Task Insert(ExoneratePayment exoneratePayment) => await _exoneratePaymentRepository.Insert(exoneratePayment);
    }
}
