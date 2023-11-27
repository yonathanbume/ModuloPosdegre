using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutoringSessionStudentService : ITutoringSessionStudentService
    {
        private readonly ITutoringSessionStudentRepository _tutoringSessionStudentRepository;

        public TutoringSessionStudentService(ITutoringSessionStudentRepository tutoringSessionStudentRepository)
        {
            _tutoringSessionStudentRepository = tutoringSessionStudentRepository;        
        }

        public async Task<bool> AnyByTutorIdAndTutoringStudentId(Guid tutoringStudentId, string tutorId, bool? absent = null, Guid? termId = null)
            => await _tutoringSessionStudentRepository.AnyByTutorIdAndTutoringStudentId(tutoringStudentId, tutorId, absent, termId);

        public async Task DeleteById(Guid tutoringSessionStudentId)
            => await _tutoringSessionStudentRepository.DeleteById(tutoringSessionStudentId);

        public async Task DeleteRange(IEnumerable<TutoringSessionStudent> tutoringSessionStudents)
            => await _tutoringSessionStudentRepository.DeleteRange(tutoringSessionStudents);

        public async Task<TutoringSessionStudent> Get(Guid tutoringSessionStudentId)
            => await _tutoringSessionStudentRepository.Get(tutoringSessionStudentId);

        public async Task<IEnumerable<TutoringSessionStudent>> GetAllByTutoringSessionId(Guid tutoringSessionId)
            => await _tutoringSessionStudentRepository.GetAllByTutoringSessionId(tutoringSessionId);

        public async Task<IEnumerable<TutoringSessionStudent>> GetAllByTutoringStudentId(Guid tutoringStudentId)
            => await _tutoringSessionStudentRepository.GetAllByTutoringStudentId(tutoringStudentId);

        public async Task<TutoringSessionStudent> GetByTutoringSessionIdAndTutoringStudentId(Guid tutoringSessionId, Guid tutoringStudentId)
            => await _tutoringSessionStudentRepository.GetByTutoringSessionIdAndTutoringStudentId(tutoringSessionId, tutoringStudentId);

        public async Task<DataTablesStructs.ReturnedData<TutoringSessionStudent>> GetTutoringSessionStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? supportOfficeId = null, Guid? careerId = null, Guid? termId = null, string tutorId = null, bool? attended = null)
            => await _tutoringSessionStudentRepository.GetTutoringSessionStudentsDatatable(sentParameters, searchValue, supportOfficeId, careerId, termId, tutorId, attended);
        public async Task<List<TutoringSessionStudent>> GetTutoringSessionStudents(Guid supportOfficeId, string search = null, Guid? careerId = null, Guid? termId = null)
            => await _tutoringSessionStudentRepository.GetTutoringSessionStudents(supportOfficeId, search, careerId, termId);
        public async Task<DataTablesStructs.ReturnedData<TutoringSessionStudent>> GetHistoryTutoringSessionStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? supportOfficeId = null, Guid? careerId = null, Guid? termId = null, string tutorId = null, bool? attended = null)
            => await _tutoringSessionStudentRepository.GetHistoryTutoringSessionStudentsDatatable(sentParameters, searchValue, supportOfficeId, careerId, termId, tutorId, attended);
        public async Task Insert(TutoringSessionStudent tutoringSessionStudent)
            => await _tutoringSessionStudentRepository.Insert(tutoringSessionStudent);

        public async Task InsertRange(IEnumerable<TutoringSessionStudent> tutoringSessionStudents)
            => await _tutoringSessionStudentRepository.InsertRange(tutoringSessionStudents);

        public async Task Update(TutoringSessionStudent tutoringSessionStudent)
            => await _tutoringSessionStudentRepository.Update(tutoringSessionStudent);

        public async Task UpdateRange(IEnumerable<TutoringSessionStudent> tutoringSessionStudents)
            => await _tutoringSessionStudentRepository.UpdateRange(tutoringSessionStudents);
        public async Task<List<TutoringSessionStudent>> GetWithData(Guid tutoringSessionId, Guid tutoringStudentId)
            => await _tutoringSessionStudentRepository.GetWithData(tutoringSessionId, tutoringStudentId);
        public async Task<DataTablesStructs.ReturnedData<object>> GetTutoringSessionStudentsDatatableByStudent(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid? termId = null, string search = null)
            => await _tutoringSessionStudentRepository.GetTutoringSessionStudentsDatatableByStudent(sentParameters, studentId, termId, search);
        public async Task<IEnumerable<TutoringSessionStudent>> GetAllWithInclude()
            => await _tutoringSessionStudentRepository.GetAllWithInclude();

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentSessionsDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid termId)
            => await _tutoringSessionStudentRepository.GetStudentSessionsDatatable(sentParameters, studentId, termId);

        public Task<TutoringSessionStudent> AddAsync(TutoringSessionStudent tutoringSessionStudent)
            => _tutoringSessionStudentRepository.Add(tutoringSessionStudent);
    }
}
