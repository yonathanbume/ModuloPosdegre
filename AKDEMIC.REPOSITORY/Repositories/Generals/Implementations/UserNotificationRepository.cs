using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.UserNotification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class UserNotificationRepository : Repository<UserNotification>, IUserNotificationRepository
    {
        public UserNotificationRepository(AkdemicContext context) : base(context) { }

        public async Task<int> CountReadUserNotifications(string userId)
        {
            var query = _context.UserNotifications
                .Where(x => x.IsRead)
                .Where(x => x.UserId == userId);

            return await query.CountAsync();
        }

        public async Task<int> CountUnreadUserNotifications(string userId)
        {
            var query = _context.UserNotifications
                .Where(x => !x.IsRead)
                .Where(x => x.UserId == userId);

            return await query.CountAsync();
        }

        public async Task<int> CountUserNotifications(string userId)
        {
            var query = _context.UserNotifications.Where(x => x.UserId == userId);

            return await query.CountAsync();
        }

        public async Task<IEnumerable<UserNotification>> GetReadUserNotifications(string userId)
        {
            var result = _context.UserNotifications
                .Where(x => x.IsRead)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Notification.SendDate)
                .SelectUserNotification();

            return await result.ToListAsync();
        }

        public async Task<IEnumerable<UserNotification>> GetReadUserNotifications(string userId, int count)
        {
            var result = _context.UserNotifications
                .Where(x => x.IsRead)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Notification.SendDate)
                .Skip(count * ConstantHelpers.NOTIFICATIONS.DEFAULT_PAGE_SIZE)
                .Take(ConstantHelpers.NOTIFICATIONS.DEFAULT_PAGE_SIZE)
                .SelectUserNotification();

            return await result.ToListAsync();
        }

        public async Task<IEnumerable<UserNotification>> GetUnreadUserNotifications(string userId)
        {
            var result = _context.UserNotifications
                .Where(x => !x.IsRead)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Notification.SendDate)
                .SelectUserNotification();

            return await result.ToListAsync();
        }

        public async Task<IEnumerable<UserNotification>> GetUnreadUserNotifications(string userId, int count)
        {
            var result = _context.UserNotifications
                .Where(x => !x.IsRead)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Notification.SendDate)
                .Skip(count * ConstantHelpers.NOTIFICATIONS.DEFAULT_PAGE_SIZE)
                .Take(ConstantHelpers.NOTIFICATIONS.DEFAULT_PAGE_SIZE)
                .SelectUserNotification();

            return await result.ToListAsync();
        }

        public async Task<IEnumerable<UserNotification>> GetUserNotifications(string userId)
        {
            var result = _context.UserNotifications
                .Where(x => !x.IsRead)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Notification.SendDate)
                .SelectUserNotification();

            return await result.ToListAsync();
        }

        public async Task<IEnumerable<UserNotification>> GetUserNotifications(string userId, int count)
        {
            var result = _context.UserNotifications
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Notification.SendDate)
                .Skip(count * ConstantHelpers.NOTIFICATIONS.DEFAULT_PAGE_SIZE)
                .Take(ConstantHelpers.NOTIFICATIONS.DEFAULT_PAGE_SIZE)
                .SelectUserNotification();

            return await result.ToListAsync();
        }

        public async Task<CommunicationStructs.NotificationResponseParameters> GetUserNotificationsCommunication(CommunicationStructs.NotificationRequestParameters notificationRequestParameters)
        {
            var query = _context.UserNotifications
                .Where(x => x.UserId == notificationRequestParameters.UserId)
                .AsNoTracking();

            if (notificationRequestParameters.Records != null && notificationRequestParameters.Records > 0)
            {
                if (notificationRequestParameters.Page != null && notificationRequestParameters.Page > 0)
                {
                    query = query.Skip(notificationRequestParameters.Page.Value);
                }

                query = query.Take(notificationRequestParameters.Records.Value);
            }

            var read = await query
                .Where(x => x.IsRead)
                .CountAsync();
            var unread = await query
                .Where(x => !x.IsRead)
                .CountAsync();
            var notificationResults = await query
                .OrderByDescending(x => x.Notification.SendDate)
                .Select(x => new CommunicationStructs.NotificationResult
                {
                    Id = x.Id,
                    //Ellapsed = x.Notification.SendDate.ElapsedTime(),
                    EllapsedTimeText = x.Notification.SendDate.ElapsedTimeText(),
                    IsRead = x.IsRead,
                    SendDate = x.Notification.SendDate.ToLocalTime(),
                    StateColor = ConstantHelpers.COLORS.BADGE.STATE.VALUES[x.Notification.StateColor],
                    //StateColorText = x.Notification.StateColor,
                    StateText = x.Notification.StateText,
                    Text = x.Notification.Text,
                    Url = x.Notification.Url
                })
                .ToListAsync();

            return new CommunicationStructs.NotificationResponseParameters
            {
                Read = read,
                Unread = unread,
                NotificationResults = notificationResults
            };
        }
        public async Task<UserNotification> GetNotificationHomeByUser(Guid id)
        {
            var userNotification = await _context.UserNotifications.Include(x => x.Notification).FirstOrDefaultAsync(x => x.Id == id);

            return userNotification;
        }

        public async Task<List<UserNotificationTemplate>> GetUserNotificationsByFilters(string userId, bool? isRead = null, int? count = null)
        {
            var query = _context.UserNotifications
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Notification.SendDate)
                .AsNoTracking();

            if (isRead != null)
                query = query.Where(x => x.IsRead == isRead);

            if (count != null)
                query = query.Take(count.Value);


            var userNotifications = await query
                .Select(x => new UserNotificationTemplate
                {
                    NotificationMessage = x.Notification.Text,
                    SendDate = x.Notification.SendDate.ToLocalDateFormat(),
                    FullName = x.User.FullName,
                    UserId = x.UserId,
                    UserName = x.User.UserName,
                    IsRead = x.IsRead,
                    ReadDate = x.ReadDate
                })
                .ToListAsync();

            return userNotifications;
        }
    }
}
