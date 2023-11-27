using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class CurriculumVitae : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        public Student Student { get; set; }

        public Guid StudentId { get; set; }

        public byte Type { get; set; }

        public string File { get; set; }

        public string Description { get; set; }

        public string Linkedin { get; set; }

        public string DriverLicenseCode { get; set; }
        public string DriverLicenseCategory { get; set; }
        public int TiutionStatus { get; set; }
        public string TiutionNumber { get; set; }
        public string DisabilityCertificatePath { get; set; }
    }
}
