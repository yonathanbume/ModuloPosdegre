using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Implementations
{
    public class ConferenceUserRepository : Repository<ConferenceUser>,IConferenceUserRepository
    {
        public ConferenceUserRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<ConferenceUser>> GetConferenceUsersByConferenceId(Guid conferenceId)
            => await _context.ConferenceUsers.Where(x => x.ConferenceId == conferenceId).ToArrayAsync();
    }
}
