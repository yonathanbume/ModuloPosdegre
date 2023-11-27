using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.Hubs;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System.Collections.Generic;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates;

namespace AKDEMIC.INTRANET.Controllers
{
    [Authorize]
    [Route("notification")]
    public class NotificationController : BaseController
    {
        private readonly IUserNotificationService _userNotificationService;
        private readonly ITutoringMessageService _tutoringMessageService;
        private readonly ITutoringStudentService _tutoringStudentService;

        public NotificationController(IUserService userService,
            IHubContext<AkdemicHub> hubContext,
            ITutoringMessageService tutoringMessageService,
            ITutoringStudentService tutoringStudentService,
            IUserNotificationService userNotificationService) : base(userService, hubContext)
        {
            _userNotificationService = userNotificationService;
            _tutoringMessageService = tutoringMessageService;
            _tutoringStudentService = tutoringStudentService;
        }

        /// <summary>
        /// Obtiene las notificaciones del usuario logeado
        /// </summary>
        /// <param name="count">Indicador de pagina</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("get/{count?}")]
        public async Task<IActionResult> GetNotifications(int? count)
        {
            var user = await GetCurrentUserAsync();
            var userNotifications = await _userNotificationService.GetUserNotifications(user.Id, count ?? 0);
            var pending = await _userNotificationService.CountUnreadUserNotifications(user.Id);
            var result = userNotifications
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Notification.Text,
                    state_text = x.Notification.State ?? "",
                    background_state_class = ConstantHelpers.NOTIFICATIONS.COLORS.LABELS[x.Notification.BackgroundStateClass],
                    url = x.Notification.Url,
                    date = x.Notification.SendDate.ElapsedTime(),
                    _date = x.Notification.SendDate.ToLocalTime(),
                    readed = x.IsRead
                })
                .ToList();

            var messages = new List<TutoringMessagesTemplate>();
            //Mensajes de Tutoria
            var tutoringStudent = await _tutoringStudentService.GetByUserId(user.Id);

            if (tutoringStudent != null)
            {
                var tutoringMessages = await _tutoringMessageService.GetAllByTutoringStudentIdAndTutorId(tutoringStudent.StudentId);
                messages = tutoringMessages.Select(x => new TutoringMessagesTemplate
                {
                    Title = x.Title,
                    Message = x.Message,
                    CreatedAt = x.CreatedAt?.ElapsedTime()
                }).ToList();
            }



            return Ok(new { result, pending, messages });
        }

        /// <summary>
        /// Obtiene las notificaciones no leidas del usuario logeado
        /// </summary>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("noreaded/count/get")]
        public async Task<IActionResult> GetCountNoReadedNotifications()
        {
            var user = await GetCurrentUserAsync();
            var pending = await _userNotificationService.CountUnreadUserNotifications(user.Id);

            return Ok(pending);
        }
    }
}
