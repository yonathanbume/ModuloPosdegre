using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class VirtualClassCredential : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Site { get; set; }
        public ICollection<VirtualClass> VirtualClasses { get; set; }
    }
}
