using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class EventFileService : IEventFileService
    {
        private readonly IEventFileRepository _eventFileRepository;

        public EventFileService(IEventFileRepository eventFileRepository)
        {
            _eventFileRepository = eventFileRepository;
        }

        public async Task Delete(EventFile entity)
            => await _eventFileRepository.Delete(entity);

        public async Task<EventFile> Get(Guid id)
            => await _eventFileRepository.Get(id);

        public async Task<List<EventFile>> GetAllByEvent(Guid eventId)
            => await _eventFileRepository.GetAllByEvent(eventId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid eventId)
            => await _eventFileRepository.GetDatatable(parameters, eventId);

        public async Task Insert(EventFile entity)
            => await _eventFileRepository.Insert(entity);

        public async Task Update(EventFile entity)
            => await _eventFileRepository.Update(entity);
    }
}
