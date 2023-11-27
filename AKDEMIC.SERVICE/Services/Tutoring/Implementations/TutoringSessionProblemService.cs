using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutoringSessionProblemService : ITutoringSessionProblemService
    {
        private readonly ITutoringSessionProblemRepository _tutoringSessionProblemRepository;

        public TutoringSessionProblemService(ITutoringSessionProblemRepository tutoringSessionProblemRepository)
        {
            _tutoringSessionProblemRepository = tutoringSessionProblemRepository;
        }

        public async Task<bool> AnyByTutoringSessionIdAndProblemId(Guid tutoringSessionId, Guid tutoringProblemId)
            => await _tutoringSessionProblemRepository.AnyByTutoringSessionIdAndProblemId(tutoringSessionId, tutoringProblemId);

        public async Task<int> CountByTutoringProblemId(Guid? tutoringProblemId = null, byte? category = null, Guid? termId = null, Guid? careerId = null, string tutorId = null, bool? isMultiple = null)
            => await _tutoringSessionProblemRepository.CountByTutoringProblemId(tutoringProblemId, category, termId, careerId, tutorId, isMultiple);

        public async Task DeleteById(Guid tutoringSessionProblemId)
            => await _tutoringSessionProblemRepository.DeleteById(tutoringSessionProblemId);
        
        public async Task<IEnumerable<TutoringSessionProblem>> GetAllByTutoringSessionId(Guid tutoringSessionId)
            => await _tutoringSessionProblemRepository.GetAllByTutoringSessionId(tutoringSessionId);

        public async Task<DataTablesStructs.ReturnedData<TutoringSessionProblem>> GetTutoringSessionProblemsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? tutoringSessionId = null)
            => await _tutoringSessionProblemRepository.GetTutoringSessionProblemsDatatable(sentParameters, searchValue, tutoringSessionId);

        public async Task Insert(TutoringSessionProblem tutoringSessionProblem)
            => await _tutoringSessionProblemRepository.Insert(tutoringSessionProblem);

        public async Task Update(TutoringSessionProblem tutoringSessionProblem)
            => await _tutoringSessionProblemRepository.Update(tutoringSessionProblem);

        public async Task<IEnumerable<TutoringSessionProblem>> GetAllWithInclude()
            => await _tutoringSessionProblemRepository.GetAllWithInclude();

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentSessionProblemsDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid termId)
            => await _tutoringSessionProblemRepository.GetStudentSessionProblemsDatatable(sentParameters, studentId, termId);

    }
}
