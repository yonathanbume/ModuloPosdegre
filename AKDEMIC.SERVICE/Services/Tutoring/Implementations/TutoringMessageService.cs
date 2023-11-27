using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutoringMessageService : ITutoringMessageService
    {
        private readonly ITutoringMessageRepository _tutoringMessageRepository;

        public TutoringMessageService(ITutoringMessageRepository tutoringMessageRepository)
        {
            _tutoringMessageRepository = tutoringMessageRepository;
        }

        public async Task<IEnumerable<TutoringMessage>> GetAllByTutor(string tutorId)
            => await _tutoringMessageRepository.GetAllByTutor(tutorId);
        public async Task DeleteById(Guid id)
            => await _tutoringMessageRepository.DeleteById(id);

        public async Task<TutoringMessage> Get(Guid id)
            => await _tutoringMessageRepository.Get(id);

        public async Task<IEnumerable<TutoringMessage>> GetAll()
            => await _tutoringMessageRepository.GetAll();

        public async Task<IEnumerable<TutoringMessage>> GetAllByTutoringStudentIdAndTutorId(Guid? tutoringStudentId = null, string tutorId = null)
            => await _tutoringMessageRepository.GetAllByTutoringStudentIdAndTutorId(tutoringStudentId, tutorId);
        public async Task DeleteRange(IEnumerable<TutoringMessage> tutoringMessages)
            => await _tutoringMessageRepository.DeleteRange(tutoringMessages);
        public async Task Insert(TutoringMessage tutoringMessage)
            => await _tutoringMessageRepository.Insert(tutoringMessage);

        public async Task Update(TutoringMessage tutoringMessage)
            => await _tutoringMessageRepository.Update(tutoringMessage);
    }
}
