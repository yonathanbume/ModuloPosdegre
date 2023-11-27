using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class QualificationLog : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public DateTime DateTime { get; set; }
        public Guid VExamDetailId { get; set; }
        public VExamDetail VExamDetail { get; set; }
        public decimal Value { get; set; }
    }
}
