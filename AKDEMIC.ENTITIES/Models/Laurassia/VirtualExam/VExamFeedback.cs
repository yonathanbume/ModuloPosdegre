using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class VExamFeedback : Entity, ITimestamp
    {
        public Guid? Id { get; set; }
        public Guid VExamId { get; set; }
        public decimal? End { get; set; }
        public decimal? Start { get; set; }
        public string Text { get; set; }
        public virtual VExam VExam { get; set; }
    }
}