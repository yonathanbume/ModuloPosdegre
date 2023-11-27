using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IPaymentToValidateRepository : IRepository<PaymentToValidate>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search);
        Task ProcessAllPayments(ClaimsPrincipal user);
        Task<Tuple<bool,string>> ProcessUserPayments(int paymentToValidateId, List<Guid> payments, ClaimsPrincipal user);
        Task<DataTablesStructs.ReturnedData<object>> GetUserPaymentsDatatable(int paymentToValidateId);
        Task<DataTablesStructs.ReturnedData<object>> GetExternalPaymentsDatatable(DataTablesStructs.SentParameters sentParameters, string search);
    }
}
