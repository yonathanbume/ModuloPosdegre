using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScaleExtraBenefitFieldRepository : IRepository<ScaleExtraBenefitField>
    {
        Task<ScaleExtraBenefitField> GetScaleExtraBenefitFieldByResolutionId(Guid resolutionId);
        Task<IEnumerable<ScaleExtraBenefitField>> GetByScaleResolutionTypeAndUser(string userId, string resolutionTypeName);
    }
}
