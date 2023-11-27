using AKDEMIC.REPOSITORY.Repositories.VisaNet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.VisaNet.Templates;
using AKDEMIC.SERVICE.Services.VisaNet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.VisaNet.Implementations
{
    public class VisaNetPaymentService : IVisaNetPaymentService
    {
        private readonly IVisaNetPaymentRepository _visaNetPaymentRepository;

        public VisaNetPaymentService(IVisaNetPaymentRepository visaNetPaymentRepository)
        {
            _visaNetPaymentRepository = visaNetPaymentRepository;
        }

        public Task<SessionKeyResponseTemplate> GetSecurityToken(string totalAmount, string merchantId, string uriBase, string visaCredentials)
            => _visaNetPaymentRepository.GetSecurityToken(totalAmount,merchantId,uriBase,visaCredentials);

        public Task<VisaApiResponseTemplate> SendVisaBill(string mainToken, string amount, string apiResponseToken, int purchaseNumber, string merchantId, string uriBase)
            => _visaNetPaymentRepository.SendVisaBill(mainToken,amount,apiResponseToken,purchaseNumber,merchantId,uriBase);
    }
}
