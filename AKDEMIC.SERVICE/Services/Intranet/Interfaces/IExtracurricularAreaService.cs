using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IExtracurricularAreaService
    {
        Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task Insert(ExtracurricularArea extracurricularArea);
        Task Update(ExtracurricularArea extracurricularArea);
        Task DeleteById(Guid id);
        Task<ExtracurricularArea> Get(Guid id);
        Task<IEnumerable<ExtracurricularArea>> GetAll();
    }
}
