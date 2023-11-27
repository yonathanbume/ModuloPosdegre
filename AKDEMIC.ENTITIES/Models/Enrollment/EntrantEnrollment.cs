using System;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class EntrantEnrollment
    {
        public Guid Id { get; set; }

        public bool Finished { get; set; } = false;

        public Guid CareerId { get; set; }

        public Guid TermId { get; set; }

        public Term Term { get; set; }

        public Career Career { get; set; }
    }
}
