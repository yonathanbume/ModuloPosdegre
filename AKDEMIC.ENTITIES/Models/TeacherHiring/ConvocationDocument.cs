using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeacherHiring
{
    public class ConvocationDocument : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ConvocationId { get; set; }
        public Convocation Convocation { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
