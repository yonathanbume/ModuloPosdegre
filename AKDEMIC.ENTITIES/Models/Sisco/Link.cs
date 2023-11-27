using System;

namespace AKDEMIC.ENTITIES.Models.Sisco
{
    public class Link
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string UrlImage { get; set; }
        public string UrlDirection { get; set; }
        public DateTime PublicationDate { get; set; }
        public byte Status { get; set; }
        public string Type { get; set; }
    }
}
