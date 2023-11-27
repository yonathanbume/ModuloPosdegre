using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<Tuple<int, List<Tuple<string, int>>>> GetDepartmentsQuantityReportByPaginationParameters(PaginationParameter paginationParameter);
        Task<List<Tuple<string, int>>> GetDepartmentsTypeQuantityReport(string search);
        Task<object> GetDepartmentsByCountryCode(string countryCode, string search);
        Task<object> GetDepartmenstJson(string q, string countryCode);
        Task<Department> GetByName(string cell);
        Task<Guid> GetGuiByName(string name);
        Task<object> GetDepartments();
        Task<object> GetDepartmentSelect2ClientSide();
        Task<bool> AnyByCode(string code, Guid? id = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDepartmentDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? countryId = null);
    }
}
