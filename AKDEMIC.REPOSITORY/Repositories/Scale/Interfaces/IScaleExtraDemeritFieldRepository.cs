using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScaleExtraDemeritFieldRepository : IRepository<ScaleExtraDemeritField>
    {
        Task<ScaleExtraDemeritField> GetScaleExtraDemeritFieldByResolutionId(Guid resolutionId);
        Task<IEnumerable<ScaleExtraDemeritField>> GetByUserId(string userId);
    }
}
