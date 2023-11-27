using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class TutoringStudent
    {
        public Guid StudentId { get; set; }
        public Guid TermId { get; set; }    
        
        public int TimesUpdated { get; set; } = 0;

        public Student Student { get; set; }
        public Term Term { get; set; }

        public ICollection<TutoringAttendance> TutoringAttendances { get; set; }
        public ICollection<TutoringSessionStudent> TutoringSessionStudents { get; set; }
        public ICollection<TutorTutoringStudent> TutorTutoringStudents { get; set; }
    }
}
