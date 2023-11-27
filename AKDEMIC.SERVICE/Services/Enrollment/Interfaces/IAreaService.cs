using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IAreaService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task Delete(Area area);
        Task DeleteById(Guid id);
        Task Insert(Area area);
        Task Update(Area area);
        Task<Area> Get(Guid id);
        Task<IEnumerable<Select2Structs.Result>> GetAreasSelect2ClientSide();
        Task<object> GetAllAsSelect2ClientSide();
        Task<object> GetAreaJson(Guid id);
        Task<object> GetAreaWithDataJson();
        Task<Area> GetFirst();
        Task<IEnumerable<Area>> GetAll();
        Task<Area> GetByName(string name);
    }
}