using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class ScaleExtraMeritFieldService : IScaleExtraMeritFieldService
    {
        private readonly IScaleExtraMeritFieldRepository _scaleExtraMeritFieldRepository;

        public ScaleExtraMeritFieldService(IScaleExtraMeritFieldRepository scaleExtraMeritFieldRepository)
        {
            _scaleExtraMeritFieldRepository = scaleExtraMeritFieldRepository;
        }

        public async Task<ScaleExtraMeritField> Get(Guid id)
        {
            return await _scaleExtraMeritFieldRepository.Get(id);
        }

        public async Task Insert(ScaleExtraMeritField entity)
        {
            await _scaleExtraMeritFieldRepository.Insert(entity);
        }

        public async Task Update(ScaleExtraMeritField entity)
        {
            await _scaleExtraMeritFieldRepository.Update(entity);
        }

        public async Task Delete(ScaleExtraMeritField entity)
        {
            await _scaleExtraMeritFieldRepository.Delete(entity);
        }

        public async Task<ScaleExtraMeritField> GetScaleExtraMeritFieldByResolutionId(Guid resolutionId)
        {
            return await _scaleExtraMeritFieldRepository.GetScaleExtraMeritFieldByResolutionId(resolutionId);
        }

        public async Task<IEnumerable<ScaleExtraMeritField>> GetByUserId(string userId)
        {
            return await _scaleExtraMeritFieldRepository.GetByUserId(userId);
        }
    }
}
