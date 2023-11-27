using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class TutoringMessage : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public TutoringStudent TutoringStudent { get; set; }

        public Tutor Tutor { get; set; }

        public string TutorId { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public Guid TutoringStudentTermId { get; set; }
        public Guid TutoringStudentStudentId { get; set; }
    }
}
