using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class ExtraordinaryEvaluation : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string TeacherId { get; set; }
        public Guid TermId { get; set; }

        public string Resolution { get; set; }
        public string ResolutionFile { get; set; }
        public byte Type { get; set; } = ConstantHelpers.Intranet.ExtraordinaryEvaluation.EXTRAORDINARY;

        public Course Course { get; set; }
        public Teacher Teacher { get; set; }
        public Term Term { get; set; }

        public ICollection<ExtraordinaryEvaluationStudent> ExtraordinaryEvaluationStudents { get; set; }
        public ICollection<ExtraordinaryEvaluationCommittee> ExtraordinaryEvaluationCommittees { get; set; }
    }
}
