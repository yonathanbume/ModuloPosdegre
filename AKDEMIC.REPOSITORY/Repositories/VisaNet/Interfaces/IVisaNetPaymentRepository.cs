using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.VisaNet.Templates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.VisaNet.Interfaces
{
    public interface IVisaNetPaymentRepository
    {
        Task<SessionKeyResponseTemplate> GetSecurityToken(string totalAmount, string merchantId, string uriBase, string visaCredentials);
        Task<VisaApiResponseTemplate> SendVisaBill(string mainToken, string amount, string apiResponseToken, int purchaseNumber, string merchantId, string uriBase);
    }
}
