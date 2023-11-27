using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class TeacherAcademicCharge
    {
        public Guid Id { get; set; }
        public string TeacherId { get; set; }
        public Guid TermId { get; set; }

        public bool IsValidated { get; set; }
        public string Observation { get; set; }
        public DateTime ValidationDateTime { get; set; }

        public Teacher Teacher { get; set; }
        public Term Term { get; set; }
    }
}
