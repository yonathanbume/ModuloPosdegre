using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutoringAttendanceService : ITutoringAttendanceService
    {
        private readonly ITutoringAttendanceRepository _tutoringAttendanceRepository;

        public TutoringAttendanceService(ITutoringAttendanceRepository tutoringAttendanceRepository)
        {
            _tutoringAttendanceRepository = tutoringAttendanceRepository;
        }

        public async Task DeleteById(Guid id)
            => await _tutoringAttendanceRepository.DeleteById(id);

        public async Task<TutoringAttendance> Get(Guid id)
            => await _tutoringAttendanceRepository.Get(id);
        public async Task<TutoringAttendance> GetWithData(Guid id)
            => await _tutoringAttendanceRepository.GetWithData(id);
        public async Task<IEnumerable<TutoringAttendance>> GetAll()
            => await _tutoringAttendanceRepository.GetAll();
        
        public async Task<IEnumerable<TutoringAttendance>> GetAllByTutorId(string tutorId, Guid? tutoringStudentId = null)
            => await _tutoringAttendanceRepository.GetAllByTutorId(tutorId, tutoringStudentId);

        public async Task<DataTablesStructs.ReturnedData<TutoringAttendance>> GetTutoringAttendancesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? supportOfficeId = null, string tutorId = null, Guid? tutoringStudentId = null, Guid? termId = null, Guid? careerId = null)
            => await _tutoringAttendanceRepository.GetTutoringAttendancesDatatable(sentParameters, searchValue, supportOfficeId, tutorId, tutoringStudentId, termId, careerId);
        public async Task<List<TutoringAttendance>> GetTutoringAttendances(Guid supportOfficeId, Guid? termId = null, Guid? careerId = null, string searchValue = null)
            => await _tutoringAttendanceRepository.GetTutoringAttendances(supportOfficeId, termId, careerId, searchValue);
        public async Task Insert(TutoringAttendance tutoringAttendance)
            => await _tutoringAttendanceRepository.Insert(tutoringAttendance);

        public async Task Update(TutoringAttendance tutoringAttendance)
            => await _tutoringAttendanceRepository.Update(tutoringAttendance);
        public async Task<TutoringAttendance> GetAllByStudenIdAndSupportOfficeId(Guid tutoringStudentId, Guid supportOfficeId)
            => await _tutoringAttendanceRepository.GetAllByStudenIdAndSupportOfficeId(tutoringStudentId, supportOfficeId);

        public Task<object> GetInformation(Guid id)
            => _tutoringAttendanceRepository.GetInformation(id);
    }
}
