using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Faculty;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IFacultyService
    {
        Task<Faculty> Get(Guid id);
        Task<Faculty> GetWithHistory(Guid id);
        Task<IEnumerable<Faculty>> GetAll();
        Task<DataTablesStructs.ReturnedData<Faculty>> GetFacultiesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetFacultiesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task Delete(Faculty faculty);
        Task Insert(Faculty faculty);
        Task Update(Faculty faculty);
        Task<object> GetFacultiesSelect2ClientSide(bool hasAll = false, ClaimsPrincipal user = null);
        Task<Select2Structs.ResponseParameters> GetFacultiesByAcademicCoordinatorSelect2(Select2Structs.RequestParameters requestParameters, string academicCoordinatorId, string searchValue = null);
        Task<Faculty> GetFacultyByCareerId(Guid careerId);
        Task<object> GetAllAsSelect2ClientSide(bool hasAll = false, ClaimsPrincipal user = null);
        Task<object> GetAllAsSelect2ClientSide2(bool includeTitle = false);
        Task<object> GetFaculties3();
        Task<object> GetFaculty(Guid id);
        Task<ModelFacultyTemplate> GetFacultiesAdmitted();
        Task<object> GetFacultiesJson(string q, ClaimsPrincipal user = null);
        Task<object> GetFacultiesAllJson();
        Task<List<Faculty>> GetAllWithCareers();
        Task InsertRange(Faculty[] listFaculty);
        Task<string> GetDeanById(Guid facultyId);
        Task<object> GetFacultyIncomesDatatable(DataTablesStructs.SentParameters sentParameters, int? year = null, int? type = null);
    }
}