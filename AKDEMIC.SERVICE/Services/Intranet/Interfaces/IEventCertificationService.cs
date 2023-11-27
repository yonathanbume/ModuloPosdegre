using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IEventCertificationService
    {
        Task<EventCertification> Get(Guid id);
        Task<IEnumerable<EventCertification>> GetAll();
        Task Insert(EventCertification eventCertification);
        Task Update(EventCertification eventCertification);
        Task Delete(EventCertification eventCertification);
    }
}
