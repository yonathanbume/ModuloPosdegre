using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class EventCareerService : IEventCareerService
    {
        private readonly IEventCareerRepository _eventCareerRepository;
        public EventCareerService(IEventCareerRepository eventCareerRepository)
        {
            _eventCareerRepository = eventCareerRepository;
        }

        public Task Delete(EventCareer eventCareer)
            => _eventCareerRepository.Delete(eventCareer);

        public Task DeleteRange(List<EventCareer> eventCareers)
            => _eventCareerRepository.DeleteRange(eventCareers);

        public Task<EventCareer> Get(Guid id)
            => _eventCareerRepository.Get(id);

        public Task<IEnumerable<EventCareer>> GetAll()
            => _eventCareerRepository.GetAll();

        public Task<List<EventCareer>> GetAllByEventId(Guid eventId)
            => _eventCareerRepository.GetAllByEventId(eventId);

        public Task Insert(EventCareer eventCareer)
            => _eventCareerRepository.Insert(eventCareer);

        public Task Update(EventCareer eventCareer)
            => _eventCareerRepository.Update(eventCareer);
    }
}
