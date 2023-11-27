using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class EnrollmentConcept : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public byte? Condition { get; set; }

        public byte Type { get; set; }

        public Guid ConceptId { get; set; }
        public Concept Concept { get; set; }

        public Guid? CareerId { get; set; }
        public Career Career { get; set; }

        public Guid? AdmissionTypeId { get; set; }
        public AdmissionType AdmissionType { get; set; }

        public Guid? StudentConditionId { get; set; }
        public StudentCondition StudentCondition { get; set; }

        public Guid? ConceptToReplaceId { get; set; }
        public Concept ConceptToReplace { get; set; }
    }
}
