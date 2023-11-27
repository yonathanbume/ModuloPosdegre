using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class ScaleExtraDemeritFieldService : IScaleExtraDemeritFieldService
    {
        private readonly IScaleExtraDemeritFieldRepository _scaleExtraDemeritFieldRepository;

        public ScaleExtraDemeritFieldService(IScaleExtraDemeritFieldRepository scaleExtraDemeritFieldRepository)
        {
            _scaleExtraDemeritFieldRepository = scaleExtraDemeritFieldRepository;
        }

        public async Task<ScaleExtraDemeritField> Get(Guid id)
        {
            return await _scaleExtraDemeritFieldRepository.Get(id);
        }

        public async Task Insert(ScaleExtraDemeritField entity)
        {
            await _scaleExtraDemeritFieldRepository.Insert(entity);
        }

        public async Task Update(ScaleExtraDemeritField entity)
        {
            await _scaleExtraDemeritFieldRepository.Update(entity);
        }

        public async Task Delete(ScaleExtraDemeritField entity)
        {
            await _scaleExtraDemeritFieldRepository.Delete(entity);
        }

        public async Task<ScaleExtraDemeritField> GetScaleExtraDemeritFieldByResolutionId(Guid resolutionId)
        {
            return await _scaleExtraDemeritFieldRepository.GetScaleExtraDemeritFieldByResolutionId(resolutionId);
        }

        public async Task<IEnumerable<ScaleExtraDemeritField>> GetByUserId(string userId)
        {
            return await _scaleExtraDemeritFieldRepository.GetByUserId(userId);
        }
    }
}
