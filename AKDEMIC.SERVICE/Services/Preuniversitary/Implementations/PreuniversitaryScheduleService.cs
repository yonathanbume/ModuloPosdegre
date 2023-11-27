using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Implementations
{
    public class PreuniversitaryScheduleService : IPreuniversitaryScheduleService
    {
        private readonly IPreuniversitaryScheduleRepository _preuniversitaryScheduleRepository;

        public PreuniversitaryScheduleService(IPreuniversitaryScheduleRepository preuniversitaryScheduleRepository)
        {
            _preuniversitaryScheduleRepository = preuniversitaryScheduleRepository;
        }

        public async Task<bool> AnyGroupIdByDayOfWeekAndTimeConflict(Guid groupId, byte dayOfWeek, TimeSpan st, TimeSpan et, Guid? ignoredId = null)
            => await _preuniversitaryScheduleRepository.AnyGroupIdByDayOfWeekAndTimeConflict(groupId, dayOfWeek, st, et, ignoredId);

        public async Task Delete(PreuniversitarySchedule entity)
            => await _preuniversitaryScheduleRepository.Delete(entity);

        public async Task<PreuniversitarySchedule> Get(Guid id)
            => await _preuniversitaryScheduleRepository.Get(id);

        public async Task<PreuniversitarySchedule> GetClassroomConflicted(Guid classroomId, byte dayOfWeek, TimeSpan startTime, TimeSpan endTime, Guid? ignoredId = null)
            => await _preuniversitaryScheduleRepository.GetClassroomConflicted(classroomId, dayOfWeek, startTime, endTime, ignoredId);

        public async Task<PreuniversitarySchedule> GetNextCurrentOfWeek(string userId, int currentDayOfWeek)
            => await _preuniversitaryScheduleRepository.GetNextCurrentOfWeek(userId, currentDayOfWeek);

        public async Task<DataTablesStructs.ReturnedData<object>> GetSchedulesDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId, Guid groupId, string searchValue = null)
            => await _preuniversitaryScheduleRepository.GetSchedulesDatatable(sentParameters, courseId, termId, groupId, searchValue);

        public async Task<PreuniversitarySchedule> GetScheduleToTeacher(string userId, int currentDayOfWeek)
            => await _preuniversitaryScheduleRepository.GetScheduleToTeacher(userId, currentDayOfWeek);

        public async Task<PreuniversitarySchedule> GetScheduleToTemaries(string userId, int currentDayOfWeek)
            => await _preuniversitaryScheduleRepository.GetScheduleToTemaries(userId, currentDayOfWeek);

        public async Task<PreuniversitarySchedule> GetTeacherConflicted(string teacherId, byte dayOfWeek, TimeSpan startTime, TimeSpan endTime, Guid? ignoredId = null)
            => await _preuniversitaryScheduleRepository.GetTeacherConflicted(teacherId, dayOfWeek, startTime, endTime,ignoredId);

        public async Task<object> GetWeekDetails(Guid termId, Guid userGroupId, bool? absent = null, int? week = null)
            => await _preuniversitaryScheduleRepository.GetWeekDetails(termId, userGroupId, absent, week);

        public async Task<object> GetWeeks(Guid termId)
            => await _preuniversitaryScheduleRepository.GetWeeks(termId);

        public async Task Insert(PreuniversitarySchedule entity)
            => await _preuniversitaryScheduleRepository.Insert(entity);

        public async Task Update(PreuniversitarySchedule entity)
            => await _preuniversitaryScheduleRepository.Update(entity);
    }
}
