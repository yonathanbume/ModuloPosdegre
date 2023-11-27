using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IExtracurricularActivityRepository : IRepository<ExtracurricularActivity>
    {
        Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<ExtracurricularActivity> GetByCode(string code);
        Task<ExtracurricularActivity> GetByName(string name);
        Task<IEnumerable<Select2Structs.Result>> GetExtracurricularActivitiesSelect2ClientSide();
    }
}
