using System;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class AdmissionExamPostulantGrade
    {
        public Guid Id { get; set; }
        public Guid AdmissionExamId { get; set; }
        public AdmissionExam AdmissionExam { get; set; }
        public Guid PostulantId { get; set; }
        public Postulant Postulant { get; set; }
        public decimal Grade { get; set; }
    }
}
