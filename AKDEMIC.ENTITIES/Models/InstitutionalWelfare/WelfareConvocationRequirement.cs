using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class WelfareConvocationRequirement : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid WelfareConvocationId { get; set; }
        public WelfareConvocation WelfareConvocation { get; set; }

        public ICollection<WelfareConvocationPostulantFile> PostulationFiles { get; set; }
    }
}
