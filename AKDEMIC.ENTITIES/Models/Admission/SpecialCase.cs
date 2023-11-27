using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class SpecialCase
    {
        public Guid Id { get; set; }
        public Guid CareerId { get; set; }
        public Guid? PublicConceptId { get; set; }
        public Guid? PrivateConceptId { get; set; }
        public Guid? ForeignConceptId { get; set; }
        public Guid AdmissionTypeId { get; set; }
        public bool IsExonerated { get; set; }

        public AdmissionType AdmissionType { get; set; }
        public Concept PublicConcept { get; set; }
        public Concept PrivateConcept { get; set; }
        public Concept ForeignConcept { get; set; }
        public Career Career { get; set; }
    }
}
