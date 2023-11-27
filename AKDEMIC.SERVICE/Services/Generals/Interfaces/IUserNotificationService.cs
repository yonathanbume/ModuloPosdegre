using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.UserNotification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IUserNotificationService
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
        Task Delete(UserNotification userNotification);
        Task Insert(UserNotification userNotification);
        Task Update(UserNotification userNotification);
        Task<UserNotification> Get(Guid id);
        Task<IEnumerable<UserNotification>> GetAll();
        Task Add(UserNotification userNotification);
        Task<List<UserNotificationTemplate>> GetUserNotificationsByFilters(string userId, bool? isRead = null, int? count = null);
        Task<UserNotification> GetNotificationHomeByUser(Guid id);
    }
}
