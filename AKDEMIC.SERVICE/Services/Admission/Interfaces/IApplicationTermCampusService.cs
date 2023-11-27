using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IApplicationTermCampusService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCampusesDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId);
        Task<bool> AnyPostulantByApplicationTermIdAndExamCampusId(Guid applicationTermId, Guid examCampusId);
        Task<object> GetCampusByApplicationTermSelect(Guid applicationTermId, int? type = null);
        Task<Tuple<bool, string>> AddRemoveApplicationTermExamCampusId(Guid applicationTermId, Guid examCampusId, int type);
    }
}
