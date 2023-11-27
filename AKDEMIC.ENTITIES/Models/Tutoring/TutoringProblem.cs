using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class TutoringProblem
    {
        public Guid Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Description { get; set; }
        
        public byte Category { get; set; }

        public ICollection<TutoringSessionProblem> TutoringSessionProblems { get; set; }

        public ICollection<TutoringProblemFile> TutoringProblemFiles { get; set; }

        public ICollection<TutoringAttendanceProblem> TutoringAttendanceProblems { get; set; }
    }
}
