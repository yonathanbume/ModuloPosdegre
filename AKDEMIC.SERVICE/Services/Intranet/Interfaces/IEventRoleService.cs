using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EventRole;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IEventRoleService
    {
        Task InsertEventRole(EventRole eventRole);

        Task UpdateEventRole(EventRole eventRole);

        Task DeleteEventRole(EventRole eventRole);

        Task<EventRole> GetEventRoleById(Guid id);

        Task<IEnumerable<EventRole>> GetaLLEventRoles();

        Task<List<EventInscriptionTemplate>> GetEventsInscription(IList<string> roles, string userId, bool? isPublic = null, byte? system = null);
        Task<List<EventInscriptionTemplate>> GetPublicEvents(string userId = null, byte? system = null);
        Task<object> GetEventHome(IList<string> roles);
        Task<List<EventRole>> GetAllEventRolesByEventId(Guid id);
        Task DeleteRange(List<EventRole> roles);
    }
}
