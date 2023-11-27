using System;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class GradeCorrection : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public bool RequestedByStudent { get; set; }
        public Guid GradeId { get; set; }
        public string TeacherId { get; set; }
        public decimal? NewGrade { get; set; }
        public decimal? OldGrade { get; set; }
        public int State { get; set; } = 1; // 1: Pending 2: Approbed 3: Declined
        public string CreatorIP { get; set; }
        public string Observations { get; set; }
        public bool NotTaken { get; set; }
        public string FilePath { get; set; }
        public bool ToPay { get; set; }
        public ApplicationUser Teacher { get; set; }
        public Grade Grade { get; set; }
    }
}
