using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class Notification
    {
        public Guid Id { get; set; }

        public int BackgroundStateClass { get; set; }
        public DateTime SendDate { get; set; } = DateTime.UtcNow;
        public string State { get; set; }
        public int StateColor { get; set; } = 0;
        public string StateText { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }

        public IEnumerable<UserNotification> UserNotification { get; set; }
    }
}
