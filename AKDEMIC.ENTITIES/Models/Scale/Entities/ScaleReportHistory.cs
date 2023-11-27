using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class ScaleReportHistory: Entity,ITimestamp
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Charge { get; set; }
        public string Name { get; set; }
        public string Destiny { get; set; }
        public string Url { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
