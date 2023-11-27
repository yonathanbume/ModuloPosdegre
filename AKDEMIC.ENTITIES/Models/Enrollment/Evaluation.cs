using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class Evaluation : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid CourseTermId { get; set; }
        public Guid? CourseUnitId { get; set; }
        public Guid? EvaluationTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Percentage { get; set; }
        public bool Retrievable { get; set; }
        public int? Week { get; set; }


        [NotMapped]
        public bool Taken { get; set; }

        public CourseTerm CourseTerm { get; set; }
        public CourseUnit CourseUnit { get; set; }
        public EvaluationType EvaluationType { get; set; }
        public ICollection<Grade> Grades { get; set; }
        public ICollection<GradeRegistration> GradeRegistrations { get; set; }
    }
}
