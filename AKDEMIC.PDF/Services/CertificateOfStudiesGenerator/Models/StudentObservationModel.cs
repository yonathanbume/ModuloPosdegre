using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.PDF.Services.CertificateOfStudiesGenerator.Models
{
    public class StudentObservationModel
    {
        public Guid Id { get; set; }
        public byte Type { get; set; }
        public string Term { get; set; }
        public string Observation { get; set; }
    }
}
