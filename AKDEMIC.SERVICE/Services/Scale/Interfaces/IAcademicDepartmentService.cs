using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IAcademicDepartmentService
    {
        Task<AcademicDepartment> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetAcademicDeparmentDataTable(DataTablesStructs.SentParameters sentParameters, int? status, Guid? facultyId, string searchValue = null);
        Task Insert(AcademicDepartment entity);
        Task Update(AcademicDepartment entity);
        Task Delete(AcademicDepartment entity);
        Task<IEnumerable<AcademicDepartment>> GetAll(string searchValue = null, bool? onlyActive = null);
        Task<bool> AnyByNameAndFacultyId(string name, Guid facultyId,Guid? ignoredId = null);
        Task<Select2Structs.ResponseParameters> GetAcademicDepartmentSelect2(Select2Structs.RequestParameters parameters, ClaimsPrincipal user, string searchValue);
        Task<object> GetAcademicDepartmentSelect2ClientSide(ClaimsPrincipal user, Guid? facultyId = null);
        Task<object> GetAcademicDepartmentSelect();
        Task<List<AcademicDepartment>> GetDataList();
        Task<IEnumerable<AcademicDepartment>> GetCareerAll();
        Task<object> GetCareersToForum();
    }
}