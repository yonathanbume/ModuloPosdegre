using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class EventCareerRepository: Repository<EventCareer> , IEventCareerRepository
    {
        public EventCareerRepository(AkdemicContext context) : base(context) { }

        public async Task<List<EventCareer>> GetAllByEventId(Guid eventId)
        {
            var result = await _context.EventCareers.Where(x => x.EventId == eventId).ToListAsync();

            return result;
        }
    }
}
