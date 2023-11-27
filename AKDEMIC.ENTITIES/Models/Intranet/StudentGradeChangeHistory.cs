using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class StudentGradeChangeHistory : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public string IpAddress { get; set; }
        public string Description { get; set; }
        public Guid AcademicHistoryId { get; set; }
        public AcademicHistory AcademicHistory { get; set; }
        public string ResolutionFile { get; set; }
    }
}
