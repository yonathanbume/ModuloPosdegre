using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeacherSchedule;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public sealed class TeacherScheduleService : ITeacherScheduleService
    {
        private readonly ITeacherScheduleRepository _teacherScheduleRepository;

        public TeacherScheduleService(ITeacherScheduleRepository teacherScheduleRepository)
        {
            _teacherScheduleRepository = teacherScheduleRepository;
        }

        public async Task<IEnumerable<TeacherScheduleTemplateA>> GetDaiylyReportAsTemplateA(Guid termId, Guid? careerId, DateTime start, DateTime end, string teacherId, Guid? academicDepartmentId)
        {
            return await _teacherScheduleRepository.GetDaiylyReportAsTemplateA(termId,careerId, start,end, teacherId,academicDepartmentId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportTeacherSchedulesDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string search)
            => await _teacherScheduleRepository.GetReportTeacherSchedulesDatatable(parameters, termId, search);

        public async Task<List<TeacherScheduleReportTemplate>> GetReportTeacherSchedulesTemplate(Guid termId)
            => await _teacherScheduleRepository.GetReportTeacherSchedulesTemplate(termId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetSchedulesDatatable(DataTablesStructs.SentParameters parameters, Guid sectionId, string teacherId)
            => await _teacherScheduleRepository.GetSchedulesDatatable(parameters, sectionId, teacherId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherClassesDetailedWithoutAttendance(DataTablesStructs.SentParameters parameters, Guid termId, DateTime endTime, string teacherId)
            => await _teacherScheduleRepository.GetTeacherClassesDetailedWithoutAttendance(parameters, termId, endTime, teacherId);
        public async Task SaveChanges()
        {
            await _teacherScheduleRepository.SaveChanges();
        }

        Task ITeacherScheduleService.Delete(TeacherSchedule teacherSchedule)
            => _teacherScheduleRepository.Delete(teacherSchedule);

        Task ITeacherScheduleService.DeleteById(Guid teacherScheduleId)
            => _teacherScheduleRepository.DeleteById(teacherScheduleId);

        Task ITeacherScheduleService.DeleteRange(IEnumerable<TeacherSchedule> teacherSchedules)
            => _teacherScheduleRepository.DeleteRange(teacherSchedules);

        Task<object> ITeacherScheduleService.GetAllAsModelB(string teacherId)
            => _teacherScheduleRepository.GetAllAsModelB(teacherId);

        Task<object> ITeacherScheduleService.GetAllAsModelC(Guid? termId, string teacherId)
            => _teacherScheduleRepository.GetAllAsModelC(termId, teacherId);

        Task<object> ITeacherScheduleService.GetAllAsModelD(string teacherId)
            => _teacherScheduleRepository.GetAllAsModelD(teacherId);

        Task<IEnumerable<TeacherScheduleTemplateA>> ITeacherScheduleService.GetAllAsTemplateA(Guid termId, string teacherId)
            => _teacherScheduleRepository.GetAllAsTemplateA(termId, teacherId);

        Task<IEnumerable<TeacherSchedule>> ITeacherScheduleService.GetAllByClassSchedule(Guid classScheduleId)
            => _teacherScheduleRepository.GetAllByClassSchedule(classScheduleId);

        Task<IEnumerable<TeacherSchedule>> ITeacherScheduleService.GetAllByClassSchedule2(Guid classScheduleId)
            => _teacherScheduleRepository.GetAllByClassSchedule2(classScheduleId);

        Task<TeacherSchedule> ITeacherScheduleService.GetConflictedClass(string teacherId, int weekDay, TimeSpan startTime, TimeSpan timeEnd, Guid termId, Guid? id)
            => _teacherScheduleRepository.GetConflictedClass(teacherId, weekDay, startTime, timeEnd, termId, id);

        Task ITeacherScheduleService.Insert(TeacherSchedule teacherSchedule)
            => _teacherScheduleRepository.Insert(teacherSchedule);

        Task ITeacherScheduleService.InsertRange(IEnumerable<TeacherSchedule> teacherSchedules)
            => _teacherScheduleRepository.InsertRange(teacherSchedules);

        Task ITeacherScheduleService.Update(TeacherSchedule teacherSchedule)
            => _teacherScheduleRepository.Update(teacherSchedule);

        Task ITeacherScheduleService.UpdateRange(IEnumerable<TeacherSchedule> teacherSchedules)
            => _teacherScheduleRepository.UpdateRange(teacherSchedules);
    }
}