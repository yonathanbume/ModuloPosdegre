using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class DisapprovedCourseConcept
    {
        public Guid Id { get; set; }

        public byte Try { get; set; }

        public bool IsCostPerCredit { get; set; }

        public Guid ConceptId { get; set; }
        public Concept Concept { get; set; }

        public Guid? AdmissionTypeId { get; set; }
        public AdmissionType AdmissionType { get; set; }
    }
}
