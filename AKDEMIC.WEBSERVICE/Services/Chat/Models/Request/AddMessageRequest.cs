using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.IO;

namespace AKDEMIC.WEBSERVICE.Services.Chat.Models.Request
{
    public class AddMessageRequest
    {
        public string UserId { get; set; }
        public string Text { get; set; }
        public Guid ChatId { get; set; }
        public DateTime DateTime { get; set; }
        public IFormFile File { get; set; }
        public bool IsImage { get; set; }
        public string FileName { get; set; }
        public Result result { get; set; }
    }

    public class Result
    {
        public Guid? Id { get; set; }
        public string text { get; set; }
    }
}
