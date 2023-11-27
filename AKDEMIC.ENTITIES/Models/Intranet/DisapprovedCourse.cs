using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class DisapprovedCourse
    {
        public Guid StudentId { get; set; }

        public Guid CourseId { get; set; }

        public Student Student { get; set; }

        public Course Course { get; set; }

        public byte LastTry { get; set; }

        public Guid TermId { get; set; }

        public Term Term { get; set; }
    }
}
