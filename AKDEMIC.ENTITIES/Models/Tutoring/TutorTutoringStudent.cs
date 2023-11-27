using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class TutorTutoringStudent
    {
        public Guid TutoringStudentTermId { get; set; }
        public Guid TutoringStudentStudentId { get; set; }

        [Required]
        public string TutorId { get; set; }
        public Tutor Tutor { get; set; }
        public TutoringStudent TutoringStudent { get; set; }
    }
}
