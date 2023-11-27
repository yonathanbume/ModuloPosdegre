using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.WorkingDay;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Teacher;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IWorkingDayService
    {
        Task InsertRange(ICollection<WorkingDay> workingDays);
        Task UpdateRange(ICollection<WorkingDay> workingDays);
        Task<WorkingDay> GetByFilter(DateTime? date = null, string userId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeachersDailyAssitance(DataTablesStructs.SentParameters sentParameters, string teacherId, DateTime startDate, string search, ClaimsPrincipal user);
        Task<IEnumerable<TeacherPlusAssitance>> GetTeachersDailyAssitance(string teacherId, DateTime startDate, string search, ClaimsPrincipal user);
        Task<IEnumerable<WorkingDay>> GetWorkingDayByDateAndUser(DateTime registerDate, string userId);
        Task AddWorkingDaysRange(IEnumerable<WorkingDay> entities);
        Task UpdateWorkingDays(IEnumerable<WorkingDay> entites);
        Task<WorkingDay> Get(Guid Id);
        Task Update(WorkingDay entity);
        Task<DataTablesStructs.ReturnedData<object>> GetWorkingDaysDatatable(DataTablesStructs.SentParameters sentParameters, DateTime? initDatetime, DateTime? finishDatetime, string academicCoordinatorId = null, string searchValue = null);
        Task<object> GetReportTeachearAssistenace(DateTime initDatetime, DateTime finishDatetime);
        Task<IEnumerable<Select2Structs.Result>> GetWorkingDaySelect2ClientSide(Guid termId, string userId, bool? isAbsent = false);
        Task Insert(WorkingDay ta);
        Task<TeacherPlusMonthlyAssitance> GetMonthlyAssistance(string teacherId, string startDate, string search, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeachersToTakeAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string searchValue = null);
        Task<IEnumerable<WorkingDayConsolited>> GetConsolidatedWorkingDayByMonth(DateTime starDate, DateTime endDate, List<string> usersId = null);
    }
}