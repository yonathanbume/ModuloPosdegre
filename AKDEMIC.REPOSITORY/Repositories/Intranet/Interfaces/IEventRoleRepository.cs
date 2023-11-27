using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EventRole;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IEventRoleRepository : IRepository<EventRole> 
    {
        Task<List<EventInscriptionTemplate>> GetEventsInscription(IList<string> roles, string userId, bool? isPublic = null, byte? system = null);
        Task<List<EventInscriptionTemplate>> GetPublicEvents(string userId = null, byte? system = null);
        Task<object> GetEventHome(IList<string> roles);
        Task<List<EventRole>> GetAllEventRolesByEventId(Guid id);
    }
}
