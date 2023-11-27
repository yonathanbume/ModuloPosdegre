using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.UserNotification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IUserNotificationRepository : IRepository<UserNotification>
    {
        Task<int> CountReadUserNotifications(string userId);
        Task<int> CountUnreadUserNotifications(string userId);
        Task<int> CountUserNotifications(string userId);
        Task<IEnumerable<UserNotification>> GetReadUserNotifications(string userId);
        Task<IEnumerable<UserNotification>> GetReadUserNotifications(string userId, int count);
        Task<IEnumerable<UserNotification>> GetUnreadUserNotifications(string userId);
        Task<IEnumerable<UserNotification>> GetUnreadUserNotifications(string userId, int count);
        Task<IEnumerable<UserNotification>> GetUserNotifications(string userId);
        Task<IEnumerable<UserNotification>> GetUserNotifications(string userId, int count);
        Task<CommunicationStructs.NotificationResponseParameters> GetUserNotificationsCommunication(CommunicationStructs.NotificationRequestParameters notificationRequestParameters);
        Task<UserNotification> GetNotificationHomeByUser(Guid id);
        Task<List<UserNotificationTemplate>> GetUserNotificationsByFilters(string userId, bool? isRead = null, int? count = null);
    }
}
