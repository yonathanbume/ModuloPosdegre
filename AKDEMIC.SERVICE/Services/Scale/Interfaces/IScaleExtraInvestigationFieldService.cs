using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleExtraInvestigationFieldService
    {
        Task<ScaleExtraInvestigationField> Get(Guid id);
        Task Insert(ScaleExtraInvestigationField entity);
        Task Update(ScaleExtraInvestigationField entity);
        Task Delete(ScaleExtraInvestigationField entity);
        Task<ScaleExtraInvestigationField> GetScaleExtraInvestigationFieldByResolutionId(Guid resolutionId);
        Task<IEnumerable<ScaleExtraInvestigationField>> GetAllByUserId(string userId);
        Task<IEnumerable<ScaleExtraInvestigationField>> GetByScaleResolutionTypeAndUser(string userId,string resolutionTypeName);
    }
}
