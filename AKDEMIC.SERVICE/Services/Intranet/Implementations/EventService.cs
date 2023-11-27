using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Event;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EventRole;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task InsertEvent(Event eventt) =>
            await _eventRepository.Insert(eventt);

        public async Task UpdateEvent(Event eventt) =>
            await _eventRepository.Update(eventt);

        public async Task DeleteEvent(Event eventt) =>
            await _eventRepository.Delete(eventt);

        public async Task<Event> GetEventById(Guid id) =>
            await _eventRepository.Get(id);

        public Task<Event> GetEventWithIncludeById(Guid id)
            => _eventRepository.GetEventWithIncludeById(id);

        public async Task<IEnumerable<Event>> GetaLLEvents() =>
            await _eventRepository.GetAll();

        public async Task<List<EventTemplate>> GetEventsSiscoToHome() =>
            await _eventRepository.GetEventsSiscoToHome();
        public async Task<List<EventTemplate>> GetEventsSiscoAllToHome() =>
            await _eventRepository.GetEventsSiscoAllToHome();

        public Task<DataTablesStructs.ReturnedData<EventDataTableTemplate>> GetEventsDataTable(DataTablesStructs.SentParameters sentParameters)
            => _eventRepository.GetEventsDataTable(sentParameters);

        public async Task<ReportEventTemplate> GetEventWithUserEvents(Guid eid)
        {
            return await _eventRepository.GetUserEventByEventId(eid);
        }

        public async Task<object> GetUserRoles()
            => await _eventRepository.GetUserRoles();

        public async Task<DataTablesStructs.ReturnedData<object>> GetEvents(DataTablesStructs.SentParameters sentParameters, byte system, Guid eventTypeId, string searchValue = null, string organizerId = null, bool isAdmin = false)
            => await _eventRepository.GetEvents(sentParameters, system, eventTypeId, searchValue, organizerId, isAdmin);

        public async Task<EventInscriptionTemplate> GetEventInscriptionById(Guid id)
            => await _eventRepository.GetEventInscriptionById(id);

        public async Task<List<EventInscriptionTemplate>> GetTwoNextUpcomingEvents()
        {
            return await _eventRepository.GetTwoNextUpcomingEvents();
        }

        public Task<EventTemplate> GetEventData(Guid id)
            => _eventRepository.GetEventData(id);
    }
}
