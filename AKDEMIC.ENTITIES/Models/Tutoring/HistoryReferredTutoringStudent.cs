using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class HistoryReferredTutoringStudent : ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? TutoringSessionStudentId { get; set; }
        public TutoringSessionStudent TutoringSessionStudent { get; set; }   
        public Guid? TutoringAttendanceId { get; set; }
        public TutoringAttendance TutoringAttendance{ get; set; }
        public string NameAttend { get; set; }
        public string Rol { get; set; }
        public DateTime? SendTime { get; set; }
        public string SupportOfficeName { get; set; }
        public string Observation { get; set; }
        public string Problems { get; set; }

    }
}
