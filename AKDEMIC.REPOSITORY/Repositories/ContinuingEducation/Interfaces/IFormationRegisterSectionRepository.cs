using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Interfaces
{
    public interface IFormationRegisterSectionRepository:IRepository<ENTITIES.Models.ContinuingEducation.RegisterSection>
    {
        Task<bool> AnyBySectionUser(Guid sectionId, string userId);
        Task<bool> AnyBySectionDni(Guid sectionId, string dni);
    }
}
