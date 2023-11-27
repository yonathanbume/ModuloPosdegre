using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IExchangeRateService
    {
        Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters);
        Task Insert(ExchangeRate exchangeRate);
        Task Update(ExchangeRate exchangeRate);
        Task<ExchangeRate> Get(Guid id);
        Task DeleteById(Guid id);
    }
}
