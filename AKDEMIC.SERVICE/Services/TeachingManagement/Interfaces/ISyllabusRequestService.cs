using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.SyllabusRequest;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface ISyllabusRequestService
    {
        Task<object> GetAsModelA(Guid? id = null);
        Task<SyllabusRequest> GetByTerm(Guid termId);
        Task<bool> AnyByTermId(Guid termId);
        Task<object> GetAllAsModelA();
        Task<object> GetAllAsModelB(string coordinatorId = null, string teacherId = null);
        Task<SyllabusRequest> GetAsync(Guid id);
        Task InsertAsync(SyllabusRequest syllabusRequest);
        Task DeleteAsync(SyllabusRequest syllabusRequest);
        Task UpdateAsync(SyllabusRequest syllabusRequest);
        Task<ChartJSTemplate> GetChartJsReport(Guid termId, Guid? facultyId, ClaimsPrincipal user);
        Task<DataTablesStructs.ReturnedData<object>> GetSyllabusRequestDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<DataTablesStructs.ReturnedData<object>> GetSyllabusRequestToTeachersDatatable(DataTablesStructs.SentParameters sentParameters, string teacherId, Guid? termId, string searchValue = null);
        Task<ChartJSTemplate> GetChartJsReportByAcademicDepartment(Guid termId, Guid? academicDepartmentId, ClaimsPrincipal user);
        Task<SyllabusRequest> GetLastSyllabusRequestOpened();
        Task<List<Select2Structs.Result>> GetSyllabusRequestTermSelect2();
    }
}