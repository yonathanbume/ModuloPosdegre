using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ContinuingEducation.Interfaces
{
    public interface IFormationRegisterSectionService
    {
        Task<ENTITIES.Models.ContinuingEducation.RegisterSection> Get(Guid id);
        Task<bool> AnyBySectionUser(Guid sectionId, string userId);
        Task<bool> AnyBySectionDni(Guid sectionId, string dni);
        Task<IEnumerable<ENTITIES.Models.ContinuingEducation.RegisterSection>> GetAll();
        Task Insert(ENTITIES.Models.ContinuingEducation.RegisterSection formationRegisterSection);
        Task Update(ENTITIES.Models.ContinuingEducation.RegisterSection formationRegisterSection);
        Task Delete(ENTITIES.Models.ContinuingEducation.RegisterSection formationRegisterSection);

    }
}
