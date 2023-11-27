using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Event;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EventRole;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IEventService
    {
        Task InsertEvent(Event eventt);

        Task UpdateEvent(Event eventt);

        Task DeleteEvent(Event eventt);

        Task<Event> GetEventById(Guid id);
        Task<Event> GetEventWithIncludeById(Guid id);
        Task<EventTemplate> GetEventData(Guid id);

        Task<IEnumerable<Event>> GetaLLEvents();
        Task<List<EventTemplate>> GetEventsSiscoToHome();
        Task<List<EventTemplate>> GetEventsSiscoAllToHome();
        Task<ReportEventTemplate> GetEventWithUserEvents(Guid eid);
        Task<DataTablesStructs.ReturnedData<EventDataTableTemplate>> GetEventsDataTable(DataTablesStructs.SentParameters sentParameters);
        Task<object> GetUserRoles();
        Task<DataTablesStructs.ReturnedData<object>> GetEvents(DataTablesStructs.SentParameters sentParameters, byte system, Guid eventTypeId, string searchValue = null, string organizerId = null, bool isAdmin = false);     
        Task<EventInscriptionTemplate> GetEventInscriptionById(Guid id);
        Task<List<EventInscriptionTemplate>> GetTwoNextUpcomingEvents();
    }
}
