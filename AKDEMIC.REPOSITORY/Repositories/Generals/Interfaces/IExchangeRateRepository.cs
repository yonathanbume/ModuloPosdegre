using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IExchangeRateRepository : IRepository<ExchangeRate>
    {
        Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters);
    }
}
