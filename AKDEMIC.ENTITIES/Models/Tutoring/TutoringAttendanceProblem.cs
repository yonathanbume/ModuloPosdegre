using System;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class TutoringAttendanceProblem
    {
        public Guid Id { get; set; }

        public Guid TutoringAttendanceId { get; set; }

        public TutoringAttendance TutoringAttendance { get; set; }

        public Guid TutoringProblemId { get; set; }

        public TutoringProblem TutoringProblem { get; set; }
    }
}
