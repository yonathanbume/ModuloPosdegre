using System;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class TutorialStudent
    {
        public Guid Id { get; set; }

        public Guid TutorialId { get; set; }

        public Tutorial Tutorial { get; set; }

        public Guid StudentId { get; set; }

        public Student Student { get; set; }
        public bool Absent { get; set; }
    }
}
