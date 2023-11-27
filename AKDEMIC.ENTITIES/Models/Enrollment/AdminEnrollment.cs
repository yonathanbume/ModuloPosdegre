using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class AdminEnrollment : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid TermId { get; set; }
        public string UserId { get; set; }

        public string ActivityLog { get; set; }
        public string Filename { get; set; }
        public string FileUrl { get; set; }
        
        public bool IsRectification { get; set; }
        public bool WasApplied { get; set; }

        public string Observations { get; set; }

        public ApplicationUser User { get; set; }
        public Student Student { get; set; }
        public Term Term { get; set; }
    }
}
