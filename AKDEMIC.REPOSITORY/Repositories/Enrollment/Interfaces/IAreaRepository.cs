using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IAreaRepository : IRepository<Area>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<IEnumerable<Select2Structs.Result>> GetAreasSelect2ClientSide();
        Task<object> GetAllAsSelect2ClientSide();
        Task<object> GetAreaJson(Guid id);
        Task<object> GetAreaWithDataJson();
        Task<Area> GetFirst();
        Task<Area> GetByName(string name);
    }
}
