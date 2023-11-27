using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class DirectedCourse : Entity, ITimestamp
    {
        public Guid Id { get; set; }      
        public string TeacherId { get; set; }
        public Guid CourseId { get; set; }
        public Guid CareerId { get; set; }
        public Guid TermId { get; set; }
        public string Resolution { get; set; }
        public string File { get; set; }

        public Teacher Teacher { get; set; }
        public Course Course { get; set; }
        public Career Career { get; set; }
        public Term Term { get; set; }

        public ICollection<DirectedCourseStudent> Students { get; set; }
    }
}
