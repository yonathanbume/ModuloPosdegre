using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static AKDEMIC.CORE.Helpers.ConstantHelpers;

namespace AKDEMIC.ENTITIES.Models.Server
{
    public class GeneralLink : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string ImageUrl { get; set; }
        public byte Type { get; set; } = CAMPUS_GENERAL_LINK.TYPE.TOPBAR;

        public ICollection<GeneralLinkRole> GeneralLinkRoles { get; set; }
    }
}
