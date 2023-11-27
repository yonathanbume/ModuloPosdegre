using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAll();
        Task<Department> Get(Guid departmentId);
        Task<Tuple<int, List<Tuple<string, int>>>> GetDepartmentsQuantityReportByPaginationParameters(PaginationParameter paginationParameter);
        Task<List<Tuple<string, int>>> GetDepartmentsTypeQuantityReport(string search);
        Task<object> GetDepartmentsByCountryCode(string countryCode, string search);
        Task<object> GetDepartmentSelect2ClientSide();
        Task<object> GetDepartmenstJson(string q, string countryCode);
        Task<Department> GetByName(string cell);
        Task<Guid> GetGuiByName(string name);
        Task<object> GetDepartments();

        Task<DataTablesStructs.ReturnedData<object>> GetAllDepartmentDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? countryId = null);
        Task<bool> AnyByCode(string code, Guid? id = null);
        Task Insert(Department department);
        Task Update(Department department);
        Task Delete(Department department);
    }
}
