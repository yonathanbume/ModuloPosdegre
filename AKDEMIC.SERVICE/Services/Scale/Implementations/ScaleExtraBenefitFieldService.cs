using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class ScaleExtraBenefitFieldService : IScaleExtraBenefitFieldService
    {
        private readonly IScaleExtraBenefitFieldRepository _scaleExtraBenefitFieldRepository;

        public ScaleExtraBenefitFieldService(IScaleExtraBenefitFieldRepository scaleExtraBenefitFieldRepository)
        {
            _scaleExtraBenefitFieldRepository = scaleExtraBenefitFieldRepository;
        }

        public async Task<ScaleExtraBenefitField> Get(Guid id)
        {
            return await _scaleExtraBenefitFieldRepository.Get(id);
        }

        public async Task Insert(ScaleExtraBenefitField entity)
        {
            await _scaleExtraBenefitFieldRepository.Insert(entity);
        }

        public async Task Update(ScaleExtraBenefitField entity)
        {
            await _scaleExtraBenefitFieldRepository.Update(entity);
        }

        public async Task Delete(ScaleExtraBenefitField entity)
        {
            await _scaleExtraBenefitFieldRepository.Delete(entity);
        }

        public async Task<ScaleExtraBenefitField> GetScaleExtraBenefitFieldByResolutionId(Guid resolutionId)
        {
            return await _scaleExtraBenefitFieldRepository.GetScaleExtraBenefitFieldByResolutionId(resolutionId);
        }

        public async Task<IEnumerable<ScaleExtraBenefitField>> GetByScaleResolutionTypeAndUser(string userId, string resolutionTypeName)
        {
            return await _scaleExtraBenefitFieldRepository.GetByScaleResolutionTypeAndUser(userId, resolutionTypeName);
        }
    }
}
