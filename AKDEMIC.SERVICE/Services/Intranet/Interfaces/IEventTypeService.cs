using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IEventTypeService
    {
        Task InsertEventType(EventType eventType);
        Task UpdateEventType(EventType eventType);
        Task DeleteEventType(EventType eventType);
        Task<EventType> GetEventTypeById(Guid id);
        Task<IEnumerable<EventType>> GetAllEventTypes();
        Task<EventType> GetEventTypeByType(Guid eventType);
        Task<List<EventType>> EventTypesDelete();
        Task<object> GetEventTypes();
        Task<bool> FindAnyEventType(Guid id);
        Task<object> GetAllEventTypesInscription(bool isAll = false);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
