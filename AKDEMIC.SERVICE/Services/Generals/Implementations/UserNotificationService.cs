using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.UserNotification;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IUserNotificationRepository _userNotificationRepository;

        public UserNotificationService(IUserNotificationRepository userNotificationRepository)
        {
            _userNotificationRepository = userNotificationRepository;
        }

        public async Task<int> CountReadUserNotifications(string userId)
        {
            return await _userNotificationRepository.CountReadUserNotifications(userId);
        }

        public async Task<int> CountUnreadUserNotifications(string userId)
        {
            return await _userNotificationRepository.CountUnreadUserNotifications(userId);
        }

        public async Task<int> CountUserNotifications(string userId)
        {
            return await _userNotificationRepository.CountUserNotifications(userId);
        }

        public async Task<IEnumerable<UserNotification>> GetReadUserNotifications(string userId)
        {
            return await _userNotificationRepository.GetReadUserNotifications(userId);
        }

        public async Task<IEnumerable<UserNotification>> GetReadUserNotifications(string userId, int count)
        {
            return await _userNotificationRepository.GetReadUserNotifications(userId, count);
        }

        public async Task<IEnumerable<UserNotification>> GetUnreadUserNotifications(string userId)
        {
            return await _userNotificationRepository.GetUnreadUserNotifications(userId);
        }

        public async Task<IEnumerable<UserNotification>> GetUnreadUserNotifications(string userId, int count)
        {
            return await _userNotificationRepository.GetUnreadUserNotifications(userId, count);
        }

        public async Task<IEnumerable<UserNotification>> GetUserNotifications(string userId)
        {
            return await _userNotificationRepository.GetUserNotifications(userId);
        }

        public async Task<IEnumerable<UserNotification>> GetUserNotifications(string userId, int count)
        {
            return await _userNotificationRepository.GetUserNotifications(userId, count);
        }

        public async Task<CommunicationStructs.NotificationResponseParameters> GetUserNotificationsCommunication(CommunicationStructs.NotificationRequestParameters notificationRequestParameters)
        {
            return await _userNotificationRepository.GetUserNotificationsCommunication(notificationRequestParameters);
        }

        public async Task Delete(UserNotification userNotification)
        {
            await _userNotificationRepository.Delete(userNotification);
        }

        public async Task Insert(UserNotification userNotification)
        {
            await _userNotificationRepository.Insert(userNotification);
        }

        public async Task Update(UserNotification userNotification)
        {
            await _userNotificationRepository.Update(userNotification);
        }

        public async Task<UserNotification> GetNotificationHomeByUser(Guid id)
            => await _userNotificationRepository.GetNotificationHomeByUser(id);

        public Task<UserNotification> Get(Guid id)
            => _userNotificationRepository.Get(id);

        public Task<IEnumerable<UserNotification>> GetAll()
            => _userNotificationRepository.GetAll();

        public Task Add(UserNotification userNotification)
            => _userNotificationRepository.Add(userNotification);

        public Task<List<UserNotificationTemplate>> GetUserNotificationsByFilters(string userId, bool? isRead = null, int? count = null)
            => _userNotificationRepository.GetUserNotificationsByFilters(userId , isRead, count);
    }
}
