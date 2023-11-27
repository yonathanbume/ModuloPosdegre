using System;

namespace AKDEMIC.WEBSERVICE.Services.Chat.Models.Result
{
    public class AddMessageResult
    {
        public string status { get; set; }
        public string message { get; set; }
        public Guid? result { get; set; }
    }
}
