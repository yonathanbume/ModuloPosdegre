using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class TutoringSessionStudent
    {
        public Guid Id { get; set; }
        public Guid? SupportOfficeId { get; set; }
        public string SupportOfficeUserId { get; set; }
        public Guid TutoringSessionId { get; set; }
        public Guid TutoringStudentStudentId { get; set; }
        public Guid TutoringStudentTermId { get; set; }
        
        public bool Absent { get; set; } = false;
        public bool Attended { get; set; } = false;
        public double Grade { get; set; }
        public string Observation { get; set; }
        public DateTime? SendTime { get; set; }
        
        public SupportOffice SupportOffice { get; set; }
        public SupportOfficeUser SupportOfficeUser { get; set; }
        public TutoringSession TutoringSession { get; set; }
        public TutoringStudent TutoringStudent { get; set; }
        
        public ICollection<HistoryReferredTutoringStudent> HistoryReferredTutoringStudents { get; set; }
    }
}
