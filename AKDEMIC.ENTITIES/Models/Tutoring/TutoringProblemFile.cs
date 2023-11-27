using System;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class TutoringProblemFile
    {
        public Guid Id { get; set; }

        public Guid TutoringProblemId { get; set; }

        public TutoringProblem TutoringProblem { get; set; }

        public bool IsUrl { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }
    }
}
