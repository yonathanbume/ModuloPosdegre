using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Template
{
    public class NormTemplate
    {
        public Guid Id { get; set; }
        public string StandardNumber { get; set; }
        public string Sumilla { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int? TypeId { get; set; }
        public int? StatusId { get; set; }
        public string PublicationDate { get; set; }
        public string TransmissionDate { get; set; }
        public string UrlPdf { get; set; }
        public string UrlWord { get; set; }
        public SelectList ListType { set; get; }
        public SelectList ListStatus { set; get; }
    }
}
