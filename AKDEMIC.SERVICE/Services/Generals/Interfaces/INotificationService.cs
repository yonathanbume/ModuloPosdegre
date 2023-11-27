using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface INotificationService
    {
        Task<Notification> Get(Guid id);
        Task<IEnumerable<Notification>> GetAll();
        Task Insert(Notification notification);
        Task Update(Notification notification);
        Task Delete(Notification notification);
        Task Add(Notification notification);
    }
}
