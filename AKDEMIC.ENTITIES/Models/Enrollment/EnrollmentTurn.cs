using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class EnrollmentTurn : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid TermId { get; set; }
        
        public DateTime? ConfirmationDate { get; set; }
        public decimal CreditsLimit { get; set; } = 18;
        //public bool EnableExtemporaneousEnrollment { get; set; }
        //public string ExtemporaneousEnrollmentReceipt { get; set; }
        public string FileUrl { get; set; }
        public bool IsConfirmed { get; set; } = false;
        public bool IsOnline { get; set; } = true;
        public bool IsReceived { get; set; } = false;
        public bool IsRectificationActive { get; set; }
        public string Observations { get; set; }
        public bool SpecialEnrollment { get; set; } = false;
        public DateTime Time { get; set; }
        public bool WasStudentInformationUpdated { get; set; } = false;
        
        public Student Student { get; set; }
        public Term Term { get; set; }

        public ICollection<EnrollmentTurnHistory> EnrollmentTurnHistories { get; set; }
    }
}
