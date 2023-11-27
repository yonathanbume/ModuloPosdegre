using AKDEMIC.ENTITIES.Models.Enrollment;
using System;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class AdmissionTypeDescount
    {
        public Guid Id { get; set; }
        public Guid AdmissionTypeId { get; set; }
        public Guid TermId { get; set; }

        public float Descount { get; set; } = 0.00f;

        public AdmissionType AdmissionType { get; set; }
        public Term Term { get; set; }
    }
}
