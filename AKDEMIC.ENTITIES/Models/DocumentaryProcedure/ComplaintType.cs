using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class ComplaintType : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
