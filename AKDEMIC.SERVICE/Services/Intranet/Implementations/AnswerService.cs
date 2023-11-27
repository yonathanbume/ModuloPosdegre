using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;

        public AnswerService(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public Task<Answer> Add(Answer answer)
            => _answerRepository.Add(answer);

        public async Task AddRange(IEnumerable<Answer> answers)
        {
            await _answerRepository.AddRange(answers);
        }

        public async Task Delete(Answer answer)
        {
            await _answerRepository.Delete(answer);
        }

        public async Task DeleteRange(IEnumerable<Answer> answers)
        {
            await _answerRepository.DeleteRange(answers);
        }

        public async Task<Answer> Get(Guid id)
        {
            return await _answerRepository.Get(id);
        }
        public async Task<IEnumerable<Answer>> GetAnswersByQuestionId(Guid id)
        {
            return await _answerRepository.GetAnswersByQuestionId(id);
        }
    }
}
