using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class AdmissionExamClassroom
    {
        public Guid Id { get; set; }
        public Guid AdmissionExamId { get; set; }
        public Guid ClassroomId { get; set; }
        
        //public DateTime Datetime { get; set; }
        public int Vacancies { get; set; }
        
        public AdmissionExam AdmissionExam { get; set; }
        public Classroom Classroom { get; set; }
        
        public ICollection<AdmissionExamClassroomCareer> Careers { get; set; }
        public ICollection<AdmissionExamClassroomPostulant> Postulants { get; set; }
        public ICollection<AdmissionExamClassroomTeacher> Teachers { get; set; }
    }
}
