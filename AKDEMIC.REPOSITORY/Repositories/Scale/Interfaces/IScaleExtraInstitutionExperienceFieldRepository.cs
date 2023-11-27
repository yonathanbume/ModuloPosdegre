using System;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScaleExtraInstitutionExperienceFieldRepository : IRepository<ScaleExtraInstitutionExperienceField>
    {
        Task<ScaleExtraInstitutionExperienceField> GetScaleExtraInstitutionExperienceFieldByResolutionId(Guid resolutionId);
    }
}
