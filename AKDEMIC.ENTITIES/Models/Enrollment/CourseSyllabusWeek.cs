using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CourseSyllabusWeek
    {
        public Guid Id { get; set; }
        public string PerformanceCriterion { get; set; }
        public string EssentialKnowledge { get; set; }
        public int Week { get; set; }
        public Guid CourseSyllabusId { get; set; }
        public CourseSyllabus CourseSyllabus { get; set; }
    }
}
