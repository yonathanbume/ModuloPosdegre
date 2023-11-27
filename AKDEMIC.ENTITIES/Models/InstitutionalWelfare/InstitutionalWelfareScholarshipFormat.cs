using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class InstitutionalWelfareScholarshipFormat
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public Guid InstitutionalWelfareScholarshipId { get; set; }
        public InstitutionalWelfareScholarship InstitutionalWelfareScholarship { get; set; }
    }
}
