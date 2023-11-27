using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class TeacherInformation
    {
        public Guid Id { get; set; }

        public int LaborType { get; set; }
        public int LaborRegime { get; set; }
        public int LaborCondition { get; set; }
        public int Category { get; set; }
        public int Dedication { get; set; }
        public Guid? DependencyId { get; set; }
        public Dependency Dependency { get; set; }

        public Guid? CampusId { get; set; } //Sede
        public Campus Campus { get; set; }
        public string Department { get; set; }

        public string SeatCode { get; set; }
        public string WorkerCode { get; set; }
        public DateTime DateAdmission { get; set; }
        public DateTime? DateAppointment { get; set; }
        public string PathAppointment { get; set; }

        public DateTime? DatePromotion { get; set; }
        public string PathPromotion { get; set; }

        //Carga Laboral
        public int WorkloadHours { get; set; }
        public string WorkloadDedication { get; set; }
        public Guid? ResolutionId { get; set; }
        public Resolution Resolution { get; set; }
        public Teacher Teacher { get; set; }
        public virtual ICollection<TeacherExperience> TeacherExperiences { get; set; }
    }
}
