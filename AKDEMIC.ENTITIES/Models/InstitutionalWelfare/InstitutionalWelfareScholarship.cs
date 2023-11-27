using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class InstitutionalWelfareScholarship : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ApplicationStartDate { get; set; }
        public DateTime ApplicationEndDate { get; set; }
        public Guid InstitutionalWelfareScholarshipTypeId { get; set; }
        public InstitutionalWelfareScholarshipType InstitutionalWelfareScholarshipType { get; set; }
        public ICollection<InstitutionalWelfareScholarshipRequirement> Requirements { get; set; }
        public ICollection<InstitutionalWelfareScholarshipFormat> Formats { get; set; }
        public ICollection<ScholarshipStudent> ScholarshipStudents { get; set; }
    }
}
