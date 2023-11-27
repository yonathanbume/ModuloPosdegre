using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassSchedule;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class ClassScheduleService : IClassScheduleService
    {
        private readonly IClassScheduleRepository _classScheduleRepository;

        public ClassScheduleService(IClassScheduleRepository classScheduleRepository)
        {
            _classScheduleRepository = classScheduleRepository;
        }

        public async Task<IEnumerable<ClassSchedule>> GetAll()
            => await _classScheduleRepository.GetAll();
        public async Task<ClassSchedule> Get(Guid id)
            => await _classScheduleRepository.Get(id);

        public async Task<IEnumerable<ClassSchedule>> GetAllBySection(Guid sectionId) =>
            await _classScheduleRepository.GetAllBySection(sectionId);

        public async Task<IEnumerable<ClassSchedule>> GetAllByStudentAndTerm(Guid studentId, Guid termId)
            => await _classScheduleRepository.GetAllByStudentAndTerm(studentId, termId);

        public async Task<IEnumerable<ClassSchedule>> GetAllByTeacherAndTerm(string teacherId, Guid termId)
            => await _classScheduleRepository.GetAllByTeacherAndTerm(teacherId, termId);

        public async Task<ClassSchedule> GetWithTeacherSchedules(Guid id)
            => await _classScheduleRepository.GetWithTeacherSchedules(id);

        public async Task<ClassSchedule> GetFirstForSection(Guid id)
            => await _classScheduleRepository.GetFirstForSection(id);

        public async Task<IEnumerable<ClassScheduleTemplate>> GetClassSchedulesByStudentIdAndTermId(Guid studentId, Guid termId)
            => await _classScheduleRepository.GetClassSchedulesByStudentIdAndTermId(studentId, termId);

        public async Task<object> GetSchedule(Guid studentId, Guid termId)
            => await _classScheduleRepository.GetSchedule(studentId, termId);

        public Task<IEnumerable<ClassScheduleTemplateA>> GetAllAsModelA(Guid termId, string teacherId)
            => _classScheduleRepository.GetAllAsModelA(termId, teacherId);

        public Task<IEnumerable<ClassSchedule>> GetAllByClassroomAndTerm(Guid classroomId, Guid termId)
            => _classScheduleRepository.GetAllByClassroomAndTerm(classroomId, termId);

        public async Task<ClassSchedule> GetClassSchedulesBySectionId(Guid sectionId)
            => await _classScheduleRepository.GetClassSchedulesBySectionId(sectionId);

        public Task InsertAsync(ClassSchedule classSchedule)
            => _classScheduleRepository.Insert(classSchedule);

        public Task AddAsync(ClassSchedule classSchedule)
            => _classScheduleRepository.Add(classSchedule);

        public Task DeleteAsync(ClassSchedule classSchedule)
            => _classScheduleRepository.Delete(classSchedule);

        public Task UpdateAsync(ClassSchedule classSchedule)
            => _classScheduleRepository.Update(classSchedule);

        public Task InsertRangeAsync(IEnumerable<ClassSchedule> classSchedules)
            => _classScheduleRepository.InsertRange(classSchedules);

        public Task DeleteRangeAsync(IEnumerable<ClassSchedule> classSchedules)
            => _classScheduleRepository.DeleteRange(classSchedules);

        public Task UpdateRangeAsync(IEnumerable<ClassSchedule> classSchedules)
            => _classScheduleRepository.UpdateRange(classSchedules);

        public Task<object> GetAsModelB(Guid id)
            => _classScheduleRepository.GetAsModelB(id);

        public Task<object> GetAllAsModelC(Guid sectionId, string teacherId = null)
            => _classScheduleRepository.GetAllAsModelC(sectionId, teacherId);

        public async Task<object> GetSectionClassSchedulesDatatable(DataTablesStructs.SentParameters sentParameters, Guid sectionId,string search = null)
        {
            return await _classScheduleRepository.GetSectionClassSchedulesDatatable(sentParameters,sectionId,search);
        }

        public Task<ClassSchedule> GetWithSectionCourseTermCourse(Guid classRoomId, int weekDay, TimeSpan timeStart, TimeSpan timeEnd, Guid termId, Guid? id = null)
            => _classScheduleRepository.GetWithSectionCourseTermCourse(classRoomId, weekDay, timeStart, timeEnd, termId, id);

        public async Task<object> GetStudentScheduleEnrollment(Guid id, Guid? termId = null)
            => await _classScheduleRepository.GetStudentScheduleEnrollment(id, termId);

        public async Task<List<ClassSchedule>> GetStudentSchedules(Guid studentId, Guid? termId = null)
            => await _classScheduleRepository.GetStudentSchedules(studentId, termId);

        public async Task<object> GetAllWithData(Guid termId, string userId)
            => await _classScheduleRepository.GetAllWithData(termId, userId);

        public async Task<List<ClassSchedule>> GetStudentSchedulesWithData(Guid termId, string userId)
            => await _classScheduleRepository.GetStudentSchedulesWithData(termId, userId);

        public async Task<object> GetAllByGroupId(Guid id)
        {
            return await _classScheduleRepository.GetAllByGroupId(id);
        }

        public async Task CreateSectionsJob(Term term,string userId)
        {
            await _classScheduleRepository.CreateSectionsJob(term, userId);
        }

        public async Task<IEnumerable<ClassSchedule>> GetAllBySections(List<Guid> sectionsId)
            => await _classScheduleRepository.GetAllBySections(sectionsId);

        public async Task<object> GetStudentSectionsSchedule(Guid termId, string userId)
            => await _classScheduleRepository.GetStudentSectionsSchedule(termId, userId);

        public async Task<UnassignedSchedulesReportTemplate> GetUnassignedSchedulesReportTemplate(Guid termId, Guid? careerId, Guid? curriculumId, ClaimsPrincipal user)
            => await _classScheduleRepository.GetUnassignedSchedulesReportTemplate(termId, careerId, curriculumId, user);

        public async Task<ResultTemplate> CreateClassSchedule(ValidateClassScheduleTemplate model)
            => await _classScheduleRepository.CreateClassSchedule(model);

        public async Task<ResultTemplate> EditClassSchedule(ValidateClassScheduleTemplate model)
            => await _classScheduleRepository.EditClassSchedule(model);

        public async Task<ResultTemplate> DeleteClassSchedule(Guid classScheduleId)
            => await _classScheduleRepository.DeleteClassSchedule(classScheduleId);

        public async Task CompleteClassesToActiveTerm()
            => await _classScheduleRepository.CompleteClassesToActiveTerm();

        public async Task<List<ClassScheduleTemplate>> GetClassScheduleTemplateBySectionId(Guid sectionId)
            => await _classScheduleRepository.GetClassScheduleTemplateBySectionId(sectionId);

        public Task<ClassScheduleReportTemplate> GetScheduleReport(Guid studentId, Guid termId)
            => _classScheduleRepository.GetScheduleReport(studentId, termId);
    }
}