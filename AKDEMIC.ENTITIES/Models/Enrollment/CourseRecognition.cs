using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CourseRecognition : Entity, ITimestamp, ISoftDelete
    {
        public Guid CourseId { get; set; }
        public Guid RecognitionId { get; set; }

        public Course Course { get; set; }
        public Recognition Recognition{get;set;}
    }
}
