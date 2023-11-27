using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IApplicationTermCampusRepository : IRepository<ApplicationTermCampus>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCampusesDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId);
        Task<bool> AnyPostulantByApplicationTermIdAndExamCampusId(Guid applicationTermId, Guid examCampusId);
        Task<object> GetCampusByApplicationTermSelect(Guid applicationTermId, int? type = null);
        Task<Tuple<bool, string>> AddRemoveApplicationTermExamCampusId(Guid applicationTermId, Guid examCampusId, int type);
    }
}
