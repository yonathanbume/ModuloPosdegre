using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces
{
    public interface IAeContactMessageRepository : IRepository<AeContactMessage>
    {
        Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<object> GetDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId);
    }
}
