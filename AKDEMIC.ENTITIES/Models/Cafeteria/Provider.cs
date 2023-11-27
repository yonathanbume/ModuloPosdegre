using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    public class Provider : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }        
        public string UserId { get; set; }
        public Guid? CareerId { get; set; }
        public byte ScholarshipType { get; set; }
        public decimal Cost { get; set; }
        public bool Group { get; set; }
        public string Code { get; set; }
        public string ContactName { get; set; }
        public string PhoneContact { get; set; }
        public ApplicationUser User { get; set; }
        public Career Career { get; set; }

    }
}
