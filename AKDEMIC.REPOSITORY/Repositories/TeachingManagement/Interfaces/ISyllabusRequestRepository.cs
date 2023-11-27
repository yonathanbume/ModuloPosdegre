using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.SyllabusRequest;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface ISyllabusRequestRepository : IRepository<SyllabusRequest>
    {
        Task<bool> AnyByTermId(Guid termId);
        Task<SyllabusRequest> GetByTerm(Guid termId);
        Task<object> GetAllAsModelA();
        Task<object> GetAsModelA(Guid? id = null);
        Task<object> GetAllAsModelB(string coordinatorId = null, string teacherId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetSyllabusRequestDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<DataTablesStructs.ReturnedData<object>> GetSyllabusRequestToTeachersDatatable(DataTablesStructs.SentParameters sentParameters, string teacherId, Guid? termId, string searchValue = null);
        Task<ChartJSTemplate> GetChartJsReportByAcademicDepartment(Guid termId, Guid? academicDepartmentId, ClaimsPrincipal user);
        Task<ChartJSTemplate> GetChartJsReport(Guid termId, Guid? facultyId, ClaimsPrincipal user);
        Task<SyllabusRequest> GetLastSyllabusRequestOpened();
        Task<List<Select2Structs.Result>> GetSyllabusRequestTermSelect2();
    }
}