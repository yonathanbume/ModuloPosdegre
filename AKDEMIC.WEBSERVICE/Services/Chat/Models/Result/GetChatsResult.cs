using System;
using System.Collections.Generic;

namespace AKDEMIC.WEBSERVICE.Services.Chat.Models.Result
{
    public class GetChatsResult
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<ChatResult> result { get; set; }
    }

    public class ChatResult
    {
        public Guid id { get; set; }
        public int messages { get; set; }
        public string userId { get; set; }
        public DateTime dateTime { get; set; }
        public string fullName { get; set; }
        public bool isImage { get; set; }
        public string fileName { get; set; }
        public string lastMsg { get; set; }
    }
}
