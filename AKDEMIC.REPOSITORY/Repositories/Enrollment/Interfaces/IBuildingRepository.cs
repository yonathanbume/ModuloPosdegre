
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IBuildingRepository : IRepository<Building>
    {
        Task<IEnumerable<Building>> GetAllByCampusId(Guid campusId);

        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue, Guid? campus);
        Task<IEnumerable<Select2Structs.Result>> GetBuildingsSelect2ClientSide(Guid? campusId = null);
        Task<object> GetBuildingsJson(Guid id);
        Task<IEnumerable<Building>> GetAllWithData();
    }
}
