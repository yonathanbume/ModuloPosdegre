using AKDEMIC.ENTITIES.Models.Portal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface IInstitutionalActivityService
    {
        Task Insert(InstitutionalActivity regulation);
        Task<InstitutionalActivity> Get(Guid id);
        Task Update(InstitutionalActivity regulation);
        Task DeleteById(Guid id);
        Task<bool> ExistAnyWithName(Guid id, string name);
        Task<IEnumerable<InstitutionalActivity>> GetAll();
        Task<IEnumerable<InstitutionalActivity>> GetBySlug(string slug);
    }
}
