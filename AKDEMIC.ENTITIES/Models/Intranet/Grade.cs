using System;
using System.Collections;
using System.Collections.Generic;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class Grade : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? EvaluationId { get; set; }
        public Guid StudentSectionId { get; set; }

        public bool Attended { get; set; } = true;
        public string CreatorIP { get; set; }
        public decimal Value { get; set; } = 0;

        public virtual Enrollment.Evaluation Evaluation { get; set; }
        public StudentSection StudentSection { get; set; }

        public ICollection<GradeCorrection> GradeCorrections { get; set; }
    }
}
