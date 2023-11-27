using AKDEMIC.ENTITIES.Models.Degree;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class GradeReportRequirement
    {
        public Guid Id { get; set; }
        public Guid GradeReportId { get; set; }
        public Guid DegreeRequirementId { get; set; }
        public GradeReport GradeReport { get; set; }
        public DegreeRequirement DegreeRequirement { get; set; }
        public string Document { get; set; }
        public string Description { get; set; }
    }
}
