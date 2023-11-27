using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class EventTypeService : IEventTypeService
    {
        private readonly IEventTypeRepository _eventTypeRepository;

        public EventTypeService(IEventTypeRepository eventTypeRepository)
        {
            _eventTypeRepository = eventTypeRepository;
        }

        public async Task InsertEventType(EventType eventType) =>
            await _eventTypeRepository.Insert(eventType);

        public async Task UpdateEventType(EventType eventType) =>
            await _eventTypeRepository.Update(eventType);

        public async Task DeleteEventType(EventType eventType) =>
            await _eventTypeRepository.Delete(eventType);

        public async Task<EventType> GetEventTypeById(Guid id) =>
            await _eventTypeRepository.Get(id);

        public async Task<IEnumerable<EventType>> GetAllEventTypes() =>
            await _eventTypeRepository.GetAll();

        public async Task<EventType> GetEventTypeByType(Guid eventType)
            => await _eventTypeRepository.GetEventType(eventType);

        public async Task<List<EventType>> EventTypesDelete()
            => await _eventTypeRepository.EventTypesDelete();

        public async Task<object> GetEventTypes()
            => await _eventTypeRepository.GetEventTypes();

        public async Task<bool> FindAnyEventType(Guid id)
            => await _eventTypeRepository.FindAnyEventType(id);

        public async Task<object> GetAllEventTypesInscription(bool isAll = false)
            => await _eventTypeRepository.GetAllEventTypesInscription(isAll);

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _eventTypeRepository.GetAllDatatable(sentParameters, searchValue);
    }
}
