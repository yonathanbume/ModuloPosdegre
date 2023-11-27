using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class TutoringAttendance : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? SupportOfficeId { get; set; }
        public string SupportOfficeUserId { get; set; }
        public Guid TermId { get; set; }
        public string TutorId { get; set; }
        public Guid TutoringStudentStudentId { get; set; }
        public Guid TutoringStudentTermId { get; set; }

        public string Observation { get; set; }

        public SupportOffice SupportOffice { get; set; }
        public SupportOfficeUser SupportOfficeUser { get; set; }
        public Term Term { get; set; }
        public Tutor Tutor { get; set; }
        public TutoringStudent TutoringStudent { get; set; }

        public ICollection<HistoryReferredTutoringStudent> HistoryReferredTutoringStudents { get; set; }
        public ICollection<TutoringAttendanceProblem> TutoringAttendanceProblems { get; set; }
    }
}
