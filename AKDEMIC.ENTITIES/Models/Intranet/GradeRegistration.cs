using System;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class GradeRegistration
    {
        public Guid Id { get; set; }
        public Guid EvaluationId { get; set; }
        public Guid SectionId { get; set; }
        public string TeacherId { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
        
        public bool WasLate { get; set; }        
        public bool WasPublished { get; set; }

        public Enrollment.Evaluation Evaluation { get; set; }
        public Section Section { get; set; }
        public Teacher Teacher { get; set; }
    }
}
