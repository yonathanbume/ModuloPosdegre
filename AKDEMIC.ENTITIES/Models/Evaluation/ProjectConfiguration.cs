using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;

namespace AKDEMIC.ENTITIES.Models.Evaluation
{
    public class ProjectConfiguration : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid TermId { get; set; }
        public Term Term { get; set; }
        public DateTime CreateEndDate { get; set; }
        public DateTime FirstReportEndDate { get; set; }
        public DateTime SecondReportEndDate { get; set; }
        public DateTime LastReportEndDate { get; set; }
    }
}
