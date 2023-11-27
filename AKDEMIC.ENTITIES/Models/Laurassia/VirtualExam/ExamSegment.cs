using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class ExamSegment : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public VExam VExam { get; set; }
        public string Name { get; set; }
        public Guid VExamId { get; set; }
        public List<VQuestion> VQuestions { get; set; }
    }
}
