using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class InternalOutput : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid DependencyId { get; set; }
        public string ToUserId { get; set; }
        public Dependency Dependency { get; set; }
        public ApplicationUser ToUser { get; set; }
        public ICollection<InternalOutputItem> InternalOutputItems { get; set; }
    }
}
