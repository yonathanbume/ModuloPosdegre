using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public sealed class ResolutionService : IResolutionService
    {
        private readonly IResolutionRepository _resolutionRepository;
        public ResolutionService(IResolutionRepository resolutionRepository)
        {
            _resolutionRepository = resolutionRepository;
        }

        public async Task Insert(Resolution entity)
            => await _resolutionRepository.Insert(entity);

        public async Task Update(Resolution entity)
            => await _resolutionRepository.Update(entity);

        Task<Resolution> IResolutionService.GetAsync(Guid id)
            => _resolutionRepository.Get(id);

        public async Task AddAsync(Resolution entity)
            => await _resolutionRepository.Add(entity);
    }
}