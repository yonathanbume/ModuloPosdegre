using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces
{
    public interface IConferenceUserRepository : IRepository<ConferenceUser>
    {
        Task<IEnumerable<ConferenceUser>> GetConferenceUsersByConferenceId(Guid conferenceId);
    }
}
