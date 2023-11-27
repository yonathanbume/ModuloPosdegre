using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutoringAttendanceProblemService : ITutoringAttendanceProblemService
    {
        private readonly ITutoringAttendanceProblemRepository _tutoringAttendanceProblemRepository;

        public TutoringAttendanceProblemService(ITutoringAttendanceProblemRepository tutoringAttendanceProblemRepository)
        {
            _tutoringAttendanceProblemRepository = tutoringAttendanceProblemRepository;
        }

        public async Task<bool> AnyByTutoringAttendanceIdAndProblemId(Guid tutoringAttendanceId, Guid tutoringProblemId)
            => await _tutoringAttendanceProblemRepository.AnyByTutoringAttendanceIdAndProblemId(tutoringAttendanceId, tutoringProblemId);

        public async Task<int> CountByTutoringProblemId(Guid? tutoringProblemId = null, byte? category = null, Guid? termId = null, Guid? careerId = null, string tutorId = null)
            => await _tutoringAttendanceProblemRepository.CountByTutoringProblemId(tutoringProblemId, category, termId, careerId, tutorId);

        public async Task DeleteById(Guid tutoringAttendanceProblemId)
            => await _tutoringAttendanceProblemRepository.DeleteById(tutoringAttendanceProblemId);

        public async Task<IEnumerable<TutoringAttendanceProblem>> GetAllByTutoringAttendanceId(Guid tutoringAttendanceId)
            => await _tutoringAttendanceProblemRepository.GetAllByTutoringAttendanceId(tutoringAttendanceId);

        public async Task<DataTablesStructs.ReturnedData<TutoringAttendanceProblem>> GetTutoringAttendanceProblemsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? tutoringAttendanceId = null)
            => await _tutoringAttendanceProblemRepository.GetTutoringAttendanceProblemsDatatable(sentParameters, searchValue, tutoringAttendanceId);

        public async Task Insert(TutoringAttendanceProblem tutoringAttendanceProblem)
            => await _tutoringAttendanceProblemRepository.Insert(tutoringAttendanceProblem);

        public async Task Update(TutoringAttendanceProblem tutoringAttendanceProblem)
            => await _tutoringAttendanceProblemRepository.Update(tutoringAttendanceProblem);
    }
}
