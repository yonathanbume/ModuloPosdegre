using Microsoft.AspNetCore.Http;
using System;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Template
{
    public class LinkTemplate
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string PublicationDate { get; set; }
        public string Status { get; set; }
        public bool StatusId { get; set; }
        public string UrlImage { get; set; }
        public string UrlDirection { get; set; }
        public string Type { get; set; }
        public string Other { get; set; }
        public IFormFile Image { get; set; }
        public int TitleId { get; set; }
    }
}
