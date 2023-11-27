using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces
{
    public interface IAeContactMessageService
    {
        Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<object> GetDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId);
        Task Insert(AeContactMessage model);
        Task Update(AeContactMessage model);
        Task Get<AeContactMessage>(Guid id);
    }
}
