using Microsoft.AspNetCore.Http;
using System;

namespace AKDEMIC.INTRANET.ViewModels.ForumViewModels
{
    public class PostViewModel
    {
        public Guid TopicId { get; set; }
        public Guid? PostCitedId { get; set; }
        public string Message { get; set; }
        public IFormFile File { get; set; }
    }
}
