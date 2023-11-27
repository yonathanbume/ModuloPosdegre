using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class Tutor
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Guid CareerId { get; set; }

        public Career Career { get; set; }
        public byte Type { get; set; }

        public ICollection<TutorTutoringStudent> TutorTutoringStudents { get; set; }
        public ICollection<TutoringSession> TutoringSessions { get; set; }
        public ICollection<TutoringAttendance> TutoringAttendances { get; set; }
        public ICollection<TutorWorkingPlan> TutorWorkingPlans { get; set; }

        public int TimesUpdated { get; set; } = 0;
    }
}
