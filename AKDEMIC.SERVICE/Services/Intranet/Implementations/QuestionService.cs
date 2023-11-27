using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class QuestionService: IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Question> Get(Guid id)
        {
            return await _questionRepository.Get(id);
        }

        public async Task Delete(Question question)
        {
            await _questionRepository.Delete(question);
        }

        public async Task DeleteById(Guid id)
        {
            await _questionRepository.DeleteById(id);
        }

        public async Task Insert(Question question)
        {
            await _questionRepository.Insert(question);
        }

        public async Task Update(Question question)
        {
            await _questionRepository.Update(question);
        }

        public async Task<Question> GetIncludeAnswers(Guid id)
        {
            return await _questionRepository.GetIncludeAnswers(id);
        }

        public Task<Question> Add(Question question)
            => _questionRepository.Add(question);
    }
}
