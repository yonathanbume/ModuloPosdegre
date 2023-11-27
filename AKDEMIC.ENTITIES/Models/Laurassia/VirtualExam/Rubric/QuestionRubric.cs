using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class QuestionRubric : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid VQuestionId { get; set; }
        public VQuestion VQuestion { get; set; }
        public string Description { get; set; }
        public decimal Score { get; set; }
        public ICollection<QuestionRubricDetail> QuestionRubricDetails { get; set; }
        public ICollection<QuestionRubricStudent> QuestionRubricStudents { get; set; }
    }
}
