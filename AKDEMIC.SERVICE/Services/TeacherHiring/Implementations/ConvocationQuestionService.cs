using AKDEMIC.ENTITIES.Models.TeacherHiring;
using AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Interfaces;
using AKDEMIC.SERVICE.Services.TeacherHiring.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeacherHiring.Implementations
{
    public class ConvocationQuestionService : IConvocationQuestionService
    {
        private readonly IConvocationQuestionRepository _convocationQuestionRepository;

        public ConvocationQuestionService(IConvocationQuestionRepository convocationQuestionRepository)
        {
            _convocationQuestionRepository = convocationQuestionRepository;
        }

        public async Task<bool> AnyByDescription(Guid sectionId, string description, Guid? ignoredId = null)
            => await _convocationQuestionRepository.AnyByDescription(sectionId, description, ignoredId);

        public async Task Delete(ConvocationQuestion entity)
            => await _convocationQuestionRepository.Delete(entity);

        public async Task DeleteRange(IEnumerable<ConvocationQuestion> questions)
            => await _convocationQuestionRepository.DeleteRange(questions);

        public async Task<ConvocationQuestion> Get(Guid id)
            => await _convocationQuestionRepository.Get(id);

        public async Task<IEnumerable<ConvocationQuestion>> GetAllBySectionId(Guid convocationSectionId)
            => await _convocationQuestionRepository.GetAllBySectionId(convocationSectionId);

        public async Task Insert(ConvocationQuestion entity)
            => await _convocationQuestionRepository.Insert(entity);

        public async Task Update(ConvocationQuestion entity)
            => await _convocationQuestionRepository.Update(entity);
    }
}
