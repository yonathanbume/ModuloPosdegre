using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencyManagementDocument : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsLink { get; set; }
    }
}
