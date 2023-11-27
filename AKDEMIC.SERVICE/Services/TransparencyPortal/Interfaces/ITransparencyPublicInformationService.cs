using AKDEMIC.ENTITIES.Models.Portal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ITransparencyPublicInformationService
    {
        Task Insert(TransparencyPublicInformation regulation);
        Task<TransparencyPublicInformation> Get(Guid id);
        Task Update(TransparencyPublicInformation regulation);
        Task DeleteById(Guid id);
        Task<bool> ExistAnyWithName(Guid id, string name);
        Task<IEnumerable<TransparencyPublicInformation>> GetAll();
        Task<IEnumerable<TransparencyPublicInformation>> GetBySlug(string slug);
    }
}
