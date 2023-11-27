using System;

namespace AKDEMIC.ENTITIES.Models.Sisco
{
    public class SectionSisco
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public byte Status { get; set; }
        public string UrlImage { get; set; }
        public byte SequenceOrder { get; set; }
        public string UrlDirection { get; set; }
        public byte StatusDirection { get; set; }
        public string NameDirection { get; set; }
    }
}
