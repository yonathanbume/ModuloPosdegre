using System;

namespace AKDEMIC.WEBSERVICE.Services.Chat.Models.Result
{
    public class AddChatResult
    {
        public string status { get; set; }
        public string message { get; set; }
        public Result result { get; set; }
       
    }

    public class Result {
        public Guid? Id { get; set; }
        public string text { get; set; }
    }
}
