using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IEventTypeRepository : IRepository<EventType>
    {
        Task<EventType> GetEventType(Guid eventType);
        Task<List<EventType>> EventTypesDelete();
        Task<object> GetEventTypes();
        Task<bool> FindAnyEventType(Guid id);
        Task<object> GetAllEventTypesInscription(bool isAll = false);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
