using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IExtracurricularActivityService
    {
        Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<ExtracurricularActivity> Get(Guid id);
        Task<IEnumerable<ExtracurricularActivity>> GetAll();
        Task Insert(ExtracurricularActivity extracurricularActivity);
        Task Update(ExtracurricularActivity extracurricularActivity);
        Task DeleteById(Guid id);
        Task<ExtracurricularActivity> GetByCode(string code);
        Task<ExtracurricularActivity> GetByName(string name);
        Task<IEnumerable<Select2Structs.Result>> GetExtracurricularActivitiesSelect2ClientSide();
    }
}
