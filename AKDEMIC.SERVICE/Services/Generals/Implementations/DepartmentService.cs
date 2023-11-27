using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<Department>> GetAll()
            => await _departmentRepository.GetAll();
        public async Task<Department> Get(Guid departmentId)
        {
            return await _departmentRepository.Get(departmentId);
        }

        public async Task<object> GetDepartmentsByCountryCode(string countryCode, string search)
        {
            return await _departmentRepository.GetDepartmentsByCountryCode(countryCode, search);
        }

        public async Task<Tuple<int, List<Tuple<string, int>>>> GetDepartmentsQuantityReportByPaginationParameters(PaginationParameter paginationParameter)
        {
            return await _departmentRepository.GetDepartmentsQuantityReportByPaginationParameters(paginationParameter);
        }

        public async Task<List<Tuple<string, int>>> GetDepartmentsTypeQuantityReport(string search)
        {
            return await _departmentRepository.GetDepartmentsTypeQuantityReport(search);
        }

        public async Task<object> GetDepartmenstJson(string q, string countryCode)
            => await _departmentRepository.GetDepartmenstJson(q, countryCode);

        public async Task<Department> GetByName(string cell)
            => await _departmentRepository.GetByName(cell);
        public async Task<Guid> GetGuiByName(string name)
            => await _departmentRepository.GetGuiByName(name);
        public async Task<object> GetDepartments()
            => await _departmentRepository.GetDepartments();

        public async Task<object> GetDepartmentSelect2ClientSide()
            => await _departmentRepository.GetDepartmentSelect2ClientSide();

        public Task<bool> AnyByCode(string code, Guid? id = null)
            => _departmentRepository.AnyByCode(code, id);

        public Task Insert(Department department)
            => _departmentRepository.Insert(department);

        public Task Update(Department department)
            => _departmentRepository.Update(department);

        public Task Delete(Department department)
            => _departmentRepository.Delete(department);

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDepartmentDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? countryId = null)
            => _departmentRepository.GetAllDepartmentDatatable(sentParameters, searchValue, countryId);
    }
}
