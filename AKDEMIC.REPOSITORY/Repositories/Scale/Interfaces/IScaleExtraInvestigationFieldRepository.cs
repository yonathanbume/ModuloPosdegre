using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScaleExtraInvestigationFieldRepository : IRepository<ScaleExtraInvestigationField>
    {
        Task<ScaleExtraInvestigationField> GetScaleExtraInvestigationFieldByResolutionId(Guid resolutionId);
        Task<IEnumerable<ScaleExtraInvestigationField>> GetAllByUserId(string userId);
        Task<IEnumerable<ScaleExtraInvestigationField>> GetByScaleResolutionTypeAndUser(string userId, string resolutionTypeName);
    }
}
