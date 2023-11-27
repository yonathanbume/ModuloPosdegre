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
    public class TutorTutoringStudentService : ITutorTutoringStudentService
    {
        private readonly ITutorTutoringStudentRepository _tutorTutoringStudentRepository;

        public TutorTutoringStudentService(ITutorTutoringStudentRepository tutorTutoringStudentRepository)
        {
            _tutorTutoringStudentRepository = tutorTutoringStudentRepository;
        }

        public async Task<bool> AnyByTutorIdAndTutoringStudentId(string tutorId, Guid tutoringStudentId, Guid term)
            => await _tutorTutoringStudentRepository.AnyByTutorIdAndTutoringStudentId(tutorId, tutoringStudentId, term);

        public async Task DeleteByTutorIdAndTutoringStudentId(string tutorId, Guid tutoringStudentId, Guid term)
            => await _tutorTutoringStudentRepository.DeleteByTutorIdAndTutoringStudentId(tutorId, tutoringStudentId, term);

        public async Task DeleteRange(IEnumerable<TutorTutoringStudent> tutorTutoringStudents)
            => await _tutorTutoringStudentRepository.DeleteRange(tutorTutoringStudents);

        public async Task<IEnumerable<TutorTutoringStudent>> GetByTutorId(string tutorId)
            => await _tutorTutoringStudentRepository.GetByTutorId(tutorId);

        public async Task<IEnumerable<TutorTutoringStudent>> GetByTutoringStudentId(Guid tutoringStudentId)
            => await _tutorTutoringStudentRepository.GetByTutoringStudentId(tutoringStudentId);

        public async Task Insert(TutorTutoringStudent tutorTutoringStudent)
            => await _tutorTutoringStudentRepository.Insert(tutorTutoringStudent);

        public async Task InsertRange(IEnumerable<TutorTutoringStudent> tutorTutoringStudents)
            => await _tutorTutoringStudentRepository.InsertRange(tutorTutoringStudents);

        public async Task<bool> AnyByStudentId(Guid studentId)
            => await _tutorTutoringStudentRepository.AnyByStudentId(studentId);
        public async Task<TutorTutoringStudent> GetByStudentId(Guid studentId)
            => await _tutorTutoringStudentRepository.GetByStudentId(studentId);
        public async Task<bool> AnyByCoordinator(string userId)
            => await _tutorTutoringStudentRepository.AnyByCoordinator(userId);
        public async Task<bool> AnyByTutor(string tutorId)
            => await _tutorTutoringStudentRepository.AnyByTutor(tutorId);

        public async Task<object> GetAllTutoringsChart(Guid? termId = null, ClaimsPrincipal user = null)
            => await _tutorTutoringStudentRepository.GetAllTutoringsChart(termId, user);

        public Task<DataTablesStructs.ReturnedData<object>> GetTutorByTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, ClaimsPrincipal user = null)
            => _tutorTutoringStudentRepository.GetTutorByTermDatatable(sentParameters, termId, user);

        public Task<object> GetTutorByTermChart(Guid? termId = null, ClaimsPrincipal user = null)
            => _tutorTutoringStudentRepository.GetTutorByTermChart(termId, user);

        public Task<object> GetTutorsByTutoringStudent(Guid studentId, Guid termId)
            => _tutorTutoringStudentRepository.GetTutorsByTutoringStudent(studentId, termId);
    }
}
