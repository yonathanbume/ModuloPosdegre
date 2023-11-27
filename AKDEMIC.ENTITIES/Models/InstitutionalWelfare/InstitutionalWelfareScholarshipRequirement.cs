using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class InstitutionalWelfareScholarshipRequirement
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid InstitutionalWelfareScholarshipId { get; set; }
        public InstitutionalWelfareScholarship InstitutionalWelfareScholarship { get; set; }
        public ICollection<ScholarshipStudentRequirement> ScholarshipStudentRequirements { get; set; }
    }
}
