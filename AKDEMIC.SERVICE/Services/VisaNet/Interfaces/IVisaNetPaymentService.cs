using AKDEMIC.REPOSITORY.Repositories.VisaNet.Templates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.VisaNet.Interfaces
{
    public interface IVisaNetPaymentService
    {
        Task<SessionKeyResponseTemplate> GetSecurityToken(string totalAmount, string merchantId, string uriBase, string visaCredentials);
        Task<VisaApiResponseTemplate> SendVisaBill(string mainToken, string amount, string apiResponseToken, int purchaseNumber, string merchantId, string uriBase);
    }
}
