using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class VExamStudent : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string StudentId { get; set; }
        public Guid VExamId { get; set; }
        public string State { get; set; }
        public virtual ApplicationUser Student { get; set; }
        public virtual VExam VExam { get; set; }
        public ICollection<ExamResolution> ExamResolutions { get; set; }
    }
}