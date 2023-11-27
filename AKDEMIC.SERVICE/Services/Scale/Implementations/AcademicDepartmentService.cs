using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class AcademicDepartmentService : IAcademicDepartmentService
    {
        private readonly IAcademicDepartmentRepository _academicDepartmentRepository;

        public AcademicDepartmentService(IAcademicDepartmentRepository academicDepartmentRepository)
        {
            _academicDepartmentRepository = academicDepartmentRepository;
        }

        public async Task<bool> AnyByNameAndFacultyId(string name, Guid facultyId,Guid? ignoredId = null)
            => await _academicDepartmentRepository.AnyByNameAndFacultyId(name, facultyId,ignoredId);

        public async Task Delete(AcademicDepartment entity)
            => await _academicDepartmentRepository.Delete(entity);

        public async Task<AcademicDepartment> Get(Guid id)
            => await _academicDepartmentRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAcademicDeparmentDataTable(DataTablesStructs.SentParameters sentParameters, int? status, Guid? facultyId, string searchValue = null)
            => await _academicDepartmentRepository.GetAcademicDeparmentDataTable(sentParameters, status, facultyId, searchValue);

        public async Task<Select2Structs.ResponseParameters> GetAcademicDepartmentSelect2(Select2Structs.RequestParameters parameters, ClaimsPrincipal user, string searchValue)
            => await _academicDepartmentRepository.GetAcademicDepartmentSelect2(parameters, user, searchValue);

        public async Task<object> GetAcademicDepartmentSelect2ClientSide(ClaimsPrincipal user, Guid? facultyId = null)
            => await _academicDepartmentRepository.GetAcademicDepartmentSelect2ClientSide(user, facultyId);

        public async Task<IEnumerable<AcademicDepartment>> GetAll(string searchValue = null, bool? onlyActive = null)
            => await _academicDepartmentRepository.GetAll(searchValue, onlyActive);

        public async Task<List<AcademicDepartment>> GetDataList()
            => await _academicDepartmentRepository.GetDataList();

        public async Task Insert(AcademicDepartment entity)
            => await _academicDepartmentRepository.Insert(entity);

        public async Task Update(AcademicDepartment entity)
            => await _academicDepartmentRepository.Update(entity);
        public async Task<IEnumerable<AcademicDepartment>> GetCareerAll()
            => await _academicDepartmentRepository.GetCareerAll();
        public async Task<object> GetCareersToForum()
            => await _academicDepartmentRepository.GetCareersToForum();

        public Task<object> GetAcademicDepartmentSelect()
            => _academicDepartmentRepository.GetAcademicDepartmentSelect();
    }
}