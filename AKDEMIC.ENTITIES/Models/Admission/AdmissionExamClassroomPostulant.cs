using System;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class AdmissionExamClassroomPostulant
    {
        public Guid Id { get; set; }

        public Guid PostulantId { get; set; }

        public Guid AdmissionExamClassroomId { get; set; }

        public int Seat { get; set; }

        public bool Attended { get; set; }

        public AdmissionExamClassroom AdmissionExamClassroom { get; set; }
       
        public Postulant Postulant { get; set; }
    }
}
