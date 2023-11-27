using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class StudentPortfolio : Entity, ITimestamp
    {
        public Guid StudentId { get; set; }
        //public byte Type { get; set; }
        public string File { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public DateTime? ValidationDate { get; set; }
        public bool IsValidated { get; set; }
        public Student Student { get; set; }
        public Guid StudentPortfolioTypeId { get; set; }
        public StudentPortfolioType StudentPortfolioType { get; set; }
    }
}
