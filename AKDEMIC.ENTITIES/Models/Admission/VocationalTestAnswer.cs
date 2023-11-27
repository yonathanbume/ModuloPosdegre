using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class VocationalTestAnswer
    {       
        public Guid Id { get; set; }
        public Guid VocationalTestQuestionId { get; set; }
        public string Description { get; set; }                
        public VocationalTestQuestion VocationalTestQuestion { get; set; }
        public ICollection<VocationalTestAnswerCareer> VocationalTestAnswerCareers { get; set; }

    }
}
