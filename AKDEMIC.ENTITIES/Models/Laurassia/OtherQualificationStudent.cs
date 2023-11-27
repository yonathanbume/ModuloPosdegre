using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class OtherQualificationStudent : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid StudentSectionId { get; set; }
        public decimal Score { get; set; }
        public Guid OtherQualificationId { get; set; }
        public OtherQualification OtherQualification { get; set; }
        public StudentSection StudentSection { get; set; }
    }
}
