using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;


namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class ExternalLink : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string ExternalUrl { get; set; }
        public string ImageUrl { get; set; }
        public int Type { get; set; } = CORE.Helpers.ConstantHelpers.EXTERNAL_LINK.TYPE.NOTDEFINED;
    }
}
