using System;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleSectionResolutionTypeService
    {
        Task<ScaleSectionResolutionType> Get(Guid id);
        Task<bool> IsActive(Guid sectionId, Guid resolutionTypeId);
        Task Insert(ScaleSectionResolutionType scaleSectionResolutionType);
        Task Update(ScaleSectionResolutionType scaleSectionResolutionType);
        Task<ScaleSectionResolutionType> GetByRelations(Guid sectionId, Guid resolutionTypeId);
        Task DeleteByRelations(Guid sectionId, Guid resolutionTypeId);
        Task Delete(ScaleSectionResolutionType sectionResolutionType);
        Task<ScaleSectionResolutionType> GetIncludeResolutionType(Guid id);
    }
}
