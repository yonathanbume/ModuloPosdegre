using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class StudentFeedbackActivity : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public StudentActivity StudentActivity { get; set; }
        public Guid StudentActivityId { get; set; }
        public DateTime DateTime { get; set; }
        public string Comment { get; set; }
        public List<StudentFeedbackActivityFile> StudentFeedbackActivityFiles { get; set; }
    }
}
