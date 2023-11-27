using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates
{
    public class CurriculumVitaeTemplate
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string Name { get; set; }
        public string PaternalSurName { get; set; }
        public string MaternalSurName { get; set; }
        public string Birthday { get; set; }
        public string Career { get; set; }
        public string Dni { get; set; }
        public string File { get; set; }
        public string Description { get; set; }
        public string Linkedin { get; set; }

        public string Sex { get; set; }
        public string Department { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string CivilStatus { get; set; }
        public string PhoneNumber { get; set; }
        public string DriverLicenseCode { get; set; }
        public string DriverLicenseCategory { get; set; }
        public int TiutionStatus { get; set; }
        public string TiutionNumber { get; set; }
        public string DisabilityCertificatePath { get; set; }
    }
}
