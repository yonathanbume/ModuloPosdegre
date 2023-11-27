using AKDEMIC.ENTITIES.Models.InterestGroup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Interfaces
{
    public interface IConferenceUserService
    {
        Task InsertRange(IEnumerable<ConferenceUser> entities);
        Task Insert(ConferenceUser entity);
        Task DeleteRange(IEnumerable<ConferenceUser> entities);
        Task<IEnumerable<ConferenceUser>> GetConferenceUsersByConferenceId(Guid conferenceId);
    }
}
