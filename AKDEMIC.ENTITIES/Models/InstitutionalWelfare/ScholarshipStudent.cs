using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class ScholarshipStudent : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid InstitutionalWelfareScholarshipId { get; set; }
        public InstitutionalWelfareScholarship InstitutionalWelfareScholarship { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public byte Status { get; set; } = ConstantHelpers.INSTITUTIONAL_WELFARE_SCHOLARSHIP.STUDENT_STATUS.UNANSWERED;
        public ICollection<ScholarshipStudentRequirement> ScholarshipStudentRequirements { get; set; }
    }
}
