using AKDEMIC.ENTITIES.Models;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Hubs
{
    [Authorize]
    public class AkdemicHub : Hub
    {
        private readonly AkdemicContext _dbContext;

        public AkdemicHub(AkdemicContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SendNotification(List<string> usersIds, string link)
        {
            var url = link;
            var text = "Nueva Encuesta";
            string stateText = null;
            var backgroundStateClass = 0;

            var notification = new Notification
            {
                BackgroundStateClass = backgroundStateClass,
                State = stateText,
                Text = text,
                Url = url
            };

            await _dbContext.Notifications.AddAsync(notification);
            foreach (var userId in usersIds)
            {
                try
                {
                    var userNotification = new UserNotification
                    {
                        NotificationId = notification.Id,
                        UserId = userId
                    };

                    await _dbContext.UserNotifications.AddAsync(userNotification);
                    await Clients.User(userId).SendAsync("ReceiveNotification", 1);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task SendGrades(List<string> usersIds)
        {
            var url = "/alumno/notas";
            var text = "Nueva nota cargada";
            string stateText = null;
            var backgroundStateClass = 0;

            var notification = new Notification
            {
                BackgroundStateClass = backgroundStateClass,
                State = stateText,
                Text = text,
                Url = url
            };

            await _dbContext.Notifications.AddAsync(notification);
            foreach (var userId in usersIds)
            {
                try
                {
                    var userNotification = new UserNotification
                    {
                        NotificationId = notification.Id,
                        UserId = userId,
                    };

                    await _dbContext.UserNotifications.AddAsync(userNotification);
                    await Clients.User(userId).SendAsync("ReceiveNotification", 2);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            try
            {
                _dbContext.Connections.Add(new Connection { Code = Context.ConnectionId, UserId = Context.UserIdentifier });
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return;
            }
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            try
            {
                await base.OnDisconnectedAsync(ex);

                var objConnection = await _dbContext.Connections.FirstOrDefaultAsync(x => x.Code == Context.ConnectionId);
                _dbContext.Connections.Remove(objConnection);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                await base.OnDisconnectedAsync(ex);
            }
        }
    }
}
