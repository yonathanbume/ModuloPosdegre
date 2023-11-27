using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IPaymentToValidateService
    {
        Task<PaymentToValidate> Get(int id);
        Task<IEnumerable<PaymentToValidate>> GetAll();
        Task DeleteById(int id);
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search);
        Task ProcessAllPayments(ClaimsPrincipal user);
        Task<Tuple<bool, string>> ProcessUserPayments(int paymentToValidateId, List<Guid> payments, ClaimsPrincipal user);
        Task<DataTablesStructs.ReturnedData<object>> GetUserPaymentsDatatable(int paymentToValidateId);
        Task<DataTablesStructs.ReturnedData<object>> GetExternalPaymentsDatatable(DataTablesStructs.SentParameters sentParameters, string search);
    }
}
