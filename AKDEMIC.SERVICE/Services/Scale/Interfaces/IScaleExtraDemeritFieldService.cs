using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleExtraDemeritFieldService
    {
        Task<ScaleExtraDemeritField> Get(Guid id);
        Task Insert(ScaleExtraDemeritField entity);
        Task Update(ScaleExtraDemeritField entity);
        Task Delete(ScaleExtraDemeritField entity);
        Task<ScaleExtraDemeritField> GetScaleExtraDemeritFieldByResolutionId(Guid resolutionId);
        Task<IEnumerable<ScaleExtraDemeritField>> GetByUserId(string userId);
    }
}
