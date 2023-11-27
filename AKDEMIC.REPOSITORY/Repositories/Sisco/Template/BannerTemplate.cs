using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Template
{
    public class BannerTemplate
    {
        public Guid Id { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string PublicationDate { get; set; }
        public string Status { get; set; }
        public string UrlImage { get; set; }
        public string SequenceOrder { get; set; }
        public string UrlDirection { get; set; }
        public string StatusDirection { get; set; }
        public string NameDirection { get; set; }
        public IFormFile Image { get; set; }
        public bool StatusId { get; set; }
        public bool StatusDirectionId { get; set; }
        public int SequenceOrderId { get; set; }
        public SelectList ListSequenceOrder { set; get; }
    }
}
