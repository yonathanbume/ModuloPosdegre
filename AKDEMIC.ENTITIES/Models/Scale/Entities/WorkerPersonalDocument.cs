using System;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class WorkerPersonalDocument : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        public string IdentityPhoto { get; set; }
        public string JobApplicationDocument { get; set; }
        public string HomeCertificate { get; set; }
        public string PresentationLetter { get; set; }
        public string AffidavitOfAssetsAndIncome { get; set; }
        public string HealthCertificate { get; set; }
        public string JudicialRecordCertificate { get; set; }
        public string PoliceRecordCertificate { get; set; }
        public string BirthCertificate { get; set; }
        public string LegalizedBachelorDegree { get; set; }
        public string NationalityByMarriageCertificate { get; set; }
        public string VisaOfResidence { get; set; }
        public string DisabilityResolution { get; set; }
        public string EssaludCertificate { get; set; }
        public string ArmyMemberCertificate { get; set; }
        public string Resume { get; set; }
        public string IdentityDocument { get; set; }
        public string MarriageCertificate { get; set; }
        public string CohabitationCertificate { get; set; }
        public string ChildrenIdentityDocument { get; set; }
        public string PartnerIdentityDocument { get; set; }
        public string ScaleResume { get; set; }
        public string RelatedFamilyMemberDocument { get; set; }
        public string ChildrenBirthCertificate { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
