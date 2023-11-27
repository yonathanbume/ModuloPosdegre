using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.AcademicExchange
{
    public class AcademicAgreement : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PersonInCharge { get; set; }
        public string ResolutionNumber { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public byte FileType { get; set; }
        public string UrlFile { get; set; }
        public bool HasBeenRenovated { get; set; } = false;

        public Guid? CareerId { get; set; }
        public Career Career { get; set; }
        public Guid? FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public DateTime ResolutionDate { get; set; }

        public Guid? OldAcademicAgreementId { get; set; }
        public AcademicAgreement OldAcademicAgreement { get; set; }
        public ICollection<AcademicAgreementType> AcademicAgreementTypes { get; set; }

        [NotMapped]
        public string Types { get; set; }
        [NotMapped]
        public string FormattedRegistrationDate => CreatedAt.ToLocalDateFormat();
        [NotMapped]
        public string FormattedStartDate => StartDate.ToLocalDateFormat();
        [NotMapped]
        public string FormattedEndDate => EndDate.HasValue ? EndDate.ToLocalDateFormat() : "Indefinida";
        [NotMapped]
        public string FormattedResolutionDate => ResolutionDate.ToLocalDateFormat();
        [NotMapped]
        public byte Status => (byte)(!EndDate.HasValue ? 1 : (StartDate > DateTime.UtcNow ? 2 : (EndDate > DateTime.UtcNow ? ((EndDate.Value - DateTime.UtcNow).TotalDays <= 15 ? 3 : 1) : 4)));
        [NotMapped]
        public bool IsEndingSoon => EndDate.HasValue && (EndDate.Value - DateTime.UtcNow).TotalDays <= 7;

        public Guid AdmissionTypeId { get; set; }
        public AdmissionType AdmissionType { get; set; }
    }
}
