using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Event;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EventRole;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<List<EventTemplate>> GetEventsSiscoToHome();
        Task<List<EventTemplate>> GetEventsSiscoAllToHome();
        Task<Event> GetEventWithIncludeById(Guid id);
        Task<EventTemplate> GetEventData(Guid id);
        Task<DataTablesStructs.ReturnedData<EventDataTableTemplate>> GetEventsDataTable(DataTablesStructs.SentParameters sentParameters);
        Task<ReportEventTemplate> GetUserEventByEventId(Guid eid);
        Task<object> GetUserRoles();
        Task<DataTablesStructs.ReturnedData<object>> GetEvents(DataTablesStructs.SentParameters sentParameters, byte system, Guid eventTypeId, string searchValue = null, string organizerId = null, bool isAdmin = false);
        Task<EventInscriptionTemplate> GetEventInscriptionById(Guid id);
        Task<List<EventInscriptionTemplate>> GetTwoNextUpcomingEvents();
    }
}
