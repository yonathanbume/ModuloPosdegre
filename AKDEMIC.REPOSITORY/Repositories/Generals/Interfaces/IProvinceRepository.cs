using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IProvinceRepository : IRepository<Province>
    {
        Task<Tuple<int, List<Tuple<string, string, int>>>> GetProvincesQuantityReportByPaginationParameters(PaginationParameter paginationParameter);
        Task<List<Tuple<string, string, int>>> GetProvincesTypeQuantityReport(string search);
        Task<IEnumerable<Select2Structs.Result>> GetProvincesSelect2ClientSide(Guid deparmentId);
        Task<object> GetProvincesJson(Guid did, string q);
        Task<Province> GetByNameAndDepartmentId(Guid departmentId, string cell);
        Task<Guid> GetGuidByName(string name);
        Task<object> GetProvinces();
        Task<object> GetAllAsSelect2ClientSide(Guid? departmentId = null);

        Task<DataTablesStructs.ReturnedData<object>> GetAllProvinceDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? countryId = null, Guid? departmentId = null);
        Task<bool> AnyByCode(string code, Guid? id = null);
    }
}
