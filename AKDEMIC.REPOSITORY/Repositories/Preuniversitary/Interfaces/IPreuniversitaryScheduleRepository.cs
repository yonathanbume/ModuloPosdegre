using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryScheduleRepository : IRepository<PreuniversitarySchedule>
    {
        Task<object> GetWeekDetails(Guid termId, Guid userGroupId, bool? absent = null, int? week = null);
        Task<object> GetWeeks(Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetSchedulesDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId, Guid groupId, string searchValue = null);
        Task<bool> AnyGroupIdByDayOfWeekAndTimeConflict(Guid groupId, byte dayOfWeek, TimeSpan st, TimeSpan et, Guid? ignoredId = null);
        Task<PreuniversitarySchedule> GetClassroomConflicted(Guid classroomId, byte dayOfWeek, TimeSpan startTime, TimeSpan endTime, Guid? ignoredId = null);
        Task<PreuniversitarySchedule> GetTeacherConflicted(string teacherId, byte dayOfWeek, TimeSpan startTime, TimeSpan endTime, Guid? ignoredId = null);
        Task<PreuniversitarySchedule> GetScheduleToTeacher(string userId, int currentDayOfWeek);
        Task<PreuniversitarySchedule> GetNextCurrentOfWeek(string userId, int currentDayOfWeek);
        Task<PreuniversitarySchedule> GetScheduleToTemaries(string userId, int currentDayOfWeek);
    }
}
