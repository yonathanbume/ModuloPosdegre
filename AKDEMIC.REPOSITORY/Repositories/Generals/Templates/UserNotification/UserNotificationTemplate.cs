using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.UserNotification
{
    public class UserNotificationTemplate
    {
        public string NotificationMessage { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public string SendDate { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string UserId { get; set; }
    }
}
