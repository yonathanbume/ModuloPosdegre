using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class EnrollmentReservation : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid TermId { get; set; }
        public Guid? UserProcedureId { get; set; }

        //public DateTime ApplicationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string FileURL { get; set; }
        public string Receipt { get; set; }
        public string Observation { get; set; }

        public Student Student { get; set; }
        public Term Term { get; set; }
        public UserProcedure UserProcedure { get; set; }
    }
}
