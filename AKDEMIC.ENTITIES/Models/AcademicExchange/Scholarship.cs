using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.AcademicExchange
{
    public class Scholarship :Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte Program { get; set; }
        public string Requirements { get; set; }
        public Guid TermId { get; set; }
        public Term Term { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BannerUrl { get; set; }
        public byte Target { get; set; }
        public Guid? CareerId { get; set; }
        public Career Career { get; set; }
        public IEnumerable<ScholarshipFile> ScholarshipFiles { get; set; }
        public IEnumerable<Gallery> Galleries { get; set; }
        public IEnumerable<Postulation> Postulations { get; set; }

        [NotMapped]
        public string CareerName { get; set; }
        [NotMapped]
        public string FacultyName { get; set; }

        [NotMapped]
        public string ParsedEndDate => EndDate.ToLocalDateFormat();
        [NotMapped]
        public string ParsedStartDate => StartDate.ToLocalDateFormat();

        [NotMapped]
        public byte Status => (byte)(StartDate > DateTime.UtcNow ? 2 : (EndDate > DateTime.UtcNow ? ((EndDate - DateTime.UtcNow).TotalDays <= 15 ? 3 : 1) : 4));
    }
}
