using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class UniversityAuthority:Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User{ get; set; }

        public byte Type { get; set; }

        public List<UniversityAuthorityHistory> UniversityAuthorityHistories { get; set; }
    }
}
