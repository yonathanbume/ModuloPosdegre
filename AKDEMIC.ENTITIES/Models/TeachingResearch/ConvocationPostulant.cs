using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.TeachingResearch
{
    public class ConvocationPostulant : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public byte Status { get; set; } = ConstantHelpers.TEACHING_RESEARCH.POSTULANT_STATUS.PENDING;
        public Guid ConvocationId { get; set; }
        public Convocation Convocation { get; set; }
        public ICollection<ConvocationAnswerByUser> ConvocationAnswerByUsers { get; set; }
    }
}
