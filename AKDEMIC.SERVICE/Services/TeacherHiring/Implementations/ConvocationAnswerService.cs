using AKDEMIC.ENTITIES.Models.TeacherHiring;
using AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Interfaces;
using AKDEMIC.SERVICE.Services.TeacherHiring.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeacherHiring.Implementations
{
    public class ConvocationAnswerService : IConvocationAnswerService
    {
        private readonly IConvocationAnswerRepository _convocationAnswerRepository;

        public ConvocationAnswerService(IConvocationAnswerRepository convocationAnswerRepository)
        {
            _convocationAnswerRepository = convocationAnswerRepository;
        }

        public async Task DeleteRange(IEnumerable<ConvocationAnswer> entities)
            => await _convocationAnswerRepository.DeleteRange(entities);

        public async Task<IEnumerable<ConvocationAnswer>> GetAllByQuestionId(Guid questionId)
            => await _convocationAnswerRepository.GetAllByQuestionId(questionId);

        public async Task<IEnumerable<ConvocationAnswer>> GetAllBySectionId(Guid sectionId)
            => await _convocationAnswerRepository.GetAllBySectionId(sectionId);
    }
}
