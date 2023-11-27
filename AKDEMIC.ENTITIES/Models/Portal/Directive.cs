using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class Directive : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        
        public bool IsLink { get; set; }

        public string Path { get; set; }
    }
}
