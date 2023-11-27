using System;

namespace AKDEMIC.ENTITIES.Models.Sisco
{
    public class Achievement
    {
        public Guid Id { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string UrlImage { get; set; }
        public DateTime PublicationDate { get; set; }
        public byte Status { get; set; }
    }
}
