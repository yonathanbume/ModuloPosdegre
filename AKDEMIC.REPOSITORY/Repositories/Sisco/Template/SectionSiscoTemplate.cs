using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Template
{
    public class SectionSiscoTemplate
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PublicationDate { get; set; }
        public string Status { get; set; }
        public string UrlImage { get; set; }
        public string SequenceOrder { get; set; }
        public string UrlDirection { get; set; }
        public string StatusDirection { get; set; }
        public string NameDirection { get; set; }
        public bool StatusId { get; set; }
        public bool StatusDirectionId { get; set; }
        public int SequenceOrderId { get; set; }
        public SelectList ListSequenceOrder { set; get; }
    }
}
