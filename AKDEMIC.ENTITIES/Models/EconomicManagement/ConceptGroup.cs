using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class ConceptGroup : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<ConceptGroupDetail> Details { get; set; }
    }
}
