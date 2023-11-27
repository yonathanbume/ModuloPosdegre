using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class Recognition : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid ResolutionId { get; set; }
        public string Comment { get; set; }

        public Student Student { get; set; }
        public Resolution Resolution { get; set; }
        public ICollection<CourseRecognition> CourseRecognitions { get; set; }
        public ICollection<RecognitionHistory> RecognitionHistories { get; set; }
    }
}
