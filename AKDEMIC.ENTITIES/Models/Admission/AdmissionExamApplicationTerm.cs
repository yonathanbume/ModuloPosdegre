using System;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class AdmissionExamApplicationTerm
    {
        public Guid AdmissionExamId { get; set; }
        public Guid ApplicationTermId { get; set; }
        
        public AdmissionExam AdmissionExam { get; set; }
        public ApplicationTerm ApplicationTerm { get; set; }
    }
}
