using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutoringProblemFileService : ITutoringProblemFileService
    {
        private readonly ITutoringProblemFileRepository _tutoringProblemFileRepository;

        public TutoringProblemFileService(ITutoringProblemFileRepository tutoringProblemFileRepository)
        {
            _tutoringProblemFileRepository = tutoringProblemFileRepository;
        }

        public async Task Delete(TutoringProblemFile tutoringProblemFile)
            => await _tutoringProblemFileRepository.Delete(tutoringProblemFile);

        public async Task DeleteById(Guid tutoringProblemFileId)
            => await _tutoringProblemFileRepository.DeleteById(tutoringProblemFileId);

        public async Task<TutoringProblemFile> Get(Guid tutoringProblemFileId)
            => await _tutoringProblemFileRepository.Get(tutoringProblemFileId);

        public async Task<IEnumerable<TutoringProblemFile>> GetAllByTutoringProblemId(Guid tutoringProblemId)
            => await _tutoringProblemFileRepository.GetAllByTutoringProblemId(tutoringProblemId);

        public async Task Insert(TutoringProblemFile tutoringProblemFile)
            => await _tutoringProblemFileRepository.Insert(tutoringProblemFile);

        public async Task Update(TutoringProblemFile tutoringProblemFile)
            => await _tutoringProblemFileRepository.Update(tutoringProblemFile);
    }
}
