using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EventRole;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class EventRoleService : IEventRoleService
    {
        private readonly IEventRoleRepository _eventRoleRepository;
        public EventRoleService(IEventRoleRepository eventRoleRepository)
        {
            _eventRoleRepository = eventRoleRepository;
        }
        public async Task InsertEventRole(EventRole eventRole) =>
            await _eventRoleRepository.Insert(eventRole);

        public async Task UpdateEventRole(EventRole eventRole) =>
            await _eventRoleRepository.Update(eventRole);

        public async Task DeleteEventRole(EventRole eventRole) =>
            await _eventRoleRepository.Delete(eventRole);

        public async Task<EventRole> GetEventRoleById(Guid id) =>
            await _eventRoleRepository.Get(id);

        public async Task<IEnumerable<EventRole>> GetaLLEventRoles() =>
            await _eventRoleRepository.GetAll();

        public async Task<List<EventInscriptionTemplate>> GetEventsInscription(IList<string> roles, string userId, bool? isPublic = null, byte? system = null)
            => await _eventRoleRepository.GetEventsInscription(roles, userId, isPublic, system);
        public Task<List<EventInscriptionTemplate>> GetPublicEvents(string userId = null, byte? system = null)
             => _eventRoleRepository.GetPublicEvents(userId, system);
        public async Task<object> GetEventHome(IList<string> roles)
            => await _eventRoleRepository.GetEventHome(roles);

        public async Task<List<EventRole>> GetAllEventRolesByEventId(Guid id)
        {
            return await _eventRoleRepository.GetAllEventRolesByEventId(id);
        }

        public async Task DeleteRange(List<EventRole> roles)
        {
            await _eventRoleRepository.DeleteRange(roles);
        }
    }
}
