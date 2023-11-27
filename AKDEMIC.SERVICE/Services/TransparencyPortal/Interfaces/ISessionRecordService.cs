using AKDEMIC.ENTITIES.Models.Portal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ISessionRecordService
    {
        Task Insert(SessionRecord regulation);
        Task<SessionRecord> Get(Guid id);
        Task Update(SessionRecord regulation);
        Task DeleteById(Guid id);
        Task<bool> ExistAnyWithName(Guid id, string name);
        Task<IEnumerable<SessionRecord>> GetAll();
        Task<SessionRecord> GetBySlug(string slug);
    }
}
