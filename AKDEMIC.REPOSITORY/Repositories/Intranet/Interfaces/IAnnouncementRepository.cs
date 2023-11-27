using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IAnnouncementRepository : IRepository<Announcement>
    {
        Task<object> GetHomeAnnouncement();
    }
}
