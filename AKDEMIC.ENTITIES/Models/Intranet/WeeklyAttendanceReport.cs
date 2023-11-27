using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class WeeklyAttendanceReport
    {
        [Key]
        public Guid SectionId { get; set; }
        [Key]
        public int Week { get; set; }
        public int Attendances { get; set; }
        public int Absences { get; set; }
        public decimal AverageAttendances { get; set; }
        public decimal AverageAbsences { get; set; }
        public decimal AttendancePercentage { get; set; }
        public Section Section { get; set; }
    }
}
