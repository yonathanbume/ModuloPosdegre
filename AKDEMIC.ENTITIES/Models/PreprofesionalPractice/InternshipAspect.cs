using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.PreprofesionalPractice
{
    public class InternshipAspect : Entity, ITimestamp, ISoftDelete
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte Type { get; set; }
    }
}

