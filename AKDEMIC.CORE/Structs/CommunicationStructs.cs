using System;
using System.Collections.Generic;

namespace AKDEMIC.CORE.Structs
{
    public class CommunicationStructs
    {
        public struct NotificationRequestParameters
        {
            public string UserId { get; set; }
            public int? Page { get; set; }
            public int? Records { get; set; }
        }

        public struct NotificationResponseParameters
        {
            public int Unread { get; set; }
            public int Read { get; set; }
            public IEnumerable<NotificationResult> NotificationResults { get; set; }
        }

        public struct NotificationResult
        {
            public Guid Id { get; set; }
            public string EllapsedTimeText { get; set; }
            public bool IsRead { get; set; }
            public DateTime? ReadDate { get; set; }
            public DateTime SendDate { get; set; }
            public string StateColor { get; set; }
            public string StateText { get; set; }
            public string Text { get; set; }
            public string Url { get; set; }
        }

        public struct NotificationHub
        {
            public string EllapsedTimeText { get; set; }
            public bool Sound { get; set; }
            public string StateColor { get; set; }
            public string StateText { get; set; }
            public string Text { get; set; }
            public string Url { get; set; }
        }
    }
}
