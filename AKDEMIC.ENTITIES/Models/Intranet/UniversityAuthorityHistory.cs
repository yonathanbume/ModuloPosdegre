using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class UniversityAuthorityHistory:Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User{ get; set; }

        public byte Type { get; set; }
        public Guid UniversityAuthorityId { get; set; }
        public string Resolution { get; set; }
        public DateTime ResolutionDate { get; set; }
        public string FileUrl { get; set; }
        public UniversityAuthority UniversityAuthority { get; set; }
    }
}
