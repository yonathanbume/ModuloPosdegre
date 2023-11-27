using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IPostulantCardSectionRepository:IRepository<PostulantCardSection>
    {
        Task<List<PostulantCardSection>> GetConfiguration(Guid admissionTypeId);
        Task SaveConfiguration(Guid id, List<PostulantCardSection> sections);
    }
}
