using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CurriculumEquivalence : Entity, ITimestamp
    {
        public Guid NewCurriculumId { get; set; }
        public Guid OldCurriculumId { get; set; }

        public Curriculum NewCurriculum { get; set; }
        public Curriculum OldCurriculum { get; set; }
    }
}
