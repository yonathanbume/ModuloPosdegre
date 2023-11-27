using System;
using System.Collections.Generic;

namespace AKDEMIC.WEBSERVICE.Services.Chat.Models.Result
{
    public class GetMessagesResult
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<MessageResult> result { get; set; }
    }

    public class MessageResult
    {
        public DateTime datetime { get; set; }
        public bool read { get; set; }
        public string text { get; set; }
        public Guid id { get; set; }
        public Guid chatId { get; set; }
        public string userId { get; set; }
        public bool sended { get; set; }
        public bool isImage { get; set; }
        public string fileName { get; set; }
    }
}
