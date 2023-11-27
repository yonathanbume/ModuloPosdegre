using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class ExtracurricularArea : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte Type { get; set; } = ConstantHelpers.ExtracurricularArea.Type.COURSE;
    }
}
