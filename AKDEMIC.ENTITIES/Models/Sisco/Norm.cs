using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Sisco
{
    public class Norm
    {
        public Guid Id { get; set; }
        public string StandardNumber { get; set; }
        public string Sumilla { get; set; }
        public byte Type { get; set; }
        public byte Status { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime TransmissionDate { get; set; }
        [StringLength(500)]
        public string UrlPdf { get; set; }

        [StringLength(500)]
        public string UrlWord { get; set; }
    }
}
