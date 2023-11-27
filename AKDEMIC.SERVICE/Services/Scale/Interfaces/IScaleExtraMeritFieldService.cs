using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleExtraMeritFieldService
    {
        Task<ScaleExtraMeritField> Get(Guid id);
        Task Insert(ScaleExtraMeritField entity);
        Task Update(ScaleExtraMeritField entity);
        Task Delete(ScaleExtraMeritField entity);
        Task<ScaleExtraMeritField> GetScaleExtraMeritFieldByResolutionId(Guid resolutionId);
        Task<IEnumerable<ScaleExtraMeritField>> GetByUserId(string userId);
    }
}
