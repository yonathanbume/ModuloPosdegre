using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleExtraBenefitFieldService
    {
        Task<ScaleExtraBenefitField> Get(Guid id);
        Task Insert(ScaleExtraBenefitField entity);
        Task Update(ScaleExtraBenefitField entity);
        Task Delete(ScaleExtraBenefitField entity);
        Task<ScaleExtraBenefitField> GetScaleExtraBenefitFieldByResolutionId(Guid resolutionId);
        Task<IEnumerable<ScaleExtraBenefitField>> GetByScaleResolutionTypeAndUser(string userId , string resolutionTypeName);
    }
}
