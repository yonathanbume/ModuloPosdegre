using AKDEMIC.ENTITIES.Models.TeacherHiring;
using AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Interfaces;
using AKDEMIC.SERVICE.Services.TeacherHiring.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeacherHiring.Implementations
{
    public class ConvocationSectionService : IConvocationSectionService
    {
        private readonly IConvocationSectionRepository _convocationSectionRepository;

        public ConvocationSectionService(IConvocationSectionRepository convocationSectionRepository)
        {
            _convocationSectionRepository = convocationSectionRepository;
        }

        public async Task<bool> AnyByTitle(Guid convocationId, string title, Guid? ignoredId = null)
            => await _convocationSectionRepository.AnyByTitle(convocationId, title, ignoredId);

        public async Task Delete(ConvocationSection entity)
            => await _convocationSectionRepository.Delete(entity);

        public async Task<ConvocationSection> Get(Guid id)
            => await _convocationSectionRepository.Get(id);

        public async Task<IEnumerable<ConvocationSection>> GetSectionsByConvocationId(Guid convocationId)
            => await _convocationSectionRepository.GetSectionsByConvocationId(convocationId);

        public async Task Insert(ConvocationSection entity)
            => await _convocationSectionRepository.Insert(entity);
    }
}
