using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class QuestionRubricDetail : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid QuestionRubricId { get; set; }
        public QuestionRubric QuestionRubric { get; set; }
        public decimal Score { get; set; }
        public ICollection<QuestionRubricStudent> QuestionRubricStudents { get; set; }
    }
}
