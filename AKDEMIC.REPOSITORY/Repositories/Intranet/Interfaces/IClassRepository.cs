using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Class;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassSchedule;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IClassRepository : IRepository<Class>
    {
        Task<Class> GetWithTeacherSchedules(Guid id);
        Task<IEnumerable<Class>> GetAllBySectionId(Guid sectionId);
        Task<bool> AnyCrossingByClassroomIdAndDateRange(Guid classroomId, DateTime start, DateTime end, Guid? exceptionId = null);
        Task<ClassTemplate> GetByUserIdAndDateRange(string userId, DateTime start, DateTime end, Guid? exceptionId = null);
        Task<object> GetAllByTermIdTeacherIdAndDateRange(Guid termId, string userId, DateTime start, DateTime end);
        Task<object> GetAsModelAByIdAndTeacherId(Guid id, string teacherId);
        Task<IEnumerable<Class>> GetAll(Guid? studentId = null, Guid? termId = null, string teacherId = null, Guid? classroomId = null, DateTime? start = null, DateTime? end = null, bool? isDictated = null);
        Task<int> Count(Guid? studentId = null, Guid? termId = null, string teacherId = null, Guid? classroomId = null, DateTime? start = null, DateTime? end = null, bool? isDictated = null, Guid? sectionId = null);
        Task<object> GetAllAsModelByTeacherIdWeekNumberAndTermId(string teacherId, int weekNumber, Guid termId);
        Task<int> GetTotalClassesBySectionId(Guid sectionId);
        Task<object> GetSchedulesHome(Guid studentId, Guid termId, DateTime start, DateTime end);
        Task<bool> GetExistClassRoom(Guid id, Guid classroomId, DateTime starTime, DateTime endTime);
        Task<Class> GetConflictedClass(Guid id, string teacherId, DateTime starTime, DateTime endTime);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherClassesReport(SentParameters sentParameters, Guid termId, Guid? careerId, string teacherId, Guid courseId, ClaimsPrincipal user , string startSearchDate, string endSearchDate, Guid? academicDepartmentId);
        //Task<DataTablesStructs.ReturnedData<ReportTeacherClassTemplate>> GetTeacherClassesReport(SentParameters sentParameters, Guid termId, Guid careerId, string teacherId, Guid courseId);
        Task<List<ClassExcelTemplate>> GetTeacherClassesExcelData(Guid termId, Guid? careerId, string teacherId, Guid courseId, ClaimsPrincipal user , string startSearchDate, string endSearchDate, Guid? academicDepartmentId);
        Task<List<ReportTeacherClassTemplate>> GetTeachersClassReportData(Guid termId, Guid? careerId, string teacherId, Guid courseId, ClaimsPrincipal user, string startSearchDate, string endSearchDate, Guid? academicDepartmentId);
        Task CreateClassJob();
        Task<IEnumerable<Class>> GetAllBySectionId2(Guid sectionId);
        Task<List<Class>> GetClassesByByClassScheduleAndSectionIdAndClassroomId(Guid classScheduleId, Guid sectionId, Guid classroomId);
        Task<object> GetOldClassesDatatableClientSide(Guid sectionId, DateTime? day = null, string teacherId = null);
        Task<object> GetHistoryClassesDatatableClientSide(Guid sectionId, DateTime? day = null, string teacherId = null);
        Task<List<ClassByPlanExcelTemplate>> GetReportClassAssistance(Guid termId, Guid careerId, Guid curriculumId, DateTime end);
        Task<DataTablesStructs.ReturnedData<object>> GetClassesToNeedReschedule(DataTablesStructs.SentParameters parameters, Guid termId, string teacherId);
        Task<int> GetMaxAbsencesBySection(Guid sectionId);
    }
}