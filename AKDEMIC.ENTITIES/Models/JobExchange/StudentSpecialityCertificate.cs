using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class StudentSpecialityCertificate : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Institution { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool HasExpirationDate { get; set; }
        public string SpecialityCertificateFilePath { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
    }
}
