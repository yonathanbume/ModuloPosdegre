using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class SectionEvaluation : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid EvaluationId { get; set; }
        public Evaluation Evaluation { get; set; }
        public Guid SectionId { get; set; }
        public Section Section { get; set; }
        public int Percentage { get; set; }
    }
}
