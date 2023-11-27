using AKDEMIC.ENTITIES.Models.TeacherHiring;
using AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Interfaces;
using AKDEMIC.SERVICE.Services.TeacherHiring.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeacherHiring.Implementations
{
    public class ConvocationComiteeService : IConvocationComiteeService
    {
        private readonly IConvocationComiteeRepository _convocationComiteeRepository;

        public ConvocationComiteeService(IConvocationComiteeRepository convocationComiteeRepository)
        {
            _convocationComiteeRepository = convocationComiteeRepository;
        }

        public async Task Delete(ConvocationComitee entity)
            => await _convocationComiteeRepository.Delete(entity);

        public async Task<ConvocationComitee> Get(Guid convocationId, string userId)
            => await _convocationComiteeRepository.Get(convocationId, userId);

        public async Task<List<ConvocationComitee>> GetComitee(Guid convocationId)
            => await _convocationComiteeRepository.GetComitee(convocationId);

        public async Task Insert(ConvocationComitee entity)
            => await _convocationComiteeRepository.Insert(entity);
    }
}
