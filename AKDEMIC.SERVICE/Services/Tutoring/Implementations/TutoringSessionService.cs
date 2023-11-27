using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutoringSessionService : ITutoringSessionService
    {
        private readonly ITutoringSessionRepository _tutoringSessionRepository;

        public TutoringSessionService(ITutoringSessionRepository tutoringSessionRepository)
        {
            _tutoringSessionRepository = tutoringSessionRepository;
        }

        public async Task<bool> AnyCrossingByClassroomIdAndDateRange(Guid classroomId, DateTime start, DateTime end, Guid? exceptionId = null)
            => await _tutoringSessionRepository.AnyCrossingByClassroomIdAndDateRange(classroomId, start, end, exceptionId);

        public async Task<int> CountAttendedByTutorIdAndCareerIdAndTutoringStudentId(string tutorId = null, Guid? termId = null, Guid? careerId = null, Guid? tutoringStudentId = null, Guid? supportOfficeId = null)
            => await _tutoringSessionRepository.CountAttendedByTutorIdAndCareerIdAndTutoringStudentId(tutorId, termId, careerId, tutoringStudentId, supportOfficeId);

        public async Task<int> CountByDateRangeAndCareerId(DateTime startTime, DateTime endTime, Guid? careerId = null, Guid? termId = null, bool? isMultiple = null, bool? isDictated = null, bool? isAttended = null)
            => await _tutoringSessionRepository.CountByDateRangeAndCareerId(startTime, endTime, careerId, termId, isMultiple, isDictated, isAttended);

        public async Task<int> CountByTutorIdAndCareerIdAndTutoringStudentId(string tutorId = null, Guid? termId = null, Guid? careerId = null, Guid? tutoringStudentId = null, bool? isMultiple = null, bool? isDictated = null)
            => await _tutoringSessionRepository.CountByTutorIdAndCareerIdAndTutoringStudentId(tutorId, termId, careerId, tutoringStudentId, isMultiple, isDictated);

        public async Task<int> CountReferredByTutorIdAndCareerIdAndTutoringStudentId(string tutorId = null, Guid? termId = null, Guid? careerId = null, Guid? tutoringStudentId = null, Guid? supportOfficeId = null)
            => await _tutoringSessionRepository.CountReferredByTutorIdAndCareerIdAndTutoringStudentId(tutorId, termId, careerId, tutoringStudentId, supportOfficeId);

        public async Task Delete(TutoringSession tutoringSession)
            => await _tutoringSessionRepository.Delete(tutoringSession);

        public async Task DeleteById(Guid tutoringSessionId)
            => await _tutoringSessionRepository.DeleteById(tutoringSessionId);

        public async Task<TutoringSession> Get(Guid tutoringSessionId)
            => await _tutoringSessionRepository.Get(tutoringSessionId);

        public async Task<TutoringSession> Get(Guid id, string tutorId = null, bool? isMultiple = null)
            => await _tutoringSessionRepository.Get(id, tutorId, isMultiple);

        public async Task<IEnumerable<TutoringSession>> GetAll()
            => await _tutoringSessionRepository.GetAll();

        public async Task<IEnumerable<TutoringSession>> GetAllWithInclude()
            => await _tutoringSessionRepository.GetAllWithInclude();

        public Task<List<TutoringSession>> GetAllByTutor(string tutorId)
            => _tutoringSessionRepository.GetAllByTutor(tutorId);

        public Task<bool> AnyByTutor(string tutorId)
            => _tutoringSessionRepository.AnyByTutor(tutorId);

        public async Task<IEnumerable<TutoringSession>> GetAllByDateRangeAndTutorId(DateTime start, DateTime end, string tutorId, bool? isMultiple = null, bool? isDictated = null)
            => await _tutoringSessionRepository.GetAllByDateRangeAndTutorId(start, end, tutorId);

        public async Task<IEnumerable<TutoringSession>> GetAllByDateRangeAndTutoringStudentId(DateTime start, DateTime end, Guid tutoringStudentId, bool? isMultiple = null, bool? isDictated = null)
            => await _tutoringSessionRepository.GetAllByDateRangeAndTutoringStudentId(start, end, tutoringStudentId);

        public async Task<IEnumerable<TutoringSession>> GetAllByType(bool isMultiple)
            => await _tutoringSessionRepository.GetAllByType(isMultiple);

        public async Task<TutoringSession> GetByTutorIdAndDate(string tutorId, DateTime time, bool? isMultiple = null)
            => await _tutoringSessionRepository.GetByTutorIdAndDate(tutorId, time, isMultiple);

        public async Task<TutoringSession> GetByTutorIdAndDateRange(string tutorId, DateTime start, DateTime end, Guid? exceptionId = null)
            => await _tutoringSessionRepository.GetByTutorIdAndDateRange(tutorId, start, end, exceptionId);

        public async Task<TutoringSession> GetNearestSessionByTutorId(string tutorId, DateTime? time = null, bool? isMultiple = null)
            => await _tutoringSessionRepository.GetNearestSessionByTutorId(tutorId, time, isMultiple);

        public async Task<DataTablesStructs.ReturnedData<TutoringSession>> GetTutoringSessionsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string tutorId = null, bool? isMultiple = null, bool? isPast = null, bool? isDictated = null, bool? individually = null, int? typeSection = null, Guid? term = null)
            => await _tutoringSessionRepository.GetTutoringSessionsDatatable(sentParameters, searchValue, tutorId, isMultiple, isPast, isDictated, individually, typeSection, term);

        public async Task Insert(TutoringSession tutoringSession)
            => await _tutoringSessionRepository.Insert(tutoringSession);

        public async Task Update(TutoringSession tutoringSession)
            => await _tutoringSessionRepository.Update(tutoringSession);

        public async Task<TutoringSession> GetAllWithData(Guid tutoringSessionId)
            => await _tutoringSessionRepository.GetAllWithData(tutoringSessionId);

        public async Task<object> GetAllTutorsChart(Guid? termId = null, ClaimsPrincipal user = null)
            => await _tutoringSessionRepository.GetAllTutorsChart(termId, user);

        public async Task<object> GetAllTutoringsMadeByTutorByFacultyIdAsData(int tutoringSessionType, Guid? careerId = null, Guid? termId = null, ClaimsPrincipal user = null)
            => await _tutoringSessionRepository.GetAllTutoringsMadeByTutorByFacultyIdAsData(tutoringSessionType, careerId, termId, user);

        public async Task<int> GetTutoringCount(string teacherId = null)
            => await _tutoringSessionRepository.GetTutoringCount(teacherId);

        public async Task<object> GetTutorsTeacherSelect2(ClaimsPrincipal user = null)
            => await _tutoringSessionRepository.GetTutorsTeacherSelect2(user);
    }
}
