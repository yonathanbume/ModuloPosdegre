using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IPostulantCardSectionService
    {
        Task<List<PostulantCardSection>> GetConfiguration(Guid admissionTypeId);
        Task SaveConfiguration(Guid id, List<PostulantCardSection> sections);
    }
}
