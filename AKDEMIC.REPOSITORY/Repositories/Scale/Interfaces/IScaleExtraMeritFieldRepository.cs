using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScaleExtraMeritFieldRepository : IRepository<ScaleExtraMeritField>
    {
        Task<ScaleExtraMeritField> GetScaleExtraMeritFieldByResolutionId(Guid resolutionId);
        Task<IEnumerable<ScaleExtraMeritField>> GetByUserId(string userId);
    }
}
