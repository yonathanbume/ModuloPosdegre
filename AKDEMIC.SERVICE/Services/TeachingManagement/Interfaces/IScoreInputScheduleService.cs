using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.ScoreInputSchedule;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface IScoreInputScheduleService
    {
        Task<ScoreInputSchedule> GetAsync(Guid id);
        Task InsertAsync(ScoreInputSchedule scoreInputSchedule);
        Task UpdateAsync(ScoreInputSchedule scoreInputSchedule);
        Task DeleteAsync(ScoreInputSchedule scoreInputSchedule);
        Task<bool> AnyByCourseComponentId(Guid courseComponentId);
        Task<bool> AnyByTermIdCourseComponentId(Guid termId, Guid? courseComponentId, Guid? id = null);
        Task<object> GetAllAsModelA();
        Task<object> GetAsModelB(Guid? id = null);
        Task<DataTablesStructs.ReturnedData<object>> GetScoreInputScheduleDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<DataTablesStructs.ReturnedData<object>> GetReportScoreInputScheduleDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, Guid? careerId, Guid? curriculumId,int? unit, byte? status, Guid? academicDepartmentId, ClaimsPrincipal user, string searchValue, int? week);
        Task<ScoreInputSchedule> GetByTermAndCourseComponent(Guid termId, Guid courseComponentId);
        Task<ScoreInputScheduleViewModel> GetReportScoreInputSchedule(Guid id, Guid? careerId, Guid? curriculumId,int? unit, byte? status, Guid? academicDepartmentId, ClaimsPrincipal user, int? week);
        Task<DataTablesStructs.ReturnedData<object>> GetReportConsolidatedScoreInputSchedule(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, Guid termId, Guid? careerId, Guid? curriculumId, string searchValue,byte status );
        Task<List<ReportScoreInputScheduleConsolidated>> GetReportConsolidatedScoreInputSchedule(ClaimsPrincipal user, Guid termId, Guid? careerId, Guid? curriculumId, byte status);
    }
}