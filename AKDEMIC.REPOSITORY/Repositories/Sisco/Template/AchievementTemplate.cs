using Microsoft.AspNetCore.Http;
using System;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Template
{
    public class AchievementTemplate
    {
        public Guid Id { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string PublicationDate { get; set; }
        public string Status { get; set; }
        public string UrlImage { get; set; }
        public IFormFile Image { get; set; }
        public bool StatusId { get; set; }
    }
}
