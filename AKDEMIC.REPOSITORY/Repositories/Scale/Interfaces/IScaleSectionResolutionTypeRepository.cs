using System;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScaleSectionResolutionTypeRepository : IRepository<ScaleSectionResolutionType>
    {
        Task<bool> IsActive(Guid sectionId, Guid resolutionTypeId);
        Task<ScaleSectionResolutionType> GetByRelations(Guid sectionId, Guid resolutionTypeId);
        Task DeleteByRelations(Guid sectionId, Guid resolutionTypeId);
        Task<ScaleSectionResolutionType> GetIncludeResolutionType(Guid id);
    }
}
