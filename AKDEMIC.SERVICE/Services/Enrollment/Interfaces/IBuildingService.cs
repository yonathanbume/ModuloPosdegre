using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IBuildingService
    {
        Task<Building> Get(Guid id);
        Task<IEnumerable<Building>> GetAll();
        Task<IEnumerable<Building>> GetAllByCampusId(Guid campusId);
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<DataTablesStructs.ReturnedData<object>> GetDataByCampusDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue, Guid campus);
        Task Delete(Building building);
        Task DeleteById(Guid id);
        Task Insert(Building building);
        Task Update(Building building);
        Task<IEnumerable<Select2Structs.Result>> GetBuildingsSelect2ClientSide(Guid? campusId = null);
        Task<object> GetBuildingsJson(Guid id);
    }
}
