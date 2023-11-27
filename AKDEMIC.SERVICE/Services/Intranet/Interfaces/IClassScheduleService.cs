using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassSchedule;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IClassScheduleService
    {
        Task<IEnumerable<ClassSchedule>> GetAll();
        Task<ClassSchedule> Get(Guid id);
        Task<ClassSchedule> GetFirstForSection(Guid id);
        Task<ClassSchedule> GetWithTeacherSchedules(Guid id);
        Task<ClassSchedule> GetWithSectionCourseTermCourse(Guid classRoomId, int weekDay, TimeSpan timeStart, TimeSpan timeEnd, Guid termId, Guid? id = null);
        Task<IEnumerable<ClassSchedule>> GetAllBySection(Guid sectionId);
        Task<IEnumerable<ClassSchedule>> GetAllByStudentAndTerm(Guid studentId, Guid termId);
        Task<IEnumerable<ClassSchedule>> GetAllByTeacherAndTerm(string teacherId, Guid termId);
        Task<IEnumerable<ClassSchedule>> GetAllByClassroomAndTerm(Guid classroomId, Guid termId);
        Task<IEnumerable<ClassScheduleTemplate>> GetClassSchedulesByStudentIdAndTermId(Guid studentId, Guid termId);
        Task<object> GetSchedule(Guid studentId, Guid termId);
        Task<ClassScheduleReportTemplate> GetScheduleReport(Guid studentId, Guid termId);
        Task<IEnumerable<ClassScheduleTemplateA>> GetAllAsModelA(Guid termId, string teacherId);
        Task<object> GetAsModelB(Guid id);
        Task<object> GetAllAsModelC(Guid sectionId, string teacherId = null);
        Task<object> GetSectionClassSchedulesDatatable(DataTablesStructs.SentParameters sentParameters, Guid sectionId,string search = null);
        Task<ClassSchedule> GetClassSchedulesBySectionId(Guid sectionId);
        Task InsertAsync(ClassSchedule classSchedule);
        Task AddAsync(ClassSchedule classSchedule);
        Task InsertRangeAsync(IEnumerable<ClassSchedule> classSchedules);
        Task DeleteAsync(ClassSchedule classSchedule);
        Task DeleteRangeAsync(IEnumerable<ClassSchedule> classSchedules);
        Task UpdateAsync(ClassSchedule classSchedule);
        Task UpdateRangeAsync(IEnumerable<ClassSchedule> classSchedules);
        Task<object> GetStudentScheduleEnrollment(Guid id, Guid? termId = null);
        Task<List<ClassSchedule>> GetStudentSchedules(Guid studentId, Guid? termId = null);
        Task<object> GetAllWithData(Guid termId, string userId);
        Task<List<ClassSchedule>> GetStudentSchedulesWithData(Guid termId, string userId);
        Task<object> GetAllByGroupId(Guid id);
        Task CreateSectionsJob(Term term,string userId);
        Task<IEnumerable<ClassSchedule>> GetAllBySections(List<Guid> sectionsId);
        Task<object> GetStudentSectionsSchedule(Guid termId, string userId);
        Task<UnassignedSchedulesReportTemplate> GetUnassignedSchedulesReportTemplate(Guid termId, Guid? careerId, Guid? curriculumId, ClaimsPrincipal user);

        Task<ResultTemplate> CreateClassSchedule(ValidateClassScheduleTemplate model);
        Task<ResultTemplate> EditClassSchedule(ValidateClassScheduleTemplate model);
        Task<ResultTemplate> DeleteClassSchedule(Guid classScheduleId);
        Task<List<ClassScheduleTemplate>> GetClassScheduleTemplateBySectionId(Guid sectionId);
        Task CompleteClassesToActiveTerm();
    }
}