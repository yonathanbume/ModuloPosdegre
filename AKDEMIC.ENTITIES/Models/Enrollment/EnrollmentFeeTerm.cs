using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class EnrollmentFeeTerm : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid StudentScaleId { get; set; }

        public Guid TermId { get; set; }

        public Guid CampusId { get; set; }

        public Guid CareerId { get; set; }

        public Guid ConceptId { get; set; }

        public decimal Total { get; set; }

        public StudentScale StudentScale { get; set; }

        public Term Term { get; set; }

        public Campus Campus { get; set; }

        public Career Career { get; set; }

        public Concept Concept { get; set; }
    }
}
