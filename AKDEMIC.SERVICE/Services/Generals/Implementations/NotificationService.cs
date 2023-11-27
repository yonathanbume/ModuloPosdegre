using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public Task Add(Notification notification)
            => _notificationRepository.Add(notification);

        public Task Delete(Notification notification)
            => _notificationRepository.Delete(notification);

        public Task<Notification> Get(Guid id)
            => _notificationRepository.Get(id);

        public Task<IEnumerable<Notification>> GetAll()
            => _notificationRepository.GetAll();

        public Task Insert(Notification notification)
            => _notificationRepository.Insert(notification);

        public Task Update(Notification notification)
            => _notificationRepository.Update(notification);
    }
}
