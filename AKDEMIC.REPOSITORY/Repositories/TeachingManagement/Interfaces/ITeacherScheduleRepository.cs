using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeacherSchedule;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface ITeacherScheduleRepository : IRepository<TeacherSchedule>
    {
        Task<TeacherSchedule> GetConflictedClass(string teacherId, int weekDay, TimeSpan startTime, TimeSpan timeEnd, Guid termId, Guid? id = null);
        Task<IEnumerable<TeacherSchedule>> GetAllByClassSchedule(Guid classScheduleId);
        Task<IEnumerable<TeacherSchedule>> GetAllByClassSchedule2(Guid classScheduleId);
        Task<IEnumerable<TeacherScheduleTemplateA>> GetAllAsTemplateA(Guid termId, string teacherId);
        Task<object> GetAllAsModelB(string teacherId = null);
        Task<object> GetAllAsModelC(Guid? termId = null, string teacherId = null);
        Task<object> GetAllAsModelD(string teacherId = null);
        Task<IEnumerable<TeacherScheduleTemplateA>> GetDaiylyReportAsTemplateA(Guid termId, Guid? careerId, DateTime start, DateTime end, string teacherId, Guid? academicDepartmentId);
        Task SaveChanges();
        Task<List<TeacherScheduleReportTemplate>> GetReportTeacherSchedulesTemplate(Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetReportTeacherSchedulesDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherClassesDetailedWithoutAttendance(DataTablesStructs.SentParameters parameters, Guid termId, DateTime endTime, string teacherId);

        Task<DataTablesStructs.ReturnedData<object>> GetSchedulesDatatable(DataTablesStructs.SentParameters parameters, Guid sectionId, string teacherId);
    }
}