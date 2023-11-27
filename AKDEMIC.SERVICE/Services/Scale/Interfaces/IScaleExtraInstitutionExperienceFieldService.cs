using System;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleExtraInstitutionExperienceFieldService
    {
        Task<ScaleExtraInstitutionExperienceField> Get(Guid id);
        Task Insert(ScaleExtraInstitutionExperienceField entity);
        Task Update(ScaleExtraInstitutionExperienceField entity);
        Task Delete(ScaleExtraInstitutionExperienceField entity);
        Task<ScaleExtraInstitutionExperienceField> GetScaleExtraInstitutionExperienceFieldByResolutionId(Guid resolutionId);
    }
}
