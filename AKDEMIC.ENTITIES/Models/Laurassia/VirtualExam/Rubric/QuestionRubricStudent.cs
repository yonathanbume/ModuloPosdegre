using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class QuestionRubricStudent : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public decimal Score { get; set; }
        public string Feedback { get; set; }
        public Guid VExamDetailId { get; set; }
        public Guid? QuestionRubricDetailId { get; set; }
        public VExamDetail VExamDetail { get; set; }
        public QuestionRubricDetail QuestionRubricDetail { get; set; }
        public Guid QuestionRubricId { get; set; }
        public QuestionRubric QuestionRubric { get; set; }
    }
}
