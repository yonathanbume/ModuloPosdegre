using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.PreprofesionalPractice
{
    public class InternshipRequestFile : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FileUrl { get; set; }
        public byte Type { get; set; }
        public Guid InternshipRequestId { get; set; }
        public InternshipRequest InternshipRequest { get; set; }
    }
}
