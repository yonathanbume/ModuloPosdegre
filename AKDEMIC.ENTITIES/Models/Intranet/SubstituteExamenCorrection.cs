using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class SubstituteExamCorrection : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid SubstituteExamId { get; set; }

        public string TeacherId { get; set; }

        public decimal NewGrade { get; set; }

        public decimal OldGrade { get; set; }

        public int State { get; set; } = 1; // 1: Pending 2: Approbed 3: Declined

        public string CreatorIP { get; set; }

        public string Observations { get; set; }
        public string FilePath { get; set; }
        public bool ToPay { get; set; }
        public ApplicationUser Teacher { get; set; }
        public SubstituteExam SubstituteExam { get; set; }
    }
}
