using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class AdmissionExamClassroomTeacher
    {
        public Guid Id { get; set; }
        public Guid AdmissionExamId { get; set; }
        public Guid? AdmissionExamClassroomId { get; set; }
        public string UserId { get; set; }
        
        public bool Assisted { get; set; } = false;
        
        public AdmissionExam AdmissionExam { get; set; }
        public AdmissionExamClassroom AdmissionExamClassroom { get; set; }
        public ApplicationUser User { get; set; }
    }
}
