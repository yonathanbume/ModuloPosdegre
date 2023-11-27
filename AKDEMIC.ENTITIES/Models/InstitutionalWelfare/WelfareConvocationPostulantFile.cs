using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class WelfareConvocationPostulantFile
    {
        public Guid Id { get; set; }

        public Guid WelfareConvocationPostulantId { get; set; }
        public WelfareConvocationPostulant WelfareConvocationPostulant { get; set; }

        public Guid WelfareConvocationRequirementId { get; set; }
        public WelfareConvocationRequirement WelfareConvocationRequirement { get; set; }

        public string FileUrl { get; set; }
    }
}
