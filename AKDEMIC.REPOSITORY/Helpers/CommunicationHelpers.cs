using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Helpers
{
    public class CommunicationHelpers
    {
        public static async Task SendBulkNotificationToUsers(IEnumerable<string> userIds, CommunicationStructs.NotificationHub notificationHub, string connectionString, Action<string> action)
        {
            var connection = new SqlConnection(connectionString);

            using (SqlTransaction sqlTransaction = connection.BeginTransaction())
            {
                var notificationDataTable = new DataTable();

                notificationDataTable.Columns.Add("Id", typeof(Guid));
                notificationDataTable.Columns.Add("SendDate", typeof(DateTime));
                notificationDataTable.Columns.Add("StateColor", typeof(string));
                notificationDataTable.Columns.Add("StateText", typeof(string));
                notificationDataTable.Columns.Add("Text", typeof(string));
                notificationDataTable.Columns.Add("Url", typeof(string));

                var notificationId = Guid.NewGuid();
                var notificationRow = notificationDataTable.NewRow();
                notificationRow[0] = notificationId;
                notificationRow[1] = DateTime.UtcNow;
                notificationRow[2] = notificationHub.StateColor;
                notificationRow[3] = notificationHub.StateText;
                notificationRow[4] = notificationHub.Text;
                notificationRow[5] = notificationHub.Url;

                notificationDataTable.Rows.Add(notificationRow);

                using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, sqlTransaction))
                {
                    sqlBulkCopy.DestinationTableName = ConstantHelpers.HUBS.AKDEMIC.DATABASE.TABLE.NOTIFICATION;

                    await sqlBulkCopy.WriteToServerAsync(notificationDataTable);
                    sqlTransaction.Commit();
                }

                var userNotificationDataTable = new DataTable();
                userNotificationDataTable.Columns.Add("Id", typeof(Guid));
                userNotificationDataTable.Columns.Add("NotificationId", typeof(Guid));
                userNotificationDataTable.Columns.Add("UserId", typeof(string));
                userNotificationDataTable.Columns.Add("IsRead", typeof(bool));

                foreach (var userId in userIds)
                {
                    var userNotificationRow = userNotificationDataTable.NewRow();
                    userNotificationRow[0] = Guid.NewGuid();
                    userNotificationRow[1] = notificationId;
                    userNotificationRow[2] = userId;
                    userNotificationRow[3] = false;

                    action?.Invoke(userId);
                    userNotificationDataTable.Rows.Add(userNotificationRow);
                }

                using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, sqlTransaction))
                {
                    sqlBulkCopy.DestinationTableName = ConstantHelpers.HUBS.AKDEMIC.DATABASE.TABLE.USER_NOTIFICATION;

                    await sqlBulkCopy.WriteToServerAsync(notificationDataTable);
                    sqlTransaction.Commit();
                }
            }
        }

        public static async Task SendNotificationToDependency(AkdemicContext context, Guid dependencyId, CommunicationStructs.NotificationHub notificationHub, Action<string> action = null)
        {
            var userIds = await context.UserDependencies
                .Where(x => x.DependencyId == dependencyId)
                .Select(x => x.UserId)
                .ToListAsync();

            await SendNotificationToUsers(context, userIds, notificationHub, action);
        }

        public static async Task SendNotificationToDependencies(AkdemicContext context, IEnumerable<Guid> dependencyIds, CommunicationStructs.NotificationHub notificationHub, Action<string> action = null)
        {
            foreach (var dependencyId in dependencyIds)
            {
                await SendNotificationToDependency(context, dependencyId, notificationHub, action);
            }
        }

        public static async Task SendNotificationToRole(AkdemicContext context, string roleId, CommunicationStructs.NotificationHub notificationHub, Action<string> action = null)
        {
            var userIds = await context.UserRoles
                .Where(x => x.RoleId == roleId)
                .Select(x => x.UserId)
                .ToListAsync();

            await SendNotificationToUsers(context, userIds, notificationHub, action);
        }

        public static async Task SendNotificationToRoles(AkdemicContext context, IEnumerable<string> roleIds, CommunicationStructs.NotificationHub notificationHub, Action<string> action = null)
        {
            foreach (var roleId in roleIds)
            {
                await SendNotificationToRole(context, roleId, notificationHub, action);
            }
        }

        public static async Task SendNotificationToStudentSection(AkdemicContext context, Guid sectionId, CommunicationStructs.NotificationHub notificationHub, Action<string> action = null)
        {
            var userIds = await context.StudentSections
                .Where(x => x.SectionId == sectionId)
                .Select(x => x.Student.UserId)
                .ToListAsync();

            await SendNotificationToUsers(context, userIds, notificationHub, action);
        }

        public static async Task SendNotificationToStudentSections(AkdemicContext context, IEnumerable<Guid> sectionIds, CommunicationStructs.NotificationHub notificationHub, Action<string> action = null)
        {
            foreach (var sectionId in sectionIds)
            {
                await SendNotificationToStudentSection(context, sectionId, notificationHub, action);
            }
        }

        public static async Task SendNotificationToTeacherSection(AkdemicContext context, Guid sectionId, CommunicationStructs.NotificationHub notificationHub, Action<string> action = null)
        {
            var userIds = await context.TeacherSections
                .Where(x => x.SectionId == sectionId)
                .Select(x => x.Teacher.UserId)
                .ToListAsync();

            await SendNotificationToUsers(context, userIds, notificationHub, action);
        }

        public static async Task SendNotificationToTeacherSections(AkdemicContext context, IEnumerable<Guid> sectionIds, CommunicationStructs.NotificationHub notificationHub, Action<string> action = null)
        {
            foreach (var sectionId in sectionIds)
            {
                await SendNotificationToTeacherSection(context, sectionId, notificationHub, action);
            }
        }

        public static async Task SendNotificationToUser(AkdemicContext context, string userId, CommunicationStructs.NotificationHub notificationHub, Action<string> action = null)
        {
            var notification = new Notification
            {
                SendDate = DateTime.UtcNow,
                //StateColor = notificationHub.StateColor,
                StateText = notificationHub.StateText,
                Text = notificationHub.Text,
                Url = notificationHub.Url
            };

            await context.Notifications.AddAsync(notification);
            await context.SaveChangesAsync();

            var userNotification = new UserNotification
            {
                NotificationId = notification.Id,
                UserId = userId
            };

            action?.Invoke(userId);
            await context.UserNotifications.AddAsync(userNotification);
            await context.SaveChangesAsync();
        }

        public static async Task SendNotificationToUsers(AkdemicContext context, IEnumerable<string> userIds, CommunicationStructs.NotificationHub notificationHub, Action<string> action = null)
        {
            if (userIds.LongCount() < ConstantHelpers.ENTITY_FRAMEWORK.RECORD_LIMIT)
            {
                var dbConnection = context.Database.GetDbConnection();

                await SendBulkNotificationToUsers(userIds, notificationHub, dbConnection.ConnectionString, action);
            }
            else
            {
                var notification = new Notification
                {
                    SendDate = DateTime.UtcNow,
                    //StateColor = notificationHub.StateColor,
                    StateText = notificationHub.StateText,
                    Text = notificationHub.Text,
                    Url = notificationHub.Url
                };

                await context.Notifications.AddAsync(notification);
                await context.SaveChangesAsync();

                foreach (var userId in userIds)
                {
                    var userNotification = new UserNotification
                    {
                        NotificationId = notification.Id,
                        UserId = userId
                    };

                    action?.Invoke(userId);
                    await context.UserNotifications.AddAsync(userNotification);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
