using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class ApplicationTermAdmissionFile : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public byte Type { get; set; }
        public string Url { get; set; }
        public Guid ApplicationTermId { get; set; }
        public ApplicationTerm ApplicationTerm { get; set; }
    }
}
