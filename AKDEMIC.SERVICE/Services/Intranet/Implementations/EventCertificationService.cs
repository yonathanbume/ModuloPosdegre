using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class EventCertificationService : IEventCertificationService
    {
        private readonly IEventCertificationRepository _eventCertificationRepository;
        public EventCertificationService(IEventCertificationRepository eventCertificationRepository)
        {
            _eventCertificationRepository = eventCertificationRepository;
        }
        public Task Delete(EventCertification eventCertification)
            => _eventCertificationRepository.Delete(eventCertification);

        public Task<EventCertification> Get(Guid id)
            => _eventCertificationRepository.Get(id);

        public Task<IEnumerable<EventCertification>> GetAll()
            => _eventCertificationRepository.GetAll();

        public Task Insert(EventCertification eventCertification)
            => _eventCertificationRepository.Insert(eventCertification);

        public Task Update(EventCertification eventCertification)
            => _eventCertificationRepository.Update(eventCertification);
    }
}
