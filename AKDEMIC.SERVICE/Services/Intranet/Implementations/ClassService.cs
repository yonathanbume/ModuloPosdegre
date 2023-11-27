using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Class;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassSchedule;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;

        public ClassService(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public Task<int> Count(Guid? studentId = null, Guid? termId = null, string teacherId = null, Guid? classroomId = null, DateTime? start = null, DateTime? end = null, bool? isDictated = null, Guid? sectionId = null)
            => _classRepository.Count(studentId, termId, teacherId, classroomId, start, end, isDictated, sectionId);

        public Task<Class> Get(Guid id)
            => _classRepository.Get(id);

        public Task<IEnumerable<Class>> GetAll(Guid? studentId = null, Guid? termId = null, string teacherId = null, Guid? classroomId = null, DateTime? start = null, DateTime? end = null, bool? isDictated = null)
            => _classRepository.GetAll(studentId, termId, teacherId, classroomId, start, end, isDictated);

        public Task<IEnumerable<Class>> GetAll(Guid? studentId = null, Guid? termId = null, string teacherId = null, Guid? classroomId = null, DateTime? start = null, DateTime? end = null)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetAllAsModelByTeacherIdWeekNumberAndTermId(string teacherId, int weekNumber, Guid termId)
            => _classRepository.GetAllAsModelByTeacherIdWeekNumberAndTermId(teacherId, weekNumber, termId);

        public Task<Class> GetWithTeacherSchedules(Guid id)
            => _classRepository.GetWithTeacherSchedules(id);

        public Task Update(Class @class)
            => _classRepository.Update(@class);

        Task<bool> IClassService.AnyCrossingByClassroomIdAndDateRange(Guid classroomId, DateTime start, DateTime end, Guid? exceptionId)
            => _classRepository.AnyCrossingByClassroomIdAndDateRange(classroomId, start, end, exceptionId);

        Task<IEnumerable<Class>> IClassService.GetAllBySectionId(Guid sectionId)
            => _classRepository.GetAllBySectionId(sectionId);

        Task<object> IClassService.GetAllByTermIdTeacherIdAndDateRange(Guid termId, string userId, DateTime start, DateTime end)
            => _classRepository.GetAllByTermIdTeacherIdAndDateRange(termId, userId, start, end);

        Task<object> IClassService.GetAsModelAByIdAndTeacherId(Guid id, string teacherId)
            => _classRepository.GetAsModelAByIdAndTeacherId(id, teacherId);

        Task<ClassTemplate> IClassService.GetByUserIdAndDateRange(string userId, DateTime start, DateTime end, Guid? exceptionId)
            => _classRepository.GetByUserIdAndDateRange(userId, start, end, exceptionId);

        public async Task<int> GetTotalClassesBySectionId(Guid sectionId)
            => await _classRepository.GetTotalClassesBySectionId(sectionId);

        public async Task<object> GetSchedulesHome(Guid studentId, Guid termId, DateTime start, DateTime end)
            => await _classRepository.GetSchedulesHome(studentId, termId, start, end);

        public Task InsertAync(Class _class)
            => _classRepository.Insert(_class);

        public Task UpdateAync(Class _class)
            => _classRepository.Update(_class);

        public Task DeleteAync(Class _class)
            => _classRepository.Delete(_class);

        public Task InsertRangeAync(IEnumerable<Class> _clases)
            => _classRepository.InsertRange(_clases);
        public async Task<bool> GetExistClassRoom(Guid id, Guid classroomId, DateTime starTime, DateTime endTime)
            => await _classRepository.GetExistClassRoom(id, classroomId, starTime, endTime);
        public async Task<Class> GetConflictedClass(Guid id, string teacherId, DateTime starTime, DateTime endTime)
            => await _classRepository.GetConflictedClass(id, teacherId, starTime, endTime);

        //public async Task<DataTablesStructs.ReturnedData<ReportTeacherClassTemplate>> GetTeacherClassesReport(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid careerId, string teacherId, Guid courseId)
        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherClassesReport(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId, string teacherId, Guid courseId, ClaimsPrincipal user, string startSearchDate, string endSearchDate, Guid? academicDepartment)
        {
            return await _classRepository.GetTeacherClassesReport(sentParameters, termId, careerId, teacherId, courseId, user , startSearchDate, endSearchDate,academicDepartment);
        }

        public async Task CreateClassJob()
        {
            await _classRepository.CreateClassJob();
        }

        public async Task<List<Class>> GetClassesByByClassScheduleAndSectionIdAndClassroomId(Guid classScheduleId, Guid sectionId, Guid classroomId)
            => await _classRepository.GetClassesByByClassScheduleAndSectionIdAndClassroomId(classScheduleId, sectionId, classroomId);

        public async Task DeleteRange(IEnumerable<Class> entites)
            => await _classRepository.DeleteRange(entites);

        public async Task<IEnumerable<Class>> GetAllBySectionId2(Guid sectionId)
        {
            return await _classRepository.GetAllBySectionId2(sectionId);
        }

        public async Task<object> GetOldClassesDatatableClientSide(Guid sectionId, DateTime? day = null, string teacherId = null)
            => await _classRepository.GetOldClassesDatatableClientSide(sectionId, day, teacherId);

        public Task<List<ClassExcelTemplate>> GetTeacherClassesExcelData(Guid termId, Guid? careerId, string teacherId, Guid courseId, ClaimsPrincipal user , string startSearchDate, string endSearchDate, Guid? academicDepartmentId)
            => _classRepository.GetTeacherClassesExcelData(termId,careerId,teacherId,courseId,user , startSearchDate,  endSearchDate, academicDepartmentId);

        public async Task<List<ReportTeacherClassTemplate>> GetTeachersClassReportData(Guid termId, Guid? careerId, string teacherId, Guid courseId, ClaimsPrincipal user, string startSearchDate, string endSearchDate, Guid? academicDepartmentId)
            => await _classRepository.GetTeachersClassReportData(termId, careerId, teacherId, courseId, user, startSearchDate, endSearchDate, academicDepartmentId);

        public async Task<object> GetHistoryClassesDatatableClientSide(Guid sectionId, DateTime? day = null, string teacherId = null)
        {
            return await _classRepository.GetHistoryClassesDatatableClientSide(sectionId, day, teacherId);
        }

        public async Task<List<ClassByPlanExcelTemplate>> GetReportClassAssistance(Guid termId, Guid careerId, Guid curriculumId, DateTime end)
            => await _classRepository.GetReportClassAssistance(termId, careerId, curriculumId, end);

        public async Task<DataTablesStructs.ReturnedData<object>> GetClassesToNeedReschedule(DataTablesStructs.SentParameters parameters, Guid termId, string teacherId)
            => await _classRepository.GetClassesToNeedReschedule(parameters, termId, teacherId);

        public async Task<int> GetMaxAbsencesBySection(Guid sectionId)
            => await _classRepository.GetMaxAbsencesBySection(sectionId);
    }
}