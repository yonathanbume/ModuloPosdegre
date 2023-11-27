using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ITutoringSessionRepository : IRepository<TutoringSession>
    {
        Task<List<TutoringSession>> GetAllByTutor(string tutorId);
        Task<bool> AnyByTutor(string tutorId);
        Task<DataTablesStructs.ReturnedData<TutoringSession>> GetTutoringSessionsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string tutorId = null, bool? isMultiple = null, bool? isPast = null, bool? isDictated = null, bool? individually = null, int? typeSection = null, Guid? term = null);
        Task<IEnumerable<TutoringSession>> GetAllByDateRangeAndTutorId(DateTime start, DateTime end, string tutorId, bool? isMultiple = null, bool? isDictated = null);
        Task<IEnumerable<TutoringSession>> GetAllByDateRangeAndTutoringStudentId(DateTime start, DateTime end, Guid tutoringStudentId, bool? isMultiple = null, bool? isDictated = null);
        Task<IEnumerable<TutoringSession>> GetAllByType(bool isMultiple);
        Task<bool> AnyCrossingByClassroomIdAndDateRange(Guid classroomId, DateTime start, DateTime end, Guid? exceptionId = null);
        Task<TutoringSession> GetNearestSessionByTutorId(string tutorId, DateTime? time = null, bool? isMultiple = null);
        Task<TutoringSession> GetByTutorIdAndDate(string tutorId, DateTime time, bool? isMultiple = null);
        Task<TutoringSession> GetByTutorIdAndDateRange(string tutorId, DateTime start, DateTime end, Guid? exceptionId = null);
        Task<TutoringSession> Get(Guid id, string tutorId = null, bool? isMultiple = null);
        Task<int> CountByTutorIdAndCareerIdAndTutoringStudentId(string tutorId = null, Guid? termId = null, Guid? careerId = null, Guid? tutoringStudentId = null, bool? isMultiple = null, bool? isDictated = null);
        Task<int> CountReferredByTutorIdAndCareerIdAndTutoringStudentId(string tutorId = null, Guid? termId = null, Guid? careerId = null, Guid? tutoringStudentId = null, Guid? supportOfficeId = null);
        Task<int> CountAttendedByTutorIdAndCareerIdAndTutoringStudentId(string tutorId = null, Guid? termId = null, Guid? careerId = null, Guid? tutoringStudentId = null, Guid? supportOfficeId = null);
        Task<int> CountByDateRangeAndCareerId(DateTime startTime, DateTime endTime, Guid? careerId = null, Guid? termId = null, bool? isMultiple = null, bool? isDictated = null, bool? isAttended = null);
        Task<IEnumerable<TutoringSession>> GetAllWithInclude();
        Task<TutoringSession> GetAllWithData(Guid tutoringSessionId);
        Task<object> GetAllTutorsChart(Guid? termId = null, ClaimsPrincipal user = null);
        Task<int> GetTutoringCount(string teacherId = null);
        Task<object> GetAllTutoringsMadeByTutorByFacultyIdAsData(int tutoringSessionType, Guid? careerId = null, Guid? termId = null, ClaimsPrincipal user = null);
        Task<object> GetTutorsTeacherSelect2(ClaimsPrincipal user = null);
    }
}
