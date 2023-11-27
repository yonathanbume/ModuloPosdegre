using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IAnnouncementService
    {
        Task<Announcement> Get(Guid id);
        Task<IEnumerable<Announcement>> GetAll();
        Task Insert(Announcement announcement);
        Task Update(Announcement announcement);
        Task Delete(Announcement announcement);
        Task DeleteById(Guid id);
        Task<object> GetHomeAnnouncement();
    }
}
