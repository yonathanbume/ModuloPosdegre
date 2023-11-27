using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IEventCareerService
    {
        Task Insert(EventCareer eventCareer);
        Task Update(EventCareer eventCareer);
        Task Delete(EventCareer eventCareer);
        Task<EventCareer> Get(Guid id);
        Task<IEnumerable<EventCareer>> GetAll();
        Task DeleteRange(List<EventCareer> eventCareers);
        Task<List<EventCareer>> GetAllByEventId(Guid eventId);
    }
}
