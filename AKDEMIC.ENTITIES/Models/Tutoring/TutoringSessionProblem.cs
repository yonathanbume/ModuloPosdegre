using System;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class TutoringSessionProblem
    {
        public Guid Id { get; set; }

        public Guid TutoringSessionId { get; set; }

        public TutoringSession TutoringSession { get; set; }

        public Guid TutoringProblemId { get; set; }

        public TutoringProblem TutoringProblem { get; set; }
    }
}
