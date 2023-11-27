using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class DirectedCourseStudent : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public decimal Grade { get; set; }
        public byte Status { get; set; }
        public string Resolution { get; set; }
        public string File { get; set; }
        public Guid DirectedCourseId { get; set; }

        public Student Student { get; set; }
        public DirectedCourse DirectedCourse { get; set; }
    }
}
