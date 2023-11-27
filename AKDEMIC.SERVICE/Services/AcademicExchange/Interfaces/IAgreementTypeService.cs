using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces
{
    public interface IAgreementTypeService
    {
        Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<object> GetSelect2ClientSide();
        Task Insert(AgreementType type);
        Task DeleteById(Guid typeId);
        Task Update(AgreementType type);
        Task<AgreementType> Get(Guid typeId);
    }
}
