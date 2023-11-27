using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class Element : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid VExamDetailId { get; set; }
        public decimal Index { get; set; }
        public string Option { get; set; }
        public virtual VExamDetail VExamDetail { get; set; }
    }
}
