using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class EnrollmentTurnHistory : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid EnrollmentTurnId { get; set; }

        public DateTime? ConfirmationDate { get; set; }
        public decimal CreditsLimit { get; set; }
        public string FileUrl { get; set; }

        //old values
        public bool IsConfirmed { get; set; }
        public bool IsReceived { get; set; }
        public bool IsRectificationActive { get; set; }
        public string Observations { get; set; }
        public bool SpecialEnrollment { get; set; }
        
        public EnrollmentTurn EnrollmentTurn { get; set; }
    }
}
