using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.InternationalCooperationManagement
{
    public class ExchangeAgreement
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public byte Type { get; set; }
        public string UrlFile { get; set; }

        public int Status { get; set; }
        public int ResolutionNumber { get; set; }

        public DateTime RegistrationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ResolutionDate { get; set; }

        public Guid? OldExchangeAgreementId { get; set; }
        public ExchangeAgreement OldExchangeAgreement { get; set; }

        public Guid CareerId { get; set; }
        public Career Career { get; set; }
        public Guid FacultyId { get; set; }
        public Faculty Faculty { get; set; }
    }
}
