using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class VocationalTestAnswerCareer
    {
        public Guid Id { get; set; }
        public Guid VocationalTestAnswerId { get; set; }
        public Guid CareerId { get; set; }
        public VocationalTestAnswer VocationalTestAnswer { get; set; }
        public decimal Score { get; set; }
        public Career Career { get; set; }    
        
        public ICollection<VocationalTestAnswerCareerPostulant> Postulants { get; set; }
    }
}
