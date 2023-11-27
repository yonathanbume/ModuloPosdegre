using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IDistrictRepository : IRepository<District>
    {
        Task<IEnumerable<Select2Structs.Result>> GetDistrictsSelect2ClientSide(Guid provinceId);
        Task<object> GetDistrictsJson(Guid pid, string q);
        Task<District> GetByNameAndProvinceId(Guid provinceId, string cell);
        Task<Guid> GetGuidByName(string name);
        Task<object> GetDistricts();

        Task<DataTablesStructs.ReturnedData<object>> GetAllDistrictDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? countryId = null, Guid? departmentId = null, Guid? provinceId = null);
        Task<bool> AnyByCode(string code, Guid? id = null);
    }
}
